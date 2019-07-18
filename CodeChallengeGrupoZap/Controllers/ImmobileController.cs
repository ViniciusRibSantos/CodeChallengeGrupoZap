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

        public ImmobileController(IImmobileService immobileService)
        {
            _immobileService = immobileService;
        }

        [Route("[action]")]
        [HttpGet]
        public JsonResult FilterByZap()
        {
            Response response = MountResponse(_immobileService.FilterByZap());

            return Json(response);
        }
        
        [Route("[action]")]
        [HttpGet]
        public JsonResult FilterByVivareal()
        {
            Response response = MountResponse(_immobileService.FilterByVivareal());

            return Json(response);
        }

        public Response MountResponse(IList<Immobile> properties)
        {
            Response response = new Response();

            response.PageSize = 20;
            response.PageNumber = properties.Count % response.PageSize == 0 ? properties.Count / response.PageSize : (properties.Count / response.PageSize) + 1;
            response.TotalCount = properties.Count;
            response.Listings = properties;

            return response;
        }
    }
}