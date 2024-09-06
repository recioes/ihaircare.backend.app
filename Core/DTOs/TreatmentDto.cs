namespace Core.DTOs
{
    public class TreatmentDto
    {
        public string? Name { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int FrequencyInDays { get; set; }
        public string? Notes { get; set; }
    }
}
