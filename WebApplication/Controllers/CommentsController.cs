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
        public IRepositoryWrapper RepositoryWrapper { get; }
        public IMapper Mapper { get; }

        private readonly CMSDbContext _dbContext;
        public CommentsController(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            CMSDbContext dbContext
            )
        {
            RepositoryWrapper = repositoryWrapper;
            Mapper = mapper;
            _dbContext = dbContext;
        }
        [HttpGet("comments", Name = nameof(GetCommentsAsync))]
        public async Task<ActionResult<IEnumerable<CommentsDto>>> GetCommentsAsync([FromQuery] CommentResourceParameters parameters)
        {
            var comments = await RepositoryWrapper.Comments.GetAllAsync(parameters);
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
            var returnDto = Mapper.Map<IEnumerable<CommentsDto>>(comments);
            return Ok(returnDto);
        }
        [HttpPost("comment")]
        public async Task<ActionResult<CommentsDto>> CreateCommentAsync(CommentsAddDto comment)
        {
            var entity = Mapper.Map<Comments>(comment);
            RepositoryWrapper.Comments.Create(entity);
            await RepositoryWrapper.Articles.SaveAsync();
            var returnDto = Mapper.Map<CommentsDto>(entity);
            return CreatedAtRoute(nameof(GetCommentsAsync), new { commentId = returnDto.ID }, returnDto);
        }
        [HttpDelete("comment/{commentId}")]
        public async Task<IActionResult> DeleteCommentAsync(int commentId)
        {
            var entity = await RepositoryWrapper.Comments.GetByIdAsync(commentId);
            if (entity == null)
            {
                return NotFound();
            }
            RepositoryWrapper.Comments.Delete(entity);
            await RepositoryWrapper.Comments.SaveAsync();
            return NoContent();
        }
        private string CreateCommentsResourceUri(CommentResourceParameters parameters, ResourceUriType type)
        {
            return type switch
            {
                ResourceUriType.PreviousPage => Url.Link(nameof(GetCommentsAsync), new
                {
                    pageNumber = parameters.PageNumber - 1,
                    pageSize = parameters.PageSize,
                    orderBy = parameters.OrderBy,
                }),
                ResourceUriType.NextPage => Url.Link(nameof(GetCommentsAsync), new
                {
                    pageNumber = parameters.PageNumber + 1,
                    pageSize = parameters.PageSize,
                    orderBy = parameters.OrderBy,
                }),
                _ => Url.Link(nameof(GetCommentsAsync), new
                {
                    pageNumber = parameters.PageNumber,
                    pageSize = parameters.PageSize,
                    orderBy = parameters.OrderBy,
                }),
            };
        }
    }
}
