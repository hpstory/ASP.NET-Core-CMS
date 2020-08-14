using WebApplication.Entities;
using WebApplication.Infrastructure.PropertyMapping;
using WebApplication.Infrastructure.Services;

namespace WebApplication.Infrastructure
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly IBannersRepository _bannersRepository = null;
        private readonly ICategoriesRepository _categoriesRepository = null;
        private readonly IArticlesRepository _articlesRepository = null;
        private readonly ICommentsRepository _commentsRepository = null;
        public CMSDbContext DbContext { get; }
        private readonly IPropertyMappingService _propertyMappingService;
        public RepositoryWrapper(CMSDbContext dbContext, IPropertyMappingService propertyMappingService)
        {
            DbContext = dbContext;
            _propertyMappingService = propertyMappingService;
        }
        public IBannersRepository Banners => _bannersRepository ?? new BannersRepository(DbContext);
        public ICategoriesRepository Categories => _categoriesRepository ?? new CategoriesRepository(DbContext);
        public IArticlesRepository Articles => _articlesRepository ?? new ArticlesRepository(DbContext, _propertyMappingService);
        public ICommentsRepository Comments => _commentsRepository ?? new CommentsRepository(DbContext, _propertyMappingService);
    }
}
