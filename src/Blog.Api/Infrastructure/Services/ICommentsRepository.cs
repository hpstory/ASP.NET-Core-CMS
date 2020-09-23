using Blog.Api.Entities;
using Blog.Api.Helpers;
using Blog.Api.Infrastructure.Repository;
using Blog.Api.Models.DtoParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Infrastructure.Services
{
    public interface ICommentsRepository : IRepositoryBase<Comments>, IRepositoryBaseById<Comments, int>
    {
        Task<PagedList<Comments>> GetAllAsync(CommentResourceParameters parameters);
    }
}
