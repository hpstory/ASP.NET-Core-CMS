using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Infrastructure.Services;

namespace WebApplication.Infrastructure
{
    public interface IRepositoryWrapper
    {
        IBannersRepository Banners { get; }
        ICategoriesRepository Categories { get; }
        IArticlesRepository Articles { get; }
    }
}
