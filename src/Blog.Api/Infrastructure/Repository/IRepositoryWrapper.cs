using Blog.Api.Infrastructure.Services;

namespace Blog.Api.Infrastructure.Repository
{
    public interface IRepositoryWrapper
    {
        IBannersRepository Banners { get; }
        ICategoriesRepository Categories { get; }
        IArticlesRepository Articles { get; }
        ICommentsRepository Comments { get; }
    }
}
