using System.Collections.Generic;
using CodeChallengeGrupoZap.Domain.Entities;
using CodeChallengeGrupoZap.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeChallengeGrupoZap.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImmobileController : ControllerBase
    {
        private readonly IImmobileService _immobileService;

        public ImmobileController(IImmobileService immobileService)
        {
            _immobileService = immobileService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Immobile>> Get()
        {
            _immobileService.FilterByZap();
            
            return null;
        }
    }
}