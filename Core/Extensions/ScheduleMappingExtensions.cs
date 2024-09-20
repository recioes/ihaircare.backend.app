using Core.DTOs;
using Core.Entities;
using MongoDB.Bson;
using System.Reflection.Metadata.Ecma335;

namespace Core.Extensions
{
    public static class ScheduleMappingExtensions
    {
        public static Schedule MapToSchedule(this ScheduleDto scheduleDto)
        {
            return new Schedule
            {
                Title = scheduleDto.Title,
                StartDate = scheduleDto.StartDate,
                EndDate = scheduleDto.EndDate,
                Treatments = scheduleDto.Treatments.Select(t => new Treatment
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = t.Name,
                    ScheduledDate = scheduleDto.StartDate,
                    FrequencyInDays = t.FrequencyInDays,
                    Notes = t.Notes,
                    IsCompleted = false
                }).ToList(),
                IsCompleted = false
            };
        }

        public static void MapToUpdatedSchedule(this ScheduleDto scheduleDto, Schedule schedule)
        {
            schedule.Title = scheduleDto.Title;
            schedule.StartDate = scheduleDto.StartDate;
            schedule.EndDate = scheduleDto.EndDate;
        }

        public static List<Treatment> MapToUpdatedScheduledTreatments(this ScheduleDto scheduleDto)
        {
            return scheduleDto.Treatments.Select(t => new Treatment
            {
                Name = t.Name,
                ScheduledDate = scheduleDto.StartDate,
                FrequencyInDays = t.FrequencyInDays,
                Notes = t.Notes,
                IsCompleted = false
            }).ToList();
        }
    }

}
