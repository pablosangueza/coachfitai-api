namespace CoachFit.Api.Models
{
    public record IntakeRequest(
        int Age,
        string Sex,
        double HeightCm,
        double WeightKg,
        string ActivityLevel,   // sedentary, light, moderate, high
        string Goal,            // lose, maintain, gain
        string? Injuries = null,
        string? Preferences = null
    );
}
