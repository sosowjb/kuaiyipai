using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Kuaiyipai.MultiTenancy.Accounting.Dto;

namespace Kuaiyipai.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
