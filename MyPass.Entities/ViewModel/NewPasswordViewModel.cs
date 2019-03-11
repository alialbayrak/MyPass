using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPass.Entities.ViewModel
{
    public class NewPasswordViewModel
    {
        [Required]
        public string Id { get; set; }

        [Display(Name = "Şifre"), Required, DataType(DataType.Password), MinLength(8, ErrorMessage = "Şifreniz en az 8 karakter olmalıdır!")]
        [RegularExpression("^(?=.*[0-9])(?=.*[A-Z])(?=.*[a-zA-Z]).{1,}$", ErrorMessage = "Şifreniz en az bir rakam ve bir büyük harf olmalıdır")]
        public string Password { get; set; }

        [Display(Name = "Şifre Tekrar"), Required, Compare(nameof(Password))]
        public string PasswordRepeat { get; set; }
    }
}
