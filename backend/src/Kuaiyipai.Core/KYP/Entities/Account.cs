using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Kuaiyipai.Authorization.Users;

namespace Kuaiyipai.KYP.Entities
{
    [Table("KYP_Account")]
    public class Account : Entity<long>, ISoftDelete
    {
        [Required]
        public string WeChatId { get; set; }

        [Required]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public double Budget { get; set; }

        public bool IsDeleted { get; set; }
    }
}