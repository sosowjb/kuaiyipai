using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Kuaiyipai.Auction.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kuaiyipai.Auction
{
    public class PillarAndCategoryManager : KuaiyipaiDomainServiceBase, IPillarAndCategoryManager
    {
        private readonly IRepository<Pillar> _pillarRepository;
        private readonly IRepository<Category> _categoryRepository;

        public PillarAndCategoryManager(IRepository<Pillar> pillarRepository, IRepository<Category> categoryRepository)
        {
            _pillarRepository = pillarRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<int> CreatePillar(string pillarName)
        {
            var maxCode = await _pillarRepository.GetAll().MaxAsync(p => p.Code);
            var nextCode = (Convert.ToInt32(maxCode) + 1).ToString().PadLeft(3, '0');
            var pillar = new Pillar
            {
                Code = nextCode,
                Name = pillarName
            };
            return await _pillarRepository.InsertAndGetIdAsync(pillar);
        }

        public async Task UpdatePillar(int pillarId, string pillarName)
        {
            var pillar = await _pillarRepository.GetAsync(pillarId);
            pillar.Name = pillarName;
            await _pillarRepository.UpdateAsync(pillar);
        }

        [UnitOfWork]
        public async Task DeletePillar(int pillarId, bool recursive)
        {
            if (recursive)
            {
                await _categoryRepository.DeleteAsync(category => category.PillarId == pillarId);
            }
            else
            {
                var hasCategories = await _categoryRepository.GetAll().AnyAsync(category => category.PillarId == pillarId);
                if (hasCategories)
                {
                    throw new UserFriendlyException("大类下有未删除的小类，不能删除此大类");
                }
            }
            await _pillarRepository.DeleteAsync(pillarId);
        }

        public async Task<int> CreateCategory(string categoryName, int pillarId)
        {
            var maxCode = await _categoryRepository.GetAll().MaxAsync(c => c.Code);
            var nextCode = (Convert.ToInt32(maxCode) + 1).ToString().PadLeft(5, '0');
            var category = new Category
            {
                Code = nextCode,
                Name = categoryName,
                PillarId = pillarId
            };
            return await _categoryRepository.InsertAndGetIdAsync(category);
        }

        public async Task UpdateCategory(int categoryId, string categoryName)
        {
            var category = await _categoryRepository.GetAsync(categoryId);
            category.Name = categoryName;
            await _categoryRepository.UpdateAsync(category);
        }

        public async Task MoveCategory(int categoryId, int pillarId)
        {
            var category = await _categoryRepository.GetAsync(categoryId);
            category.PillarId = pillarId;
            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteCategory(int categoryId)
        {
            await _categoryRepository.DeleteAsync(categoryId);
        }
    }
}