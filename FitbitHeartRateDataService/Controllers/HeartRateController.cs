using AutoMapper;
using FitbitHeartRateDataService.Data;
using FitbitHeartRateDataService.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitbitHeartRateDataService.Controllers;

[ApiController]
[Route("api/HeartRate")]
public class HeartRateController : ControllerBase
{
    private readonly HeartRateDbContext _context;
    private readonly IMapper _mapper;

    public HeartRateController(HeartRateDbContext context, IMapper mapper) // Dependency injection
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<HeartRateDto>>> GetAllHeartRates()
    {
        var HeartRates = await _context.HeartRates.ToListAsync();
        return _mapper.Map<List<HeartRateDto>>(HeartRates);

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HeartRateDto>> GetSingleHeartRate(Guid id)
    {
        var HeartRate = await _context.HeartRates.FirstOrDefaultAsync(x => x.Id == id);

        // check to see if Id is available
        if (HeartRate == null) return NotFound();
        return _mapper.Map<HeartRateDto>(HeartRate);
    }
}