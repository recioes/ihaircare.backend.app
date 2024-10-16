using Core.DTOs;
using Core.Entities;
using Core.Extensions;
using Core.Interfaces.Services;
using FluentValidation;
using Infrastructure;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Core.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IValidator<Schedule> _scheduleValidator;
        private readonly ILogger<ScheduleService> _logger;

        public ScheduleService(IScheduleRepository scheduleRepository, IValidator<Schedule> scheduleValidator, ILogger<ScheduleService> logger)
        {
            _scheduleRepository = scheduleRepository;
            _scheduleValidator = scheduleValidator;
            _logger = logger;
        }

        public async Task CreateScheduleAsync(ScheduleDto scheduleDto)
        {
            var schedule = scheduleDto.MapToSchedule();

            var validationResult = await _scheduleValidator.ValidateAsync(schedule);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _scheduleRepository.AddAsync(schedule);
            _logger.LogInformation("Schedule {Title} succesfully created.", scheduleDto.Title);

            var frequencySelected = scheduleDto.Treatments.Any(f => f.FrequencyInDays.HasValue);

            if (frequencySelected)
            {
                _logger.LogInformation("Generating recurring treatments for schedule {Title}", scheduleDto.Title);
                await GenerateRecurringTreatments(schedule);
            }
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

            scheduleDto.MapToUpdatedSchedule(schedule);

            var updatedTreatments = scheduleDto.MapToUpdatedScheduledTreatments();

            SyncTreatments(schedule, updatedTreatments);

            var validationResult = await _scheduleValidator.ValidateAsync(schedule);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _scheduleRepository.UpdateAsync(schedule);

            var frequencyUpdated = updatedTreatments.Any(t => t.FrequencyInDays.HasValue && t.FrequencyInDays != null);

            if (frequencyUpdated)
            {
                _logger.LogInformation("Frequency updated. Generating recurring treatments for updated schedule {Title}", scheduleDto.Title);
                await GenerateRecurringTreatments(schedule);
            }
        }

        private void SyncTreatments(Schedule schedule, List<Treatment> updatedTreatments)
        {
            var treatmentsToRemove = schedule.Treatments
                .Where(t => !updatedTreatments.Any(ut => ut.ScheduledDate == t.ScheduledDate && ut.Name == t.Name))
                .ToList();

            foreach (var treatment in treatmentsToRemove)
            {
                schedule.Treatments.Remove(treatment);
            }

            var newTreatments = updatedTreatments
                .Where(ut => !schedule.Treatments.Any(t => t.ScheduledDate == ut.ScheduledDate && t.Name == ut.Name))
                .ToList();

            foreach (var newTreatment in newTreatments)
            {
                newTreatment.Id = ObjectId.GenerateNewId();  
            }

            schedule.Treatments.AddRange(newTreatments);

            foreach (var treatment in schedule.Treatments)
            {
                UpdateTreatments(updatedTreatments, treatment);
            }
        }

        private void UpdateTreatments(List<Treatment> updatedTreatments, Treatment treatment)
        {
            var updatedTreatment = updatedTreatments
                                .FirstOrDefault(ut => ut.ScheduledDate == treatment.ScheduledDate && ut.Name == treatment.Name);

            if (updatedTreatment != null)
            {
                treatment.Notes = UpdateIfDifferent(treatment.Notes, updatedTreatment.Notes);
                treatment.FrequencyInDays = UpdateIfDifferent(treatment.FrequencyInDays, updatedTreatment.FrequencyInDays);
                treatment.IsCompleted = UpdateIfDifferent(treatment.IsCompleted, updatedTreatment.IsCompleted);
            }
        }

        private T UpdateIfDifferent<T>(T originalValue, T newValue)
        {
            if (!EqualityComparer<T>.Default.Equals(originalValue, newValue))
            {
                return newValue;
            }
            return originalValue;
        }

        private async Task GenerateRecurringTreatments(Schedule schedule)
        {
            var treatmentsWithFrequency = schedule.Treatments.Where(t => t.FrequencyInDays.HasValue).ToList();

            foreach (var treatment in treatmentsWithFrequency)
            {
                var frequencyInDays = treatment.FrequencyInDays!.Value;
                var currentDate = schedule.StartDate;  
                var endDate = schedule.EndDate ?? schedule.StartDate.AddMonths(1);  

                if (frequencyInDays <= 0)
                {
                    continue; 
                }

                treatment.ScheduledDate = currentDate;

                while (currentDate < endDate)
                {
                    currentDate = currentDate.AddDays(frequencyInDays);

                    if (currentDate >= endDate)
                    {
                        break; 
                    }

                    if (!schedule.Treatments.Any(t => t.ScheduledDate == currentDate && t.Name == treatment.Name))
                    {
                        var recurringTreatment = new Treatment
                        {
                            Id = ObjectId.GenerateNewId(), 
                            Name = treatment.Name,
                            ScheduledDate = currentDate,  
                            FrequencyInDays = treatment.FrequencyInDays,
                            Notes = treatment.Notes,
                            IsCompleted = false
                        };

                        schedule.Treatments.Add(recurringTreatment);
                    }

                }
            }

            await _scheduleRepository.UpdateAsync(schedule);
        }

    }
}
