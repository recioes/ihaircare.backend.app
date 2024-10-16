using Core.Entities;
using FluentValidation;
using Infrastructure;

namespace Core.Validators
{
    public class ScheduleValidator : AbstractValidator<Schedule>
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleValidator(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;

            RuleFor(schedule => schedule)
                .Must(schedule => schedule.StartDate <= schedule.EndDate)
                .WithMessage("End date must be greater than or equal to the start date.");
           

            RuleFor(schedule => schedule.Treatments)
                .NotEmpty().WithMessage("At least one treatment is required");
        }

    }
}
