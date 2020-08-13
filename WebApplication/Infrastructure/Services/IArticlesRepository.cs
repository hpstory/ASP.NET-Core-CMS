using System.Threading.Tasks;
using WebApplication.Controllers.DtoParameters;
using WebApplication.Entities;
using WebApplication.Helpers;

namespace WebApplication.Infrastructure.Services
{
    public interface IArticlesRepository: IRepositoryBase<Articles>, IRepositoryBaseById<Articles, int>
    {
        Task<PagedList<Articles>> GetAllAsync(ArticleResourceParameters parameters);
    }
}
