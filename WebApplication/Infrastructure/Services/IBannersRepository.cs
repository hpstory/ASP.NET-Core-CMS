using WebApplication.Entities;

namespace WebApplication.Infrastructure.Services
{
    public interface IBannersRepository : IRepositoryBase<Banners>, IRepositoryBaseById<Banners, int>
    {
    }
}
