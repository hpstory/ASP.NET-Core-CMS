using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Entities;
using WebApplication.Infrastructure.Services;

namespace WebApplication.Infrastructure
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IBannersRepository _bannersRepository = null;
        private ICategoriesRepository _categoriesRepository = null;
        private IArticlesRepository _articlesRepository = null;
        public CMSDbContext _dbContext { get; }
        public RepositoryWrapper(CMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IBannersRepository Banners => _bannersRepository ?? new BannersRepository(_dbContext);
        public ICategoriesRepository Categories => _categoriesRepository ?? new CategoriesRepository(_dbContext);
        public IArticlesRepository Articles => _articlesRepository ?? new ArticlesRepository(_dbContext);
    }
}
