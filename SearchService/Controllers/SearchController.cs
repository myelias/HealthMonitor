using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace SearchService;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<HeartRateDate>>> SearchDates(string dateTime, int pageNumber = 1, int pageSize = 4)
    {
        var query = DB.PagedSearch<HeartRateDate>();

        query.Sort(x => x.Ascending(a => a.dateTime)); // Sorts HeartRates in order of their dates

        if (!string.IsNullOrEmpty(dateTime))
        {
            query.Match(Search.Full, dateTime).SortByTextScore();
        }

        query.PageNumber(pageNumber);
        query.PageSize(pageSize);

        var result = await query.ExecuteAsync();

        return Ok(new{
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });


    }
}