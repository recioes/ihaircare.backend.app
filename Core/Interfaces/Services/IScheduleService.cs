using Core.DTOs;
using Core.Entities;
using MongoDB.Bson;

namespace Core.Interfaces.Services
{
    public interface IScheduleService
    {
        Task CreateScheduleAsync(ScheduleDto scheduleDto);
        Task UpdateScheduleAsync(ObjectId scheduleId, ScheduleDto scheduleDto);
        Task DeleteScheduleAsync(ObjectId id);
        Task<Schedule> GetScheduleById(ObjectId id);
        Task<IEnumerable<Schedule>> GetScheduleAsync();

    }
}
