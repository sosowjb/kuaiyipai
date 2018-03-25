using Abp.Notifications;
using Kuaiyipai.Dto;

namespace Kuaiyipai.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }
    }
}