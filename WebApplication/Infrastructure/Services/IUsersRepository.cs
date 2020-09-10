using WebApplication.Entities.Identity.Entities;

namespace WebApplication.Infrastructure.Services
{
    public interface IUsersRepository : IRepositoryBase<User>, IRepositoryBaseById<User, string>
    {
    }
}
