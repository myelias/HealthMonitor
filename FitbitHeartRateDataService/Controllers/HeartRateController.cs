using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitbitHeartRateDataService.Data;
using FitbitHeartRateDataService.DTOs;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitbitHeartRateDataService.Controllers;

[ApiController]
[Route("api/HeartRate")]
public class HeartRateController : ControllerBase
{
    private readonly HeartRateDbContext _context;
    private readonly IMapper _mapper;

    private readonly IPublishEndpoint _publishEndpoint;

    public HeartRateController(HeartRateDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint) // Dependency injection
    {
        _context = context;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<List<HeartRateDto>>> GetAllHeartRates(string date)
    {
        var query = _context.HeartRates.OrderBy(x => x.Date).AsQueryable();
        if (!string.IsNullOrEmpty(date))
        {
            query = query.Where(x => x.Date.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        //var HeartRates = await _context.HeartRates.ToListAsync();
        //return _mapper.Map<List<HeartRateDto>>(HeartRates);
        return await query.ProjectTo<HeartRateDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HeartRateDto>> GetSingleHeartRate(Guid id)
    {
        var HeartRate = await _context.HeartRates.FirstOrDefaultAsync(x => x.Id == id);

        // check to see if Id is available
        if (HeartRate == null) return NotFound();
        return _mapper.Map<HeartRateDto>(HeartRate);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteHeartRate(Guid id)
    {
        var HeartRate = await _context.HeartRates.FindAsync(id);

        if (HeartRate == null) return NotFound();

        _context.HeartRates.Remove(HeartRate);

        var result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not update DB");

        return Ok();
    }
}