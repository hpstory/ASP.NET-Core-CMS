using Microsoft.EntityFrameworkCore;
using WebApplication.Entities.Identity.Entities;

namespace WebApplication.Infrastructure.Services
{
    public class UsersRepository : RepositoryBase<User, string>, IUsersRepository
    {
        public UsersRepository(DbContext dbContext): base(dbContext)
        {

        }
    }
}
