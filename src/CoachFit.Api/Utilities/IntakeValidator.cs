// using CoachFit.Api.Models;

// namespace CoachFit.Api
// {
//     public static class IntakeValidator
//     {
//         public static List<string> Validate(IntakeRequest i)
//         {
//             var errors = new List<string>();
//             if (i.Age is < 12 or > 100) errors.Add("Age must be between 12 and 100.");
//             if (i.HeightCm is < 120 or > 230) errors.Add("HeightCm must be between 120 and 230.");
//             if (i.WeightKg is < 30 or > 250) errors.Add("WeightKg must be between 30 and 250.");
//             if (string.IsNullOrWhiteSpace(i.Sex)) errors.Add("Sex is required.");
//             if (string.IsNullOrWhiteSpace(i.ActivityLevel)) errors.Add("ActivityLevel is required.");
//             if (string.IsNullOrWhiteSpace(i.Goal)) errors.Add("Goal is required.");
//             return errors;
//         }
//     }
// }
