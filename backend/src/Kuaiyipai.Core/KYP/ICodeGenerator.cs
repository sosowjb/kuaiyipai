using Abp.Domain.Services;
using Kuaiyipai.KYP.Entities;

namespace Kuaiyipai.KYP
{
    public interface ICodeGenerator : IDomainService
    {
        string GenItemCode(Item item);

        string GenOrderCode(Order order);
    }
}