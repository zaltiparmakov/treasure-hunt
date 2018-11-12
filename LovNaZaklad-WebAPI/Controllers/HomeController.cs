using LovNaZaklad_WebAPI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace LovNaZaklad_WebAPI.Controllers
{
    public class HomeController : Controller
    {
        private LovNaZakladDbContext db = new LovNaZakladDbContext();

        public ActionResult Index()
        {
            ViewBag.Title = "Lov na Zaklad";

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Models.User user)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            User userToFind = db.Users.FirstOrDefault(u => u.Username == user.Username);
            if (userToFind != null)
            {
                if (Crypto.VerifyHashedPassword(userToFind.Password, user.Password))
                {
                    SignIn(userToFind);
                } else
                {
                    ViewBag.Error = "Wrong password!";
                }
            }
            else
            {
                ViewBag.Error = "User not found!";
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "UserID, Email, Username, Password, FirstName, LastName, DateOfBirth, RoleID")] User user)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                if (db.Users.SingleOrDefault(u => u.Username == user.Username) != null)
                {
                    ViewBag.Error("User exist");
                    return View();
                }

                db.Users.Add(new Models.User
                {
                    DateOfBirth = user.DateOfBirth,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Points = 0,
                    Email = user.Email,
                    Username = user.Username,
                    Password = Crypto.HashPassword(user.Password),
                    RoleID = db.Roles.SingleOrDefault(r => r.RoleName == "user").RoleID
                });

                db.SaveChanges();
                SignIn(user);
            } else
            {
                ModelState.AddModelError("", "Data is incorrect");
            }

            return View(user);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Panel()
        {
            return View();
        }
        
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void SignIn(User user)
        {
            string role;
            try
            {
                role = db.Roles.Find(user.RoleID).RoleName;
            } catch(Exception e)
            {
                role = "user";
            }
            ClaimsIdentity claims = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            claims.AddClaim(new Claim(ClaimTypes.Name, user.Username));
            claims.AddClaim(new Claim(ClaimTypes.Role, role));
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()));

            HttpContext.GetOwinContext().Authentication.SignIn(claims);
        }
    }
}
