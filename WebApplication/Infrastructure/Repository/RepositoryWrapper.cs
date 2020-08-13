using WebApplication.Entities;
using WebApplication.Infrastructure.PropertyMapping;
using WebApplication.Infrastructure.Services;

namespace WebApplication.Infrastructure
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IBannersRepository _bannersRepository = null;
        private ICategoriesRepository _categoriesRepository = null;
        private IArticlesRepository _articlesRepository = null;
        private ICommentsRepository _commentsRepository = null;
        public CMSDbContext _dbContext { get; }
        private readonly IPropertyMappingService _propertyMappingService;
        public RepositoryWrapper(CMSDbContext dbContext, IPropertyMappingService propertyMappingService)
        {
            _dbContext = dbContext;
            _propertyMappingService = propertyMappingService;
        }
        public IBannersRepository Banners => _bannersRepository ?? new BannersRepository(_dbContext);
        public ICategoriesRepository Categories => _categoriesRepository ?? new CategoriesRepository(_dbContext);
        public IArticlesRepository Articles => _articlesRepository ?? new ArticlesRepository(_dbContext, _propertyMappingService);
        public ICommentsRepository Comments => _commentsRepository ?? new CommentsRepository(_dbContext, _propertyMappingService);
    }
}
