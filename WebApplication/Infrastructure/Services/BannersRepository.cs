using Microsoft.EntityFrameworkCore;
using WebApplication.Entities;

namespace WebApplication.Infrastructure.Services
{
    public class BannersRepository : RepositoryBase<Banners, int>, IBannersRepository
    {
        public BannersRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
