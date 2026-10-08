using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeoxisAuthApp.Models;
using NeoxisAuthApp.ViewModels;
using System.Security.Claims;

namespace NeoxisAuthApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ─── Email / Password Login ────────────────────────────────────────────

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToAction("Index", "Dashboard");
                }
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
            }
            return View(model);
        }

        // ─── Registration ──────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email    = model.Email,
                    FullName = model.FullName
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Dashboard");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        // ─── Logout ────────────────────────────────────────────────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        // ─── External Login (Google / Microsoft) ───────────────────────────────

        /// <summary>
        /// Initiates the OAuth redirect to the external provider (Google or Microsoft).
        /// Called when the user clicks "Sign in with Google" or "Sign in with Microsoft".
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties  = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        /// <summary>
        /// Handles the callback from Google or Microsoft after the user authenticates.
        /// Creates a new Identity user on first login, or signs in an existing one.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            returnUrl ??= Url.Action("Index", "Dashboard")!;

            // Provider returned an error (user cancelled, denied access, etc.)
            if (remoteError != null)
            {
                TempData["Error"] = $"External login was cancelled or returned an error.";
                return RedirectToAction(nameof(Login));
            }

            // Retrieve the external login info from the cookie set by the provider callback
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["Error"] = "Could not retrieve your external login information. Please try again.";
                return RedirectToAction(nameof(Login));
            }

            // Try to sign in using the existing external login link (returning user)
            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "Dashboard");
            }

            // First-time user — create an Identity account linked to the external provider
            var email    = info.Principal.FindFirstValue(ClaimTypes.Email);
            var fullName = info.Principal.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Your external account did not provide an email address. Please use email/password registration instead.";
                return RedirectToAction(nameof(Login));
            }

            // Check if an account with this email already exists (created via email/password)
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                // Link the external provider to the existing account and sign in
                await _userManager.AddLoginAsync(existingUser, info);
                await _signInManager.SignInAsync(existingUser, isPersistent: false);
                return RedirectToAction("Index", "Dashboard");
            }

            // Create a brand-new user from the external provider info
            var newUser = new ApplicationUser
            {
                UserName = email,
                Email    = email,
                FullName = fullName,
                EmailConfirmed = true   // trust the provider's verified email
            };

            var createResult = await _userManager.CreateAsync(newUser);
            if (createResult.Succeeded)
            {
                await _userManager.AddLoginAsync(newUser, info);
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                return RedirectToAction("Index", "Dashboard");
            }

            // Creation failed — show errors
            TempData["Error"] = "Could not create your account: " +
                string.Join(" ", createResult.Errors.Select(e => e.Description));
            return RedirectToAction(nameof(Login));
        }

        // ─── Access Denied ─────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult AccessDenied() => View();
    }
}
