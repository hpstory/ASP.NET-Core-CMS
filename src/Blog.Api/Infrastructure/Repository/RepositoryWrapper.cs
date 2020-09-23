using Blog.Api.Entities;
using Blog.Api.Infrastructure.PropertyMapping;
using Blog.Api.Infrastructure.Services;

namespace Blog.Api.Infrastructure.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly IBannersRepository _bannersRepository = null;
        private readonly ICategoriesRepository _categoriesRepository = null;
        private readonly IArticlesRepository _articlesRepository = null;
        private readonly ICommentsRepository _commentsRepository = null;
        public BlogDbContext DbContext { get; }
        private readonly IPropertyMappingService _propertyMappingService;
        public RepositoryWrapper(BlogDbContext dbContext, IPropertyMappingService propertyMappingService)
        {
            DbContext = dbContext;
            _propertyMappingService = propertyMappingService;
        }
        public IBannersRepository Banners => _bannersRepository ?? new BannersRepository(DbContext, _propertyMappingService);
        public ICategoriesRepository Categories => _categoriesRepository ?? new CategoriesRepository(DbContext);
        public IArticlesRepository Articles => _articlesRepository ?? new ArticlesRepository(DbContext, _propertyMappingService);
        public ICommentsRepository Comments => _commentsRepository ?? new CommentsRepository(DbContext, _propertyMappingService);
    }
}
