using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ADE_ManagementSystem.Models;
using ADE_ManagementSystem.Models.User;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace ADE_ManagementSystem.Controllers.Admin
{
    [Authorize]
    public class ManageUserController : Controller
    {
        private Entities db = new Entities();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IMapper _mapper;

        public ManageUserController()
        {
        }

        public ManageUserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IMapper mapper)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _mapper = mapper;
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


        // GET: ManageUser
        public async Task<ActionResult> Index()
        {
            var loginUserId = User.Identity.GetUserId();
            //var data = await db.AspNetUsers.Where(x => x.AspNetRoles.Count(y => y.Id != "Admin") == 0).ToListAsync();
            var data = from user in db.AspNetUsers
                where user.Id != loginUserId
                select new UserViewModel()
                {
                    FullName = user.FullName,
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    UserRole =
                        user.AspNetRoles.FirstOrDefault() == null ? "N/A" : user.AspNetRoles.FirstOrDefault().Name
                };
            return View(data);
        }

        // GET: ManageUser/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // GET: ManageUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManageUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,FullName,ImageExtension,UserRole")] UserViewModel aspNetUser)
        {
            if (ModelState.IsValid)
            {
                var loginuser = await UserManager.FindByEmailAsync(User.Identity.GetUserName()); //this will work because username will be equal to email for admin


                var user = new ApplicationUser { UserName = aspNetUser.Email, Email = aspNetUser.Email };
                var result = await UserManager.CreateAsync(user, "123");
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, aspNetUser.UserRole);
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                    await SignInManager.SignInAsync(loginuser, isPersistent: false, rememberBrowser: false);


                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    var dbUser = await db.AspNetUsers.FirstOrDefaultAsync(x => x.UserName.Equals(aspNetUser.Email));
                    dbUser.FullName = aspNetUser.FullName;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                AddErrors(result);


                //return RedirectToAction("Index");
            }

            return View(aspNetUser);
        }

        // GET: ManageUser/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            UserViewModel userViewModel = Mapper.Map<UserViewModel>(aspNetUser);
            var userRole = aspNetUser.AspNetRoles.FirstOrDefault();
            if (userRole != null)
            {
                userViewModel.UserRole = userRole.Name;
            }
            return View(userViewModel);
        }

        // POST: ManageUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,FullName,ImageExtension,UserRole")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                AspNetUser aspNetUser = Mapper.Map<AspNetUser>(userViewModel);

                var query = (from user in db.AspNetUsers
                    where user.Id == userViewModel.Id
                    select new
                    {
                        Role = user.AspNetRoles.FirstOrDefault()
                    }).FirstOrDefault();

                string oldRoleName = null;
                if (query != null && query.Role != null)
                {
                    oldRoleName = query.Role.Name;
                }
                
                var userIdentity = (ClaimsIdentity)User.Identity;
                
                if (oldRoleName != null && oldRoleName != userViewModel.UserRole)
                {
                    await UserManager.RemoveFromRoleAsync(aspNetUser.Id, oldRoleName);
                    await UserManager.AddToRoleAsync(aspNetUser.Id, userViewModel.UserRole);
                }
                else if (oldRoleName == null)
                {
                    await UserManager.AddToRoleAsync(aspNetUser.Id, userViewModel.UserRole);
                }
                db.Entry(aspNetUser).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(userViewModel);
        }

        // GET: ManageUser/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: ManageUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            db.AspNetUsers.Remove(aspNetUser);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword(string id)
        {
            ViewBag.UserId = id;
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordForAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            ApplicationUser user1 = await UserManager.FindByIdAsync(model.UserId);
            if (user1 == null)
            {
              //  return NotFound();
            }
            user1.PasswordHash = UserManager.PasswordHasher.HashPassword(model.NewPassword);
            var result1 = await UserManager.UpdateAsync(user1);
            if (result1.Succeeded)
            {
                return RedirectToAction("Index");
            }

            //var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            //if (result.Succeeded)
            //{
            //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            //    if (user != null)
            //    {
            //        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            //    }
            //    return RedirectToAction("Index");
            //    //return RedirectToAction("Index", new { Message = ManageController.ManageMessageId.ChangePasswordSuccess });
            //}
            AddErrors(result1);
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
