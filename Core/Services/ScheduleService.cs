using Core.Entities;
using Core.Interfaces.Services;
using Infrastructure;

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
            throw new NotImplementedException();
        }

        public async Task DeleteScheduleAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Schedule>> GetScheduleAsync()
        {
            throw new NotImplementedException();
        }

        public async Task GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateScheduleAsync(Schedule schedule)
        {
            throw new NotImplementedException();
        }
    }
}
