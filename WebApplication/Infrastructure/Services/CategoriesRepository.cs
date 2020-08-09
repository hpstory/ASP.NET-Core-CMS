using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Entities;

namespace WebApplication.Infrastructure.Services
{
    public class CategoriesRepository : RepositoryBase<Categories, int>, ICategoriesRepository
    {
        public CategoriesRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
