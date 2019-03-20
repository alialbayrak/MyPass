using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyPass.Entities
{
    [Table("Users")]
    public class User : EntityBase
    {
        [Required, StringLength(25)]
        public string Name { get; set; }

        [Required, StringLength(25)]
        public string Surname { get; set; }

        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool IsActive { get; set; }

        public DateTime? PasswordResetExpiryDate { get; set; }

        [Display(Name = "Şifre"), Required, DataType(DataType.Password), MinLength(8, ErrorMessage = "Şifreniz en az 8 karakter olmalıdır!")]
        [RegularExpression("^(?=.*[0-9])(?=.*[A-Z])(?=.*[a-zA-Z]).{1,}$", ErrorMessage = "Şifreniz en az bir rakam ve bir büyük harf olmalıdır")]
        public string Password { get; set; }

        public virtual List<CategoryUser> CategoryUsers { get; set; }  

    }
}
