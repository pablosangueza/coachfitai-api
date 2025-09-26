namespace CoachFit.Api.Models
{
    public class PlanDto
    {
        public string Summary { get; set; } = "";
        public CaloriesRange CaloriesPerDay { get; set; } = new();
        public List<SessionDto> Sessions { get; set; } = new();
        public List<string> NutritionTips { get; set; } = new();
        public List<string> Notes { get; set; } = new();
    }

    public class CaloriesRange
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }

    public class SessionDto
    {
        public string Day { get; set; } = "";
        public string Focus { get; set; } = "";
        public int DurationMin { get; set; }
        public string? Details { get; set; }
    }

    public class PlanSavedResponse
    {
        public Guid PlanId { get; set; }
        public PlanDto Plan { get; set; } = new();
    }
}
