using WebApplication.Infrastructure.Services;

namespace WebApplication.Infrastructure
{
    public interface IRepositoryWrapper
    {
        IBannersRepository Banners { get; }
        ICategoriesRepository Categories { get; }
        IArticlesRepository Articles { get; }
        ICommentsRepository Comments { get; }
        IUsersRepository Users { get; }
    }
}
