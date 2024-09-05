using Core.Entities;
using MongoDB.Bson;

namespace Core.Interfaces.Services
{
    public interface IScheduleService
    {
        Task CreateScheduleAsync(Schedule schedule);
        Task UpdateScheduleAsync(Schedule schedule);
        Task DeleteScheduleAsync(ObjectId id);
        Task<Schedule> GetScheduleById(ObjectId id);
        Task<IEnumerable<Schedule>> GetScheduleAsync();

    }
}
