using CoachFit.Api.Models;

namespace CoachFit.Api.Services;

public interface IPlanService
{
    PlanDto GeneratePlan(IntakeDto intake);
}
