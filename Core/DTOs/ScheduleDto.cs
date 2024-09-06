namespace Core.DTOs
{
    public class ScheduleDto
    {
        public string? Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<TreatmentDto> Treatments { get; set; }
    }
}
