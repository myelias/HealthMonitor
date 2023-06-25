using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;
using System.Threading.Tasks;


namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // Decorator
    public class PlatformsController : ControllerBase
    {   
        private IPlatformRepo _repository;
        private IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        // This is the standard dependency injection pattern you see: Constructor for a class that will pass in
        // a number of parameters that will assign them to private read only fields which we will use in that class
        public PlatformsController(
        IPlatformRepo repository, 
        IMapper mapper, 
        ICommandDataClient commandDataClient) //These 2 parameters are being injected into this constructor
        {   
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            
        }

        [HttpGet] // When you call this action through the above route, we are going to return an enumeration
        // of our PlatformReadDto
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms...");
            var platformItem = _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItem));
        }

        [HttpGet("{id}", Name = "GetPlatformById")] //All method signatures must be unique or error
        // Use Get an individual Platform to retrieve a platform by ID
        public ActionResult<PlatformReadDto>GetPlatformById(int id)
        {
            var platformItem = _repository.GetPlatformById(id);
            if (platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }
            return NotFound();
        }

        [HttpPost]
        //Use POST Create Platform to hit this endpoint
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();

            //mapping to PlatformReadDto from platformModel
            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
           
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }
            
            
            return CreatedAtRoute(nameof(GetPlatformById), new{Id = platformReadDto.Id}, platformReadDto);
        }
    }
}
//yes