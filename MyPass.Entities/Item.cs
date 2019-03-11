﻿using System;
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

        [Display(Name = "Açıklama"), DataType(DataType.MultilineText), StringLength(500)]
        public string Description { get; set; }

        [Display(Name = "Şifre"), Required, StringLength(100)]
        public string Password { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public ItemType ItemTypeId { get; set; }

        public enum ItemType{
            Password = 1,
            List = 2,

        }
    }
}
