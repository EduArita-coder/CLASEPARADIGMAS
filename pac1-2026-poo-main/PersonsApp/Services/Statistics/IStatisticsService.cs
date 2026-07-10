using PersonsApp.Dtos.Common;
using PersonsApp.Dtos.Statistics;

namespace PersonsApp.Services.Statistics
{
    public interface IStatisticsService
    {
        Task<ResponseDto<StatisticsDto>> GetCounts();
    }
}