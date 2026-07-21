using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonsApp.Dtos.Common;
using PersonsApp.Dtos.Statistics;
using PersonsApp.Services.Statistics;

namespace PersonsApp.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statistics;

        public StatisticsController(
            IStatisticsService statistics
        )
        {
            _statistics = statistics;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<StatisticsDto>>> GetCounts()
        {
            var response = await _statistics.GetCounts();
            
            return StatusCode(response.StatusCode, new ResponseDto<StatisticsDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }
    }
}