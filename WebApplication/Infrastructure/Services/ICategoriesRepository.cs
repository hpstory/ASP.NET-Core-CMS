using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Entities;

namespace WebApplication.Infrastructure.Services
{
    public interface ICategoriesRepository : IRepositoryBase<Categories>, IRepositoryBaseById<Categories, int>
    {

    }
}
