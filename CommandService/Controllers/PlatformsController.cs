using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _commandRepo;
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepo commandRepo,IPlatformRepo platformRepo,IMapper imapper)
        {
            _commandRepo = commandRepo;
            _platformRepo = platformRepo;
            _mapper = imapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting platforms");

            var platforms = _platformRepo.GetPlatforms();

            return _mapper.Map<List<PlatformReadDto>>(platforms);
        }

        [HttpPost]
        public ActionResult CreatePlatform()
        {
            Console.WriteLine("--> Inbound message");
            return Ok("Sync message");
        }


    }
}
