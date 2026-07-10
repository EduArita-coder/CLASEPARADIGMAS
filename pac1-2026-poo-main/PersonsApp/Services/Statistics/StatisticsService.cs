using Microsoft.EntityFrameworkCore;
using PersonsApp.Constants;
using PersonsApp.Database;
using PersonsApp.Dtos.Common;
using PersonsApp.Dtos.Statistics;

namespace PersonsApp.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly PersonsDbContext _context;

        public StatisticsService(
            PersonsDbContext context
        )
        {
            _context = context;
        }

        public async Task<ResponseDto<StatisticsDto>> GetCounts()
        {
            var statistics = new StatisticsDto();

            statistics.PersonsCount = await _context.Persons.CountAsync();
            statistics.UsersCount = await _context.Users.CountAsync();

            return new ResponseDto<StatisticsDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = HttpMessageResponse.REGISTERS_FOUND,
                Data = statistics
            };

        }
    }
}