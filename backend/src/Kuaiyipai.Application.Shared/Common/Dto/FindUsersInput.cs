using Kuaiyipai.Dto;

namespace Kuaiyipai.Common.Dto
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }
    }
}