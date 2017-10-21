using VoloApi.Services.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VoloApi.Services;

namespace VolaApi.Services.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        // GET: Manage
        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePassswordSuccess ? "Your password has been changed"
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set"
                : "";

            var userId = User.Identity.GetUserId();

            var model = new ManageViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };

            //Verify User Role
            if(IsAdmin(User.Identity.GetUserId()) == true)
            {
                ViewBag.Administrator = true;
            }

            return View(model);
        }

        // GET : /Manage/Register
        [Authorize(Roles = "Administrator")]
        public ActionResult RegisterAccount()
        {

                var model = new RegisterBindingModel
                {
                    Roles = ListRoles()
                };
                return View(model);

        }

        // POST : /Manage/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterAccount(RegisterBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, model.RoleId);
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            //Add Roles to DropDownList
            model.Roles = ListRoles();
            return View(model);
        }

        //Auth/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //POST: /Auth/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePassswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        [Authorize(Roles = ("Administrator, Developer"))]
        //GET: /Manage/ManageUser
        public ActionResult ManageUser(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
             message == ManageMessageId.UpdatedUserSuccess ? "User Updated"
             : "";
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var query = from user in userManager.Users
                        from role in roleManager.Roles
                        where role.Users.Any(r => r.UserId == user.Id)

                        select new UserRoleViewModel
                        {
                            UserId = user.Id,
                            User = user.UserName,
                            Role = role.Name
                        };

            //Verify User Role
            if (IsAdmin(User.Identity.GetUserId()))
            {
                ViewBag.Administrator = true;
                ViewBag.AdminId = User.Identity.GetUserId();
            }
            ViewBag.userList = query;
            return View();
        }

        [Authorize(Roles = ("Administrator"))]
        //GET: /Manage/CustomizeUser/{id}
        public async Task<ActionResult> CustomizeUser(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var _userRoleNameId = await UserRoleNameId(id);

                //Get Name Role from user Id
                string oldRoleName = _userRoleNameId.Select(r => r.Value).FirstOrDefault();

                var email = UserManager.GetEmail(id);
                ViewBag.UserId = id;
                ViewBag.Email = email;
                ViewBag.OldRole = oldRoleName;
                var model = new CustomizeUserBindingModel
                {
                    Email = email,
                    Roles = ListRoles() //Add Roles to DropDownList
                };
                return View(model);
            }

            return RedirectToAction("Index");
        }
        //POST: /Manage/CustomizeUser
        [HttpPost]
        public async Task<ActionResult> CustomizeUser(CustomizeUserBindingModel model, string id)
        {
            var _userRoleNameId = await UserRoleNameId(id);

            //Get Name Role from user Id
            string oldRoleName = _userRoleNameId.Select( r => r.Value).FirstOrDefault();

            if (ModelState.IsValid)
            {
                UserManager.RemoveFromRole(id, oldRoleName);
                var resp = UserManager.AddToRole(id, model.RoleId);

                if (resp.Succeeded)
                {
                    return RedirectToAction("ManageUser", new { Message = ManageMessageId.UpdatedUserSuccess });
                }
                //Model Errors 
                AddErrors(resp);
            }

            //ViewBag Views
            ViewBag.Email = UserManager.GetEmail(id);
            ViewBag.OldRole = oldRoleName;
            //Add Roles to DropDownList
            model.Roles = ListRoles();
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private IQueryable<IdentityRole> ListRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            return roleManager.Roles;
        }

        public enum ManageMessageId
        {
            ChangePassswordSuccess,
            UpdatedUserSuccess,
            SetPasswordSuccess
        }

        public bool IsAdmin(string userId)
        {
            if (UserManager.IsInRole(userId, "Administrator"))
            {
                return true;
            }
            return false;
        }

        //Get Id and Name Of User Roles
        public async Task<Dictionary<string, string>> UserRoleNameId(string userId)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var oldUser = await UserManager.FindByIdAsync(userId);
            var oldRoleId = oldUser.Roles.Select(r => r.RoleId).SingleOrDefault();
            var oldRoleName = roleManager.Roles.SingleOrDefault(r => r.Id == oldRoleId).Name;
            Dictionary<string, string> userNameId = new Dictionary<string, string>();
            userNameId.Add(oldRoleId, oldRoleName);
            return userNameId;

        }

        #endregion Helpers
    }
}