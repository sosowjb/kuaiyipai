using System.Collections.Generic;
using Kuaiyipai.Auditing.Dto;
using Kuaiyipai.Dto;

namespace Kuaiyipai.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);
    }
}
