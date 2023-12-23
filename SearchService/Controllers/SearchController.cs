using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Search;
using MongoDB.Entities;

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
    // Search using the Query String parameters
    public async Task<ActionResult<List<HeartRateDate>>> SearchDates([FromQuery] SearchParams searchParams)
    {
        var query = DB.PagedSearch<HeartRateDate, HeartRateDate>(); // In order to use Pagination, you should use PagedSearch instead of Find
        // var query = DB.Find<HeartRateDate>();
        // query.Sort(x => x.Descending(a => a.dateTime)); // Sorts HeartRates in order of their dates


        // Below is how to match for datetime 
        //query.Match(x => x.dateTime == searchTerm);//.SortByTextScore();

        if (!string.IsNullOrEmpty(searchParams.SearchTerm)) //!= DateTime.MinValue)
        {
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
            // query.Match(Search.Full, searchParams.SearchTerm.ToString()).SortByTextScore();
        }

        query = searchParams.OrderBy switch
        {
            "HeartRate" => query.Sort(x => x.Descending(a => a.dateTime)),
            _ => query.Sort(x => x.Descending(a => a.dateTime)) // This is the "default" parameter
        };
        query = searchParams.FilterBy switch
        {
            "Today" => query.Match(x => x.dateTime == DateTime.UtcNow),
            _ => query.Match(x => x.dateTime <= DateTime.UtcNow) // This is the "default" parameter
        };
        
        // Pagination
        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);

        var result = await query.ExecuteAsync();

        return Ok(new
        {
            result = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });


    }
}