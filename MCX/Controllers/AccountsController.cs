using MCX.Models.DbEntities;
using MCX.Models.Tables;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MCX.Controllers
{

    //[Authorize]
    public class AccountsController : Controller
    {
        // GET: Accounts
        [AllowAnonymous]
        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(Users obj)
        {
            using (DbEntities Context = new DbEntities())
            {
                var count = await Context.Users
                    .FirstOrDefaultAsync(x => x.Username == obj.Username && x.Password == obj.Password && x.IsActive == true && x.IsDelete == false);


                if (count != null)
                {
                    @ViewBag.msg = "Success";
                    //FormsAuthentication.SetAuthCookie(obj.Username, true);


                    Session["LoggedInUser"] = count;


                    AuthenticationAccess(count.EmailId, true, count.LoginId, count.UserType);
                    //// reset request.isauthenticated
                    //var authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    //if (authCookie != null)
                    //{
                    //    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    //    if (authTicket != null && !authTicket.Expired)
                    //    {
                    //        var roles = authTicket.UserData.Split(',');
                    //        System.Web.HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new FormsIdentity(authTicket), roles);
                    //    }
                    //}
                    return RedirectToAction("Index", "DashBoard");
                }
                else
                {
                    @ViewBag.msg = "Invalid user name and password.";
                    return RedirectToAction("Index", "Users");
                }


            }

        }


        [NonAction]
        public void AuthenticationAccess(string EmailId, bool RememberMe, long UserId, string UserType)
        {
            FormsAuthentication.SetAuthCookie(EmailId, RememberMe);
            //string roles = "Admin,Member";
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
              1,
             EmailId,                          //user Name
              DateTime.Now,
              DateTime.Now.AddDays(1),                      // expiry in 1 day
              RememberMe,
              Convert.ToString(UserId));
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                               FormsAuthentication.Encrypt(authTicket));
            Response.Cookies.Add(cookie);
            Session["UserType"] = UserType; //_IAccount.GetUserType(UserId);

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();

            return View("Index");
        }



        public async Task<ActionResult> DashBoard()
        {
            return View();
        }

    }
}