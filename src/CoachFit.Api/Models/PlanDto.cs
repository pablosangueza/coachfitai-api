namespace CoachFit.Api.Models;

// Intake
public record IntakeDto(
    Gender Gender, int Age, double WeightKg, double HeightCm,
    Goal Goal, Level Level, BodyType BodyType,
    IReadOnlyList<DietRestriction> Restrictions,
    DailyActivity DailyActivity, string? PhotoUrl);

public enum Gender { Male, Female, Other }
public enum Goal { LoseFat, Maintain, GainMuscle, Recomp }
public enum Level { Beginner, Intermediate, Advanced }
public enum BodyType { Ectomorph, Mesomorph, Endomorph, Mixed }
public enum DailyActivity { Sedentary, Light, Moderate, Active, VeryActive }
public enum DietRestriction { Vegan, Vegetarian, LactoseFree, GlutenFree, Keto, Halal, Kosher, NutAllergy }

// Plan
public record CaloriesRangeDto(int Min, int Max);
public record MacrosDto(int Protein_g, int Carbs_g, int Fat_g);

public record IngredientDto(string Item, int Grams);
public record MealDto(string Name, IEnumerable<IngredientDto> Ingredients, int Kcal);
public record DayNutritionDto(string Day, IEnumerable<MealDto> Meals);
public record ShoppingItemDto(string Item, string Quantity);
public record ExerciseDto(string Name, int Sets, string Reps, string Intensity);
public record SessionDto(string Day, string Focus, IEnumerable<ExerciseDto> Exercises, int DurationMin);

public record PlanDto(
    CaloriesRangeDto CaloriesPerDay,
    MacrosDto Macros,
    IEnumerable<DayNutritionDto> WeeklyPlan,
    IEnumerable<ShoppingItemDto> ShoppingList,
    int DaysPerWeek,
    IEnumerable<SessionDto> Sessions,
    string[] Assumptions,
    string[] Warnings,
    string Summary);

// Helpers
public static class IntakeGuard
{
    public static (bool ok, string? error) Basic(IntakeDto i)
    {
        if (i.Age is < 12 or > 100) return (false, "Age out of range (12–100).");
        if (i.WeightKg <= 0 || i.HeightCm <= 0) return (false, "Invalid weight/height.");
        return (true, null);
    }
}

// Checkout stubs
public record PriceResponse(decimal Amount, string Currency);
public class CheckoutVerifyRequest { public string? PaymentToken { get; set; } }
public class CheckoutVerifyResponse { public bool Success { get; set; } public string Message { get; set; } = ""; }
