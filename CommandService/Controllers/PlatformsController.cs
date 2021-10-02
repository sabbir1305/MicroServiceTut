using AutoMapper;
using CommandService.Data;
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

    }
}
