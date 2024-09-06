using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Services;
using FluentValidation;
using Infrastructure;
using MongoDB.Bson;

namespace Core.Services
{
    public class ScheduleService : IScheduleService
    {

        private readonly IScheduleRepository _scheduleRepository;
        private readonly IValidator<Schedule> _scheduleValidator;

        public ScheduleService(IScheduleRepository scheduleRepository, IValidator<Schedule> scheduleValidator)
        {
            _scheduleRepository = scheduleRepository;
            _scheduleValidator = scheduleValidator;
        }

        public async Task CreateScheduleAsync(ScheduleDto scheduleDto)
        {

            var schedule = new Schedule
            {
                Title = scheduleDto.Title,
                StartDate = scheduleDto.StartDate,
                EndDate = scheduleDto.EndDate,
                Treatments = scheduleDto.Treatments.Select(t => new Treatment
                {
                    Name = t.Name,
                    ScheduledDate = t.ScheduledDate,
                    Notes = t.Notes,
                    IsCompleted = false 

                }).ToList(),
                IsCompleted = false
            };
            var validationResult = await _scheduleValidator.ValidateAsync(schedule);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _scheduleRepository.AddAsync(schedule);
        }

        public async Task DeleteScheduleAsync(ObjectId id)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(id);

            if (schedule == null)
            {
                throw new InvalidOperationException("Schedule not found.");
            }
            await _scheduleRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Schedule>> GetScheduleAsync()
        {
            var existingSchedules =  await _scheduleRepository.GetAsync();
            if (existingSchedules == null)
            {
                throw new InvalidOperationException("there are no schedules created");
            }

            return existingSchedules;
        }

        public async Task<Schedule> GetScheduleById(ObjectId id)
        {
            var existingSchedule = await _scheduleRepository.GetByIdAsync(id);

            if (existingSchedule == null) 
            {
                throw new InvalidOperationException($"schedule with id{id} not found");
            }

            return existingSchedule;
        }

        public async Task UpdateScheduleAsync(ObjectId scheduleId, ScheduleDto scheduleDto)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(scheduleId);
            if (schedule == null)
            {
                throw new InvalidOperationException("Schedule not found.");
            }

            schedule.Title = scheduleDto.Title;
            schedule.StartDate = scheduleDto.StartDate;
            schedule.EndDate = scheduleDto.EndDate;

            schedule.Treatments = scheduleDto.Treatments.Select(t => new Treatment
            {
                Name = t.Name,
                ScheduledDate = t.ScheduledDate,
                FrequencyInDays = t.FrequencyInDays,
                Notes = t.Notes
            }).ToList();

            var validationResult = await _scheduleValidator.ValidateAsync(schedule);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _scheduleRepository.UpdateAsync(schedule);
        }
    }
}
