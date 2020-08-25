using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Entities;

namespace WebApplication.Infrastructure.Services
{
    public interface ICategoriesRepository : IRepositoryBase<Categories>, IRepositoryBaseById<Categories, int>
    {
        Task<IEnumerable<Categories>> GetAllCategoriesAsync();
    }
}
