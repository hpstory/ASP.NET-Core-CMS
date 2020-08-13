using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication.Controllers.DtoParameters;
using WebApplication.Entities;
using WebApplication.Helpers;
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
        [HttpGet("comments", Name = nameof(GetCommentsAsync))]
        public async Task<ActionResult<IEnumerable<CommentsDto>>> GetCommentsAsync([FromQuery] CommentResourceParameters parameters)
        {
            var comments = await _repositoryWrapper.Comments.GetAllAsync(parameters);
            var previousPageLink = comments.HasPrevious ?
                CreateCommentsResourceUri(parameters, ResourceUriType.PreviousPage) : null;
            var nextPageLink = comments.HasNext ?
                CreateCommentsResourceUri(parameters, ResourceUriType.NextPage) : null;
            var paginationMetadata = new
            {
                totalCount = comments.TotalCount,
                pageSize = comments.PageSize,
                currentPage = comments.CurrentPage,
                totalPages = comments.TotalPages,
                previousPageLink,
                nextPageLink
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata,
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }));
            var returnDto = _mapper.Map<IEnumerable<CommentsDto>>(comments);
            return Ok(returnDto);
        }
        [HttpPost("comment")]
        public async Task<ActionResult<CommentsDto>> CreateCommentAsync(CommentsAddDto comment)
        {
            var entity = _mapper.Map<Comments>(comment);
            _repositoryWrapper.Comments.Create(entity);
            await _repositoryWrapper.Articles.SaveAsync();
            var returnDto = _mapper.Map<CommentsDto>(entity);
            return CreatedAtRoute(nameof(GetCommentsAsync), new { commentId = returnDto.ID }, returnDto);
        }
        [HttpDelete("comment/{commentId}")]
        public async Task<IActionResult> DeleteCommentAsync(int commentId)
        {
            var entity = await _repositoryWrapper.Comments.GetByIdAsync(commentId);
            if (entity == null)
            {
                return NotFound();
            }
            _repositoryWrapper.Comments.Delete(entity);
            await _repositoryWrapper.Comments.SaveAsync();
            return NoContent();
        }
        private string CreateCommentsResourceUri(CommentResourceParameters parameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetCommentsAsync), new
                    {
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetCommentsAsync), new
                    {
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                    });
                default:
                    return Url.Link(nameof(GetCommentsAsync), new
                    {
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                    });
            }
        }
    }
}
