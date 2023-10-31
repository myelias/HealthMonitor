using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using ZstdSharp.Unsafe;

namespace SearchService;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    /** This Controller should be able to search HeartRates by different metrics
    Ex:
    HeartRate by Date
    HeartRate by Period
    HeartRate by Today
    **/
    [HttpGet]
    public async Task<ActionResult<List<HeartRateDate>>> SearchDates([FromQuery]SearchParams searchParams)
    {
        var query = DB.PagedSearch<HeartRateDate, HeartRateDate>();

        //query.Sort(x => x.Ascending(a => a.dateTime)); // Sorts HeartRates in order of their dates

        if (searchParams.SearchTerm != DateTime.MinValue)
        {
            query.Match(Search.Full, searchParams.SearchTerm.ToString()).SortByTextScore();
        }

        query = searchParams.OrderBy switch
        {
            "HeartRate" => query.Sort(x => x.Descending(a => a.dateTime)),
            _ => query.Sort(x => x.Descending(a => a.dateTime)) // This is the "default" parameter
        };
        query = searchParams.FilterBy switch
        {
            "Today" => query.Match(x => x.dateTime == DateTime.UtcNow),
            _ => query.Match(x => x.dateTime <= DateTime.UtcNow)
        };
        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);

        var result = await query.ExecuteAsync();

        return Ok(new{
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });


    }
}