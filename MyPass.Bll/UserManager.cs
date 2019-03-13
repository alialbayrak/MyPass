using MyPass.Entities;
using MyPass.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MyPass.Bll.Helper;

namespace MyPass.Bll
{
    public class UserManager
    {
        UserDal _dal = new UserDal();

        public int RegisterUser(User user)
        {
            if(_dal.GetUserByEmailAdress(user.Email) == null)
            {
                string passwordHash = SecurityHelper.GenerateSHA256String(user.Password);
                user.Password = passwordHash;

                user.IsActive = false;
                
                int userId = _dal.Add(user);

                SendVerifyAccountEmail(user.Email);

                return userId;
            }
            else
            {
                throw new Exception("E-posta adresi zaten kayıtlı!");
            }
        }

        public User LoginUser(string email, string password)
        {
            string passwordHash = SecurityHelper.GenerateSHA256String(password);

            User user = _dal.GetUserbyEmailPassword(email, passwordHash);
            if (user != null)
            {
                if (user.IsActive)
                {
                    return user;
                }
                throw new Exception("Hesabınız aktifleştirilmemiş. Lütfen e-postanıza gelen aktifleştirme linkine tıklayınız.");
                
            }
            else
            {
                throw new Exception("E-posta ve şifre eşleşmiyor!");
            }
            
        }

        public void SendVerifyAccountEmail(string to)
        {
            string appName = ConfigHelper.GetSetting<string>("AppName");
            string siteUrl = ConfigHelper.GetSetting<string>("SiteUrl");
            User user = _dal.GetUserByEmailAdress(to);
            string userIdEnCode = SecurityHelper.Encode(user.Id.ToString());
            string subject = $"{appName} Üyelik Aktivasyonu!";
            string htmlMessage = $"<p>Üyelik kayıt işleminiz gerçekleştirilmiştir. Hesabınızı aktifleştirmek istiyorsanız " +
                $"<a href='{siteUrl}/User/Activation/{userIdEnCode}'>buraya tıklayın</a></p>";

            EmailHelper email = new EmailHelper();
            email.SendEmail(subject, htmlMessage, to);
        }

        public void ActivateUser(string encodedId)
        {
            int deCodedUserId = Convert.ToInt32(SecurityHelper.Decode(encodedId));
            User user = _dal.GetById(deCodedUserId);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı");

            user.IsActive = true;
            _dal.Update(user);
        }

        public void ResetPasswordRequest(string email)
        {
            User user = _dal.GetUserByEmailAdress(email);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı!");

            if (!user.IsActive)
                throw new Exception("Şifrenizi sıfırlamak için hesabınızı aktif etmeniz gerekmektedir!");

            user.PasswordResetExpiryDate = DateTime.Now.AddHours(1);

            if(_dal.Update(user) > 0)
            {
                SendResetPasswordEmail(user.Email);
            }
            else
                throw new Exception("Şifre sıfırlama işlemi başarısız!");

        }

        public void SendResetPasswordEmail(string to)
        {
            string appName = ConfigHelper.GetSetting<string>("AppName");
            string siteUrl = ConfigHelper.GetSetting<string>("SiteUrl");
            User user = _dal.GetUserByEmailAdress(to);
            string userIdEnCode = SecurityHelper.Encode(user.Id.ToString());
            string subject = $"{appName} - Şifre Sıfırlama!";
            string htmlMessage = $"<p>Şifre sıfırlamak için " +
                $"<a href='{siteUrl}/User/NewPassword/{userIdEnCode}'>buraya tıklayın</a></p>";

            EmailHelper email = new EmailHelper();
            email.SendEmail(subject, htmlMessage, to);
        }

        public void ResetPassword(string encodedId, string newPassword)
        {
            int deCodedUserId = Convert.ToInt32(SecurityHelper.Decode(encodedId));
            User user = _dal.GetById(deCodedUserId);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı!");

            if (user.PasswordResetExpiryDate == null)
                throw new Exception("Sıfırlama isteği oluşturulmamış!");

            if (user.PasswordResetExpiryDate < DateTime.Now)
                throw new Exception("Sıfırlama işleminiz süresi geçtiği için şifreniz sıfırlanamıyor. Yeni bir istek oluşturun!");

            if(!user.IsActive)
                throw new Exception("Şifrenizi sıfırlamak için hesabınızı aktif etmeniz gerekmektedir!");


            string passHash = SecurityHelper.GenerateSHA256String(newPassword);
            user.Password = passHash;
            user.PasswordResetExpiryDate = null;

            _dal.Update(user);
        }
    }
}
