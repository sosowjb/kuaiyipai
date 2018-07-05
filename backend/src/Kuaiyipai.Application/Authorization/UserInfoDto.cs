using Abp.Application.Services.Dto;

namespace Kuaiyipai.Authorization
{
    public class UserInfoDto : EntityDto<long>
    {
        public string NickName { get; set; }

        public string AvatarUrl { get; set; }
    }
}