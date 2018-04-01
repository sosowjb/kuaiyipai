﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Kuaiyipai.KYP.Entities
{
    [Table("KYP_Pillars")]
    public class Pillar : Entity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(3)]
        public string Code { get; set; }
    }
}