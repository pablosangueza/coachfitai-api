using CoachFit.Api.Models;

namespace CoachFit.Api.Services
{
    public class PlanService : IPlanService
    {
        public PlanDto GeneratePlan(IntakeRequest intake)
        {
            var bmr = intake.Sex.ToLower() switch
            {
                "male" or "m" => 10 * intake.WeightKg + 6.25 * intake.HeightCm - 5 * intake.Age + 5,
                _              => 10 * intake.WeightKg + 6.25 * intake.HeightCm - 5 * intake.Age - 161
            };

            var activityFactor = intake.ActivityLevel.ToLower() switch
            {
                "sedentary" => 1.2,
                "light"     => 1.375,
                "moderate"  => 1.55,
                "high"      => 1.725,
                _           => 1.375
            };

            var tdee = bmr * activityFactor;

            var delta = intake.Goal.ToLower() switch
            {
                "lose" => -400,
                "gain" =>  300,
                _      =>  0
            };

            var target = (int)Math.Round(tdee + delta);
            var range  = new CaloriesRange { Min = target - 100, Max = target + 100 };

            var sessions = new List<SessionDto>
            {
                new(){ Day="Mon", Focus="Full body strength", DurationMin=45, Details="Compound lifts + core" },
                new(){ Day="Tue", Focus="Low-intensity cardio", DurationMin=30, Details="Zone 2 walk/cycle" },
                new(){ Day="Wed", Focus="Upper body", DurationMin=40, Details="Push/Pull supersets" },
                new(){ Day="Thu", Focus="Mobility + core", DurationMin=25, Details="Hips, T-spine, plank" },
                new(){ Day="Fri", Focus="Lower body", DurationMin=45, Details="Squat + posterior chain" },
                new(){ Day="Sat", Focus="Intervals", DurationMin=20, Details="10 x 1:00 hard / 1:00 easy" },
                new(){ Day="Sun", Focus="Active recovery", DurationMin=30, Details="Walk + light stretch" },
            };

            var tips = new List<string>
            {
                "Prioritize 1.6–2.2 g protein per kg body weight.",
                "80% whole foods; 20% flexible.",
                "2–3L water/day; add electrolytes if sweating.",
                "Sleep 7–9h; keep a consistent schedule."
            };

            var notes = new List<string>();
            if (!string.IsNullOrWhiteSpace(intake.Injuries))
                notes.Add($"Modify sessions due to injuries: {intake.Injuries}.");
            if (!string.IsNullOrWhiteSpace(intake.Preferences))
                notes.Add($"Preferences: {intake.Preferences}.");

            return new PlanDto
            {
                Summary = $"{Cap(intake.Goal)} plan for {intake.Age}y {intake.Sex} at {intake.WeightKg}kg/{intake.HeightCm}cm (activity: {intake.ActivityLevel}).",
                CaloriesPerDay = range,
                Sessions = sessions,
                NutritionTips = tips,
                Notes = notes
            };
        }

        private static string Cap(string s) =>
            string.IsNullOrWhiteSpace(s) ? "" : char.ToUpper(s[0]) + s[1..].ToLower();
    }
}
