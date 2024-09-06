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

            RuleFor(schedule => schedule.Id)
                .NotEmpty().WithMessage("Schedule cannot be null");


            RuleFor(schedule => schedule)
                .Must(schedule => schedule.StartDate <= schedule.EndDate)
                .WithMessage("End date must be greater than or equal to the start date.")
                .MustAsync(async (schedule, cancellation) =>
                    !(await _scheduleRepository.HasUserScheduleOnDateAsync(schedule.UserId, schedule.StartDate)))
                .WithMessage(schedule => $"There is already a schedule for the user on this start date: {schedule.StartDate.ToString("dd/MM/yyyy")}");

            RuleFor(schedule => schedule.Treatments)
                .NotEmpty().WithMessage("At least one treatment is required");
        }

    }
}
