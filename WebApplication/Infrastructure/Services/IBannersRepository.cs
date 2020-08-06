using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Entities;
using WebApplication.Models;

namespace WebApplication.Infrastructure.Services
{
    public interface IBannersRepository : IRepositoryBase<Banners>, IRepositoryBaseById<Banners, int>
    {
    }
}
