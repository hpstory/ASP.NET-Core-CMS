using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication.Entities;
using WebApplication.Models;

namespace WebApplication.Infrastructure.Services
{
    public class BannersRepository : RepositoryBase<Banners, int>, IBannersRepository
    {
        public BannersRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
