using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Entities;
using WebApplication.Infrastructure;
using WebApplication.Models.Comments;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api")]
    public class CommentsController : ControllerBase
    {
        public IRepositoryWrapper _repositoryWrapper { get; }
        public IMapper _mapper { get; }

        private readonly CMSDbContext _dbContext;
        public CommentsController(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            CMSDbContext dbContext
            )
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _dbContext = dbContext;
        }
        [HttpGet("comments")]
        public async Task<ActionResult<CommentsDto>> GetCommentsAsync()
        {
            var comments = _repositoryWrapper.Comments.GetAllAsync();

        }


    }
}
