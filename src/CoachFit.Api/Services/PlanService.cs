using CoachFit.Api.Models;

namespace CoachFit.Api.Services;

public class PlanService : IPlanService
{
    public PlanDto GeneratePlan(IntakeDto intake)
    {
        // BMR (Mifflin-St Jeor)
        var bmr = intake.Gender switch
        {
            Gender.Male   => 10 * intake.WeightKg + 6.25 * intake.HeightCm - 5 * intake.Age + 5,
            Gender.Female => 10 * intake.WeightKg + 6.25 * intake.HeightCm - 5 * intake.Age - 161,
            _             => 10 * intake.WeightKg + 6.25 * intake.HeightCm - 5 * intake.Age
        };

        var activityFactor = intake.DailyActivity switch
        {
            DailyActivity.Sedentary  => 1.2,
            DailyActivity.Light      => 1.375,
            DailyActivity.Moderate   => 1.55,
            DailyActivity.Active     => 1.725,
            DailyActivity.VeryActive => 1.9,
            _ => 1.55
        };

        var tdee = bmr * activityFactor;

        var delta = intake.Goal switch
        {
            Goal.LoseFat    => -400,
            Goal.GainMuscle =>  300,
            Goal.Recomp     => -100,
            _               =>  0
        };

        var target = (int)Math.Round(tdee + delta);
        var calRange = new CaloriesRangeDto(target - 100, target + 100);

        // Simple macros
        var protein = (int)Math.Round(intake.WeightKg * 2.0);
        var fat     = (int)Math.Round(intake.WeightKg * 0.8);
        var proteinKcal = protein * 4;
        var fatKcal     = fat * 9;
        var carbsKcal   = Math.Max(0, target - (proteinKcal + fatKcal));
        var carbs       = (int)Math.Round(carbsKcal / 4.0);

        var macros = new MacrosDto(protein, carbs, fat);

        // Example stub nutrition
        var weekly = new List<DayNutritionDto>
        {
            new("Mon", new[]
            {
                new MealDto("Breakfast", new[]{ new IngredientDto("Oats", 60), new("Egg whites",150) }, 450),
                new MealDto("Lunch",     new[]{ new IngredientDto("Chicken breast",180), new("Rice",150), new("Broccoli",120) }, 620),
                new MealDto("Dinner",    new[]{ new IngredientDto("Salmon",160), new("Quinoa",140), new("Salad",120) }, 680),
            })
        };

        var shopping = new List<ShoppingItemDto>
        {
            new("Chicken breast","1.5 kg"),
            new("Rice","2 kg"),
            new("Oats","1 kg"),
            new("Eggs / egg whites","2 dozen"),
            new("Veggies assorted","2 kg")
        };

        // Example sessions
        var sessions = new List<SessionDto>
        {
            new("Mon","Full Body", new[]{
                new ExerciseDto("Squat",4,"8–10","Moderate"),
                new ExerciseDto("Bench Press",4,"8–10","Moderate"),
                new ExerciseDto("Plank",3,"60s","Easy")
            },45),
            new("Tue","Cardio Z2", new[]{
                new ExerciseDto("Treadmill / Cycle",1,"30–40m","Easy")
            },35),
        };

        var assumptions = new[]
        {
            "Macros use ~2 g/kg protein, ~0.8 g/kg fat, carbs fill remainder.",
            "Activity factor derived from reported daily activity."
        };

        var warnings = new List<string>();
        if (intake.Restrictions?.Count > 0)
            warnings.Add("Nutrition items are examples; honor diet restrictions when generating final meals.");

        var summary = $"{intake.Goal} plan for {intake.Age}y {intake.Gender} at {intake.WeightKg}kg/{intake.HeightCm}cm; activity: {intake.DailyActivity}.";

        return new PlanDto(calRange, macros, weekly, shopping, 7, sessions, assumptions, warnings.ToArray(), summary);
    }
}
