using Blog.Api.Entities;
using Blog.Api.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Infrastructure.Services
{
    public interface IBannersRepository : IRepositoryBase<Banners>, IRepositoryBaseById<Banners, int>
    {
        new Task<IEnumerable<Banners>> GetAllAsync();
    }
}
