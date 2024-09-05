using Core.Entities;
using Core.Interfaces.Services;
using Infrastructure;
using MongoDB.Bson;

namespace Core.Services
{
    public class ScheduleService : IScheduleService
    {

        private readonly IScheduleRepository _scheduleRepository; 

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task CreateScheduleAsync(Schedule schedule)
        {
            await _scheduleRepository.AddAsync(schedule);
        }

        public async Task DeleteScheduleAsync(ObjectId id)
        {
            await _scheduleRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Schedule>> GetScheduleAsync()
        {
            return await _scheduleRepository.GetAsync();
        }

        public async Task<Schedule> GetScheduleById(ObjectId id)
        {
            return await _scheduleRepository.GetByIdAsync(id);
        }

        public async Task UpdateScheduleAsync(Schedule schedule)
        {
            await _scheduleRepository.UpdateAsync(schedule);
        }
    }
}
