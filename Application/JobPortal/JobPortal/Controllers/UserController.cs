using DbLayer;
using JobPortal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private CareerConnectDbEntities DB = new CareerConnectDbEntities();

        public ActionResult NewUser()
        {
            return View(new UserMv());
        }
        [HttpPost] 
        [ValidateAntiForgeryToken]
        public ActionResult NewUser(UserMv userMv)
        {
            if (ModelState.IsValid)
            {
                var checkUser = DB.UserTables.Where(u => u.Email == userMv.Email).FirstOrDefault();
                if (checkUser != null)
                {
                    ModelState.AddModelError("Email", "Email is already Registered");
                    return View(userMv);
                }
                checkUser = DB.UserTables.Where(u => u.UserName == userMv.UserName).FirstOrDefault();
                if (checkUser != null)
                {
                    ModelState.AddModelError("UserName", "UserName is already Registered");
                    return View(userMv);
                }


                using(var trans = DB.Database.BeginTransaction())
                {
                    try
                    {
                        var user = new UserTable();
                        user.UserName = userMv.UserName;
                        user.Password = userMv.Password;
                        user.ContactNo = userMv.ContactNo;
                        user.Email = userMv.Email;
                        user.UserTypeID = userMv.AreYouProvider == true ? 2 : 3;
                        DB.UserTables.Add(user);
                        DB.SaveChanges();

                        if (userMv.AreYouProvider == true)
                        {
                            var company = new CompanyTable();
                            company.UserID = user.UserID;
                            
                            if (string.IsNullOrEmpty(userMv.Company.EmailAddress))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.EmailAddress", "Required");
                                return View(userMv);
                            }
                            if (string.IsNullOrEmpty(userMv.Company.CompanyName))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.CompanyName", "Required");
                                return View(userMv);
                            }
                            if (string.IsNullOrEmpty(userMv.Company.PhoneNo))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.PhoneNo", "Required");
                                return View(userMv);
                            }
                            if (string.IsNullOrEmpty(userMv.Company.Description))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.Description", "Required");
                                return View(userMv);
                            }

                            company.EmailAddress = userMv.Company.EmailAddress;
                            company.CompanyName = userMv.Company.CompanyName;
                            company.ContactNo = userMv.ContactNo;
                            company.PhoneNo = userMv.Company.PhoneNo;
                            company.Logo = "~/Content/assets/img/logo/logo.png";
                            company.Description = userMv.Company.Description;
                            DB.CompanyTables.Add(company);
                            DB.SaveChanges();
                        }
                        trans.Commit();
                        return RedirectToAction("Login");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Please provide correct details");
                        trans.Rollback();
                    }

                }


               
            }
            return View(userMv);
        }
        public ActionResult Login()
        {
            return View(new UserLoginMv());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginMv userLoginMv)
        {
            if (ModelState.IsValid)
            {
                var user = DB.UserTables.Where(u => u.UserName == userLoginMv.UserName && u.Password == userLoginMv.Password).FirstOrDefault();
                if(user == null)
                {
                    ModelState.AddModelError(string.Empty, "Username or Password is incorrect!");
                    return View(userLoginMv);
                }
                Session["UserID"] = user.UserID;
                Session["UserName"] = user.UserName;
                Session["UserTypeID"] = user.UserTypeID;
                if(user.UserTypeID == 2)
                {
                    Session["CompanyID"] = user.CompanyTables.FirstOrDefault().CompanyID;
                }

               return RedirectToAction("Index","Home");
            }
            return View(userLoginMv);
        }
     
        public ActionResult Logout()
        {
            Session["UserID"] = string.Empty;
            Session["UserName"] = string.Empty;
            Session["CompanyID"] = string.Empty;
            Session["UserTypeID"] = string.Empty;
            return RedirectToAction("Index","Home");
        }

        public ActionResult AllUsers()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var users = DB.UserTables.ToList();
            return View(users);
        }

        public ActionResult Forgot()
        {
            return View(new ForgotPasswordMv());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        /* public ActionResult Forgot(ForgotPasswordMv forgotPasswordMv)
         {
             var user = DB.UserTables.Where(u => u.Email == forgotPasswordMv.EmailAddress).FirstOrDefault();
             if (user != null)
             {
                 string userandpassword = "UserName: " + user.UserName + "\n" + "Password" + user.Password;

                 string body = userandpassword;
                 bool IsSendEmail = JobPortal.Forgot.EmailSend.SendEmail(user.Email, "Account Details", body, true);
                 if (IsSendEmail)
                 {
                     ModelState.AddModelError(string.Empty, "UserName and Password are sent!");
                 }
                 else
                 {
                     ModelState.AddModelError("Email", "Email Registered! Current Email sending is not working properly, Try Again Later");

                 }
             }
             else
             {
                 ModelState.AddModelError("EmailAddress", "Email is not Registered!");
             }

             return View(forgotPasswordMv);

         }*/


        public ActionResult Forgot(ForgotPasswordMv forgotPasswordMv)
        {
            if (ModelState.IsValid)
            {
                var user = DB.UserTables.FirstOrDefault(u => u.Email == forgotPasswordMv.EmailAddress);

                if (user != null)
                {
                    string subject = "Password Recovery - Job Portal";
                    string body = $"Hello {user.UserName},<br/><br/>" +
                                  $"Here are your account details:<br/>" +
                                  $"Username: <b>{user.UserName}</b><br/>" +
                                  $"Password: <b>{user.Password}</b><br/><br/>" +
                                  $"Please change your password immediately if you suspect unauthorized access.<br/><br/>" +
                                  $"Regards,<br/>Job Portal Team";

                    bool emailSent = JobPortal.Forgot.EmailSend.SendEmail(user.Email, subject, body, true);

                    if (emailSent)
                    {
                        ViewBag.Message = "Account details have been sent to your registered email.";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to send email. Please try again later.");
                    }
                }
                else
                {
                    ModelState.AddModelError("EmailAddress", "This email is not registered.");
                }
            }

            return View(forgotPasswordMv);
        }
    }
}






