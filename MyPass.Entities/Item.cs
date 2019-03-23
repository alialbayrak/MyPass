using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace MyPass.Entities
{
    [Table("Items")]
    public class Item : EntityBase
    {
        [Display(Name = "Başlık"), Required, StringLength(50)]
        public string Title { get; set; }

        [Display(Name = "Notlar"), DataType(DataType.MultilineText), Column(TypeName = "varchar")]
        public string Nodes { get; set; }

        [Display(Name = "URL"), Column(TypeName = "varchar")]
        public string Url { get; set; }

        [Display(Name = "Kullanıcı Adı"), Column(TypeName = "varchar"), StringLength(50)]
        public string Username { get; set; }

        [Display(Name = "Şifre"), Required, Column(TypeName = "varchar"), StringLength(100)]
        public string Password { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }

}
