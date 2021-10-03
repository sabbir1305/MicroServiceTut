using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaltformServiceAPI.AsyncDataServices;
using PaltformServiceAPI.Data;
using PaltformServiceAPI.Dtos;
using PaltformServiceAPI.Models;
using PaltformServiceAPI.SyncDataServices.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaltformServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private IPlatformRepo _repo;
        private IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBus;

        public PlatformsController(
            IPlatformRepo repo , 
            IMapper mapper,
            ICommandDataClient commandDataClient,
            IMessageBusClient messageBus
            )
        {
            _repo = repo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBus = messageBus;
            Console.WriteLine("Controller");
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
        {
            var platformItems = _repo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}",Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platform = _repo.GetPlatformById(id);
            if (platform == null)
                return NotFound();

            return Ok(_mapper.Map<PlatformReadDto>(platform));
        }

        [HttpPost]
        public async  Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platform = _mapper.Map<Platform>(platformCreateDto);

            _repo.CreatePlatform(platform);
            _repo.SaveChanges();
            var readDto = _mapper.Map<PlatformReadDto>(platform);

            try
            {
                await _commandDataClient.SendPatformToCommad(readDto);
            }

            catch(Exception ex)
            {
                Console.WriteLine($"Failed to send message .->{ex.Message}");
            }

            try
            {
                var publishedDto = _mapper.Map<PlatformPublishDto>(readDto);
                publishedDto.Event = "Platform_Published";
                _messageBus.PublishNewPatform(publishedDto);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Async message failed {ex.Message}");
            }
          
            return CreatedAtRoute(nameof(GetPlatformById),new {readDto.Id }, readDto);
        }
    }
}
