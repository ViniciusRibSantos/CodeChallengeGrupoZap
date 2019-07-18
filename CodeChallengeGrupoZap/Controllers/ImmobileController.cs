using System.Collections.Generic;
using CodeChallengeGrupoZap.Domain.Entities;
using CodeChallengeGrupoZap.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeChallengeGrupoZap.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImmobileController : Controller
    {
        private readonly IImmobileService _immobileService;
        private int DefaultPageSize { get; set; }

        public ImmobileController(IImmobileService immobileService)
        {
            _immobileService = immobileService;
            DefaultPageSize = 20;
        }

        [Route("[action]")]
        [HttpGet]
        public JsonResult FilterByZap()
        {
            Response response = MountResponse(_immobileService.FilterByZap(), DefaultPageSize);

            return Json(response);
        }
        
        [Route("[action]")]
        [HttpGet]
        public JsonResult FilterByVivareal()
        {
            Response response = MountResponse(_immobileService.FilterByVivareal(), DefaultPageSize);

            return Json(response);
        }

        public Response MountResponse(IList<Immobile> properties, int pageSize)
        {
            Response response = new Response();

            response.PageSize = pageSize;
            response.PageNumber = GeneratePageNumber(pageSize, properties.Count);
            response.TotalCount = properties.Count;
            response.Listings = properties;

            return response;
        }

        public int GeneratePageNumber(int pageSize,  int totalCount)
        {
            if(pageSize % totalCount == 0)
                return totalCount / pageSize;
            else
                return (totalCount / pageSize) + 1;
        }
    }
}