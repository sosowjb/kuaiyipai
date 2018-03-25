using System.Collections.Generic;
using Kuaiyipai.Authorization.Users.Dto;
using Kuaiyipai.Dto;

namespace Kuaiyipai.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}