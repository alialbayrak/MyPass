using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyPass.Entities
{
    [Table("Groups")]
    public class Group : EntityBase
    {
        [Display(Name = "Başlık"), Required, StringLength(50)]
        public string Title { get; set; }

        [Display(Name = "Açıklama"), DataType(DataType.MultilineText), StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int OwnerUserId { get; set; }

        public virtual List<Item> ItemList { get; set; }

    }
}
