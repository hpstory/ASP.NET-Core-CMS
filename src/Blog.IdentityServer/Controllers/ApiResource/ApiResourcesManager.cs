using Blog.IdentityServer.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.IdentityServer.Controllers.ApiResource
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ApiResourcesManager : Controller
    {
        private readonly ConfigurationDbContext _configurationDbContext;

        public ApiResourcesManager(ConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
        }

        /// <summary>
        /// 数据列表页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _configurationDbContext.ApiResources
                .Include(d => d.UserClaims)
                .ToListAsync());
        }

        /// <summary>
        /// 数据修改页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "SuperAdmin")]
        public IActionResult CreateOrEdit(int id)
        {
            ViewBag.ApiResourceId = id;
            return View();
        }


        [HttpGet]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<ResponseMessage<ApiResourceDto>> GetDataById(int id = 0)
        {
            var model = (await _configurationDbContext.ApiResources
                .Include(d => d.UserClaims)
                .ToListAsync()).FirstOrDefault(d => d.Id == id).ToModel();

            var apiResourceDto = new ApiResourceDto();
            if (model != null)
            {
                apiResourceDto = new ApiResourceDto()
                {
                    Name = model?.Name,
                    DisplayName = model?.DisplayName,
                    Description = model?.Description,
                    UserClaims = string.Join(",", model?.UserClaims),
                };
            }

            return new ResponseMessage<ApiResourceDto>()
            {
                Success = true,
                Message = "获取成功",
                Response = apiResourceDto
            };
        }

        [HttpPost]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<ResponseMessage<string>> SaveData(ApiResourceDto request)
        {
            if (request != null && request.Id == 0)
            {
                IdentityServer4.Models.ApiResource apiResource = new IdentityServer4.Models.ApiResource()
                {
                    Name = request?.Name,
                    DisplayName = request?.DisplayName,
                    Description = request?.Description,
                    Enabled = true,
                    UserClaims = request?.UserClaims?.Split(","),
                };

                var result = (await _configurationDbContext.ApiResources.AddAsync(apiResource.ToEntity()));
                await _configurationDbContext.SaveChangesAsync();
            }

            if (request != null && request.Id > 0)
            {

                var modelEF = (await _configurationDbContext.ApiResources
                    .Include(d => d.UserClaims)
                    .ToListAsync()).FirstOrDefault(d => d.Id == request.Id);

                modelEF.Name = request?.Name;
                modelEF.DisplayName = request?.DisplayName;
                modelEF.Description = request?.Description;

                var apiResourceClaim = new List<IdentityServer4.EntityFramework.Entities.ApiResourceClaim>();
                if (!string.IsNullOrEmpty(request?.UserClaims))
                {
                    request?.UserClaims.Split(",").Where(s => s != "" && s != null).ToList().ForEach(s =>
                    {
                        apiResourceClaim.Add(new IdentityServer4.EntityFramework.Entities.ApiResourceClaim()
                        {
                            ApiResource = modelEF,
                            ApiResourceId = modelEF.Id,
                            Type = s
                        });
                    });
                    modelEF.UserClaims = apiResourceClaim;
                }


                var result = (_configurationDbContext.ApiResources.Update(modelEF));
                await _configurationDbContext.SaveChangesAsync();
            }


            return new ResponseMessage<string>()
            {
                Success = true,
                Message = "添加成功",
            };
        }
    }
}
