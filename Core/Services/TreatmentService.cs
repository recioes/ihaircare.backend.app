using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using FluentValidation;
using Infrastructure;
using MongoDB.Bson;

namespace Core.Services
{
    public class TreatmentService : ITreatmentService
    {
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IValidator<Treatment> _treatmentValidator;

        public TreatmentService(ITreatmentRepository treatmentRepository, IValidator<Treatment> treatmentValidator, IScheduleRepository scheduleRepository)
        {
            _treatmentRepository = treatmentRepository;
            _treatmentValidator = treatmentValidator;
            _scheduleRepository = scheduleRepository;
        }

        public async Task AddTreatmentAsync(TreatmentDto treatmentDto)
        {
            var treatment = new Treatment
            {
                Name = treatmentDto.Name,
                ScheduledDate = treatmentDto.ScheduledDate,
                FrequencyInDays = treatmentDto.FrequencyInDays,
                Notes = treatmentDto.Notes,
                IsCompleted = treatmentDto.IsCompleted
            };

            var validationResult = await _treatmentValidator.ValidateAsync(treatment);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _treatmentRepository.AddAsync(treatment);

            // Gerar instâncias futuras se o tratamento for recorrente
            if (treatment.FrequencyInDays.HasValue)
            {
                await GenerateRecurringTreatments(treatment);
            }
        }

        public async Task UpdateTreatmentAsync(ObjectId treatmentId, TreatmentDto treatmentDto)
        {
            var treatment = await _treatmentRepository.GetByIdAsync(treatmentId);
            if (treatment == null)
            {
                throw new InvalidOperationException("Treatment not found.");
            }

            treatment.Name = treatmentDto.Name;
            treatment.ScheduledDate = treatmentDto.ScheduledDate;
            treatment.FrequencyInDays = treatmentDto.FrequencyInDays;
            treatment.Notes = treatmentDto.Notes;
            treatment.IsCompleted = treatmentDto.IsCompleted;

            var validationResult = await _treatmentValidator.ValidateAsync(treatment);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _treatmentRepository.UpdateAsync(treatment);

            // Atualiza instâncias futuras se o tratamento for recorrente
            if (treatment.FrequencyInDays.HasValue)
            {
                await GenerateRecurringTreatments(treatment);
            }
        }

        private async Task GenerateRecurringTreatments(Treatment treatment, Schedule schedule)
        {
            if (treatment.FrequencyInDays.HasValue)
            {
                int frequencyInDays = treatment.FrequencyInDays.Value;

                var endDate = schedule.EndDate.HasValue ? schedule.EndDate.Value : treatment.ScheduledDate.AddMonths(1); // Usa o EndDate do Schedule ou 1 mês padrão
                var currentDate = treatment.ScheduledDate;

                while (currentDate < endDate)
                {
                    currentDate = currentDate.AddDays(frequencyInDays);

                    // Verifica se já existe um tratamento para o usuário na data
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

                        // Adiciona o tratamento recorrente no cronograma
                        schedule.Treatments.Add(recurringTreatment);
                        await _treatmentRepository.AddAsync(recurringTreatment);
                    }
                }
            }
        }



        public async Task DeleteTreatmentAsync(ObjectId id)
        {
            var treatment = await _treatmentRepository.GetByIdAsync(id);
            if (treatment == null)
            {
                throw new InvalidOperationException("Treatment not found.");
            }

            await _treatmentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Treatment>> GetAllTreatmentsAsync()
        {
            return await _treatmentRepository.GetAllAsync();
        }

        public async Task<Treatment> GetTreatmentByIdAsync(ObjectId id)
        {
            var treatment = await _treatmentRepository.GetByIdAsync(id);
            if (treatment == null)
            {
                throw new InvalidOperationException("Treatment not found.");
            }

            return treatment;
        }
    }
}
