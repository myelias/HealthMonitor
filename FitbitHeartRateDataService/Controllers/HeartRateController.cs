using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using FitbitHeartRateDataService.Data;
using FitbitHeartRateDataService.DTOs;
using FitbitHeartRateDataService.Entities;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
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
        // var query = _context.HeartRates.OrderBy(x => x.Date).AsQueryable(); // Use AsQueryable to be able to further query
        
        // if (!string.IsNullOrEmpty(date))
        // {
        //     // Return HeartRates that are greater than the date query string
        //     query = query.Where(x => x.Date.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        // }

        var HeartRates = await _context.HeartRates.ToListAsync();
        return _mapper.Map<List<HeartRateDto>>(HeartRates);
        // return await query.ProjectTo<HeartRateDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<HeartRateDto>> PostAllHeartRates(CreateHeartRateDto createHeartRateDto) // We can have a "POST" method that saves the heart rates requested ?
    {
        var HeartRates = _mapper.Map<HeartRate>(createHeartRateDto); // Map from CreateHeartRateDto to HeartRate

        _context.HeartRates.Add(HeartRates); // Add that HeartRate to our DB

        var newHeartRate = _mapper.Map<HeartRateDto>(HeartRates); // Now we map from HeartRate to the HeartRateDto

        // Publish as a mapping from newHeartRate (HeartRateDto) -> HeartRatesCreated onto the queue
        await _publishEndpoint.Publish(_mapper.Map<HeartRatesCreated>(newHeartRate)); 

        var result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not save changes to the Database");

        return CreatedAtAction(nameof(GetSingleHeartRate), new {HeartRates.Id}, newHeartRate);
        // return await query.ProjectTo<HeartRateDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HeartRateDto>> GetSingleHeartRate(Guid id)
    {
        var HeartRate = await _context.HeartRates.FirstOrDefaultAsync(x => x.Id == id);

        // check to see if Id is available
        if (HeartRate == null) return NotFound();
        return _mapper.Map<HeartRateDto>(HeartRate);
    }

    //[Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteHeartRate(Guid id)
    {
        var HeartRate = await _context.HeartRates.FindAsync(id);

        if (HeartRate == null) return NotFound();

        // if (HeartRate.FitBitUser != User.Identity.Name) return Forbid();

        _context.HeartRates.Remove(HeartRate);

        await _publishEndpoint.Publish<HeartRateDeleted>(new {Id = HeartRate.Id.ToString()}); // Publish which Id needs to be deleted. 
        //The HeartRateDeletedConsumer will take care of the rest

        var result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not update DB");

        return Ok();
    }
}