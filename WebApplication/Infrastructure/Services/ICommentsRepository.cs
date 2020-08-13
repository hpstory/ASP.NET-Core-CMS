using System.Threading.Tasks;
using WebApplication.Controllers.DtoParameters;
using WebApplication.Entities;
using WebApplication.Helpers;

namespace WebApplication.Infrastructure.Services
{
    public interface ICommentsRepository : IRepositoryBase<Comments>, IRepositoryBaseById<Comments, int>
    {
        Task<PagedList<Comments>> GetAllAsync(CommentResourceParameters parameters);
    }
}
