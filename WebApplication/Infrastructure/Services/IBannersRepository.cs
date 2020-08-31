using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Entities;

namespace WebApplication.Infrastructure.Services
{
    public interface IBannersRepository : IRepositoryBase<Banners>, IRepositoryBaseById<Banners, int>
    {
        new Task<IEnumerable<Banners>> GetAllAsync();
    }
}
