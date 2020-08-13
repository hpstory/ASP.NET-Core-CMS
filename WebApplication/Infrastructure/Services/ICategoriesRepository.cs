using WebApplication.Entities;

namespace WebApplication.Infrastructure.Services
{
    public interface ICategoriesRepository : IRepositoryBase<Categories>, IRepositoryBaseById<Categories, int>
    {

    }
}
