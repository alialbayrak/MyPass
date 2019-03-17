using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPass.Entities
{
    public class EntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public bool Status { get; set; }

        [Display(Name = "Eklenme Tarihi"), Required, DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime AddedDate { get; set; }

        [Display(Name = "Güncellenme Tarihi"), DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? UpdatedDate { get; set; }
    }
}
