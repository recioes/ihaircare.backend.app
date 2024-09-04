using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IScheduleService
    {
        Task CreateScheduleAsync(Schedule schedule);
        Task UpdateScheduleAsync(Schedule schedule);
        Task DeleteScheduleAsync(Guid id);
        Task GetById(Guid id);
        Task<IEnumerable<Schedule>> GetScheduleAsync();

    }
}
