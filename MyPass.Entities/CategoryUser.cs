using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPass.Entities
{
    [Table("CategoryUsers")]
    public class CategoryUser : EntityBase
    {
        public int UserId { get; set; }

        public int CategoryId { get; set; }

        public bool IsOwner { get; set; }

        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
    }
}
