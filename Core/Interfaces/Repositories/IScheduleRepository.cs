using Core.Entities;
using MongoDB.Bson;

namespace Infrastructure
{
    public interface IScheduleRepository
    {
        Task AddAsync(Schedule schedule);
        Task UpdateAsync(Schedule schedule);
        Task DeleteAsync(ObjectId id);
        Task<Schedule> GetByIdAsync(ObjectId id);
        Task<IEnumerable<Schedule>> GetAsync();
    }
}
