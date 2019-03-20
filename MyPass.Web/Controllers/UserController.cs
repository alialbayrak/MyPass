using MyPass.Entities;
using MyPass.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPass.Entities.ViewModel;
using MyPass.Web.Filter;
using MyPass.Web.Model;

namespace MyPass.Web.Controllers
{
    public class UserController : Controller
    {
        private UserManager _bll = new UserManager();

        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        #region Register

        public ActionResult Register()
        {
            return View(new User());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int userId = _bll.Add(user);

                    ViewBag.Caption = "Kaydınız Oluşturulmuştur";
                    ViewBag.Message = "Mail adresinizi kontrol edip hesabınızı aktive etmeyi unutmayın!";
                    ViewBag.Contoller = "User";
                    ViewBag.Action = "Login";
                    return View("~/Views/Shared/Redirect.cshtml");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(user);
                }

            }

            return View(user);
        }

        #endregion

        #region Login/Logout

        public ActionResult Login()
        {
            return View(new UserLoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(UserLoginViewModel userLogin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = _bll.Login(userLogin.Email, userLogin.Password);
                    if (user != null)
                    {
                        SessionHelper.AddCurrentUser(user);
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(userLogin);

        }

        public ActionResult Logout(int? id)
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        [AuthFilter]
        public ActionResult Details(int? id)
        {
            return View();
        }

        public ActionResult Activation(string id) //Encoded Id
        {
            try
            {
                _bll.ActivateUser(id);

                ViewBag.Caption = "Aktivasyon İşlemi Başarılı!";
                ViewBag.Message = "Artık üye girişi yapabilirsiniz. Yönlendiriliyorsunuz...";
                ViewBag.Contoller = "User";
                ViewBag.Action = "Login";
                return View("~/Views/Shared/Redirect.cshtml");
            }
            catch (Exception)
            {
                ViewBag.Caption = "Aktivasyon İşlemi Başarısız!";
                ViewBag.Message = "Opss... bir hata oluştu. aktivasyon linkini tekrar isteyin.";
                ViewBag.Contoller = "User";
                ViewBag.Action = "Login";
                ViewBag.Status = "Error";
                return View("~/Views/Shared/Redirect.cshtml");
            }

        }

        #region Password Reset

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _bll.ResetPasswordRequest(forgotPassword.Email);

                    ViewBag.Caption = "Şifre Sıfırlama";
                    ViewBag.Message = "E-posta adresinize şifre sıfırlama bağlantısı gönderilmiştir";
                    ViewBag.Contoller = "Home";
                    ViewBag.Action = "Index";
                    return View("~/Views/Shared/Redirect.cshtml");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(forgotPassword);
                }
            }
            
            return View(forgotPassword);
        }

        public ActionResult NewPassword(string id) //encoded Id
        {
            if (String.IsNullOrEmpty(id))
                throw new HttpException(404, "");

            return View(new NewPasswordViewModel() { Id = id });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NewPassword(NewPasswordViewModel newPassword) //encoded Id
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _bll.ResetPassword(newPassword.Id, newPassword.Password);

                    ViewBag.Caption = "Şifreniz Sıfırlama Başarılı";
                    ViewBag.Message = "Şifreniz sıfırlanmıştır. Yeni şifrenizle giriş yapabilirsiniz.";
                    ViewBag.Contoller = "User";
                    ViewBag.Action = "Login";
                    return View("~/Views/Shared/Redirect.cshtml");
                }
                return View(newPassword);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(newPassword);
            }

        }

        #endregion




    }
}
