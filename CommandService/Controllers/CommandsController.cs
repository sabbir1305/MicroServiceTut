using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _commandRepo;
        private readonly IMapper _mapper;
        private readonly IPlatformRepo _platformRepo;

        public CommandsController(ICommandRepo commandRepo ,IPlatformRepo platformRepo, IMapper mapperRepo)
        {
            _commandRepo = commandRepo;
            _mapper = mapperRepo;
            _platformRepo = platformRepo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($" --> {nameof(GetCommandsForPlatform)}");
            if (!_platformRepo.PlatformExists(platformId))
            {
                return NotFound();
            }
            var commands = _commandRepo.GetCommandsForPlatform(platformId);

            return _mapper.Map<List<CommandReadDto>>(commands);


        }

        [HttpGet("{commandId}",Name ="GetCommandForPlatform")]

        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($" --> {nameof(GetCommandForPlatform)}");
            if (!_platformRepo.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = _commandRepo.GetCommand(platformId, commandId);

            if (command == null)
                return NotFound(); 

            return _mapper.Map<CommandReadDto>(command);
        }


        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
        {
            Console.WriteLine($" --> {nameof(CreateCommandForPlatform)}");
            if (!_platformRepo.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandDto);

            _commandRepo.CreateCommand(platformId, command);
            _commandRepo.SaveChanges();

            var readDto = _mapper.Map<CommandReadDto>(command);
            return CreatedAtRoute(nameof(GetCommandForPlatform),
                new { platformId = platformId, commandId = readDto.Id },
                readDto
                );

        }

    }
}
