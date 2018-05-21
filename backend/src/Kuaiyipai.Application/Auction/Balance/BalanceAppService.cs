using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using Castle.Core.Internal;
using Kuaiyipai.Auction.Balance.Dto;
using Kuaiyipai.Auction.Entities;
using Kuaiyipai.Authorization.Users;
using Microsoft.EntityFrameworkCore;

namespace Kuaiyipai.Auction.Balance
{
    public class BalanceAppService : KuaiyipaiAppServiceBase, IBalanceAppService
    {
        private readonly IRepository<UserBalance, long> _balanceRepository;
        private readonly IRepository<UserBalanceRecord, Guid> _balanceRecordRepository;
        private readonly IRepository<User, long> _userRepository;

        public BalanceAppService(IRepository<UserBalance, long> balanceRepository, IRepository<UserBalanceRecord, Guid> balanceRecordRepository, IRepository<User, long> userRepository)
        {
            _balanceRepository = balanceRepository;
            _balanceRecordRepository = balanceRecordRepository;
            _userRepository = userRepository;
        }

        public async Task<GetMyBalanceOutputDto> GetMyBalance()
        {
            var balance = await _balanceRepository.FirstOrDefaultAsync(b => b.UserId == AbpSession.UserId);
            return new GetMyBalanceOutputDto
            {
                Total = balance.TotalBalance,
                Frozen = balance.FrozenBalance,
                Available = balance.TotalBalance - balance.FrozenBalance
            };
        }

        public async Task<PagedResultDto<GetMyBalanceRecordsOutputDto>> GetMyBalanceRecords(GetMyBalanceRecordsInputDto input)
        {
            var query = _balanceRecordRepository.GetAll().Where(b => b.UserId == AbpSession.UserId);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }
            else
            {
                query = query.OrderByDescending(b => b.RecordTime);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input)
                .Join(_userRepository.GetAll(), b => b.UserId, u => u.Id, (b, u) => new GetMyBalanceRecordsOutputDto
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    UserFullName = u.Name,
                    Amount = b.Amount,
                    RecordTime = b.RecordTime,
                    Remarks = b.Remarks
                }).ToListAsync();

            return new PagedResultDto<GetMyBalanceRecordsOutputDto>(count, list);
        }

        [UnitOfWork]
        public async Task Charge(ChargeInputDto input)
        {
            if (AbpSession.UserId.HasValue)
            {
                var balance = await _balanceRepository.FirstOrDefaultAsync(b => b.UserId == AbpSession.UserId);
                if (balance == null)
                {
                    await _balanceRepository.InsertAsync(new UserBalance
                    {
                        TotalBalance = input.Amount,
                        FrozenBalance = 0,
                        UserId = AbpSession.UserId.Value
                    });
                }
                else
                {
                    balance.TotalBalance += input.Amount;
                    await _balanceRepository.UpdateAsync(balance);
                }

                await _balanceRecordRepository.InsertAsync(new UserBalanceRecord
                {
                    Amount = input.Amount,
                    RecordTime = DateTime.Now,
                    Remarks = "充值",
                    UserId = AbpSession.UserId.Value
                });
            }
            else
            {
                throw new UserFriendlyException("用户未登录");
            }
        }

        [UnitOfWork]
        public async Task Withdraw(WithdrawInputDto input)
        {
            if (AbpSession.UserId.HasValue)
            {
                var balance = await _balanceRepository.FirstOrDefaultAsync(b => b.UserId == AbpSession.UserId);
                if (balance == null)
                {
                    throw new UserFriendlyException("没有足够的余额可提现");
                }

                if (balance.TotalBalance - balance.FrozenBalance < input.Amount)
                {
                    throw new UserFriendlyException("没有足够的余额可提现");
                }

                balance.TotalBalance -= input.Amount;
                await _balanceRepository.UpdateAsync(balance);

                await _balanceRecordRepository.InsertAsync(new UserBalanceRecord
                {
                    Amount = -input.Amount,
                    RecordTime = DateTime.Now,
                    Remarks = "提现",
                    UserId = AbpSession.UserId.Value
                });
            }
            else
            {
                throw new UserFriendlyException("用户未登录");
            }
        }
    }
}