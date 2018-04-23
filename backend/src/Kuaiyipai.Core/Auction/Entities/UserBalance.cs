using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Kuaiyipai.Authorization.Users;

namespace Kuaiyipai.Auction.Entities
{
    [Table("AUC_Balance")]
    public class UserBalance : Entity<long>
    {
        [Required]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public double TotalBalance { get; set; }

        public double FrozenBalance { get; set; }
    }
}