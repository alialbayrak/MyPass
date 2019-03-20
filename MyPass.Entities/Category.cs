using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyPass.Entities
{
    [Table("Categories")]
    public class Category : EntityBase
    {
        public Category()
        {
            this.CategoryUsers = new List<CategoryUser>();
            this.ItemList = new List<Item>();
        }

        [Display(Name = "Başlık"), Required, StringLength(50)]
        public string Title { get; set; }

        [Display(Name = "Açıklama"), DataType(DataType.MultilineText), StringLength(500)]
        public string Description { get; set; }

        public int? ParentCategoryId { get; set; }

        public virtual List<Item> ItemList { get; set; }
        public virtual List<CategoryUser> CategoryUsers { get; set; }

    }
}
