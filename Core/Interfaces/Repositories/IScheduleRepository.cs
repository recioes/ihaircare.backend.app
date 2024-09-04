using Core.Entities;

namespace Infrastructure
{
    public interface IScheduleRepository
    {
        Task AddAsync(Schedule schedule);
        Task UpdateAsync(Schedule schedule);
        Task DeleteAsync(Guid id);
        Task GetById(Guid id);
        Task<IEnumerable<Schedule>> GetAsync();
    }
}
