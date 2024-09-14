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
                    FrequencyInDays = t.FrequencyInDays,
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

            // Adiciona o Schedule e os Treatments de uma vez
            await _scheduleRepository.AddAsync(schedule);

            // Se houver tratamentos recorrentes, gerar futuras instâncias
            await GenerateRecurringTreatments(schedule);
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
            // Busca o cronograma existente
            var schedule = await _scheduleRepository.GetByIdAsync(scheduleId);
            if (schedule == null)
            {
                throw new InvalidOperationException("Schedule not found.");
            }

            // Atualiza os campos do cronograma
            schedule.Title = scheduleDto.Title;
            schedule.StartDate = scheduleDto.StartDate;
            schedule.EndDate = scheduleDto.EndDate;

            // Atualiza os tratamentos existentes, adiciona novos e remove os que não estão mais no DTO
            var updatedTreatments = scheduleDto.Treatments.Select(t => new Treatment
            {
                Name = t.Name,
                ScheduledDate = t.ScheduledDate,
                FrequencyInDays = t.FrequencyInDays,
                Notes = t.Notes,
                IsCompleted = false
            }).ToList();

            // Sincroniza os tratamentos no cronograma
            SyncTreatments(schedule, updatedTreatments);

            // Valida o cronograma após as atualizações
            var validationResult = await _scheduleValidator.ValidateAsync(schedule);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Atualiza o cronograma no banco de dados
            await _scheduleRepository.UpdateAsync(schedule);

            // Gera tratamentos recorrentes, se necessário
            await GenerateRecurringTreatments(schedule);
        }

        private void SyncTreatments(Schedule schedule, List<Treatment> updatedTreatments)
        {
            // Remover tratamentos antigos que não estão mais no DTO
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

            schedule.Treatments.AddRange(newTreatments);

            foreach (var treatment in schedule.Treatments)
            {
                var updatedTreatment = updatedTreatments.FirstOrDefault(ut => ut.ScheduledDate == treatment.ScheduledDate && ut.Name == treatment.Name);
                if (updatedTreatment != null)
                {
                    treatment.Notes = updatedTreatment.Notes;
                    treatment.FrequencyInDays = updatedTreatment.FrequencyInDays;
                    treatment.IsCompleted = updatedTreatment.IsCompleted;
                }
            }
        }

        private async Task GenerateRecurringTreatments(Schedule schedule)
        {
            foreach (var treatment in schedule.Treatments.Where(t => t.FrequencyInDays.HasValue))
            {
                var frequencyInDays = treatment.FrequencyInDays!.Value;
                var currentDate = treatment.ScheduledDate;
                var endDate = schedule.EndDate ?? schedule.StartDate.AddMonths(12); // Definir o período para recorrência

                while (currentDate < endDate)
                {
                    currentDate = currentDate.AddDays(frequencyInDays);

                    // Verificar se já existe um cronograma ou tratamento no banco de dados na data
                    bool exists = await _scheduleRepository.HasUserScheduleOnDateAsync(schedule.UserId, currentDate);
                    if (!exists)
                    {
                        var recurringTreatment = new Treatment
                        {
                            Name = treatment.Name,
                            ScheduledDate = currentDate,
                            FrequencyInDays = treatment.FrequencyInDays,
                            Notes = treatment.Notes,
                            IsCompleted = false
                        };

                        schedule.Treatments.Add(recurringTreatment); // Adicionar tratamento ao cronograma
                    }
                }
            }

            // Atualiza o cronograma com os tratamentos recorrentes
            await _scheduleRepository.UpdateAsync(schedule);
        }



    }
}
