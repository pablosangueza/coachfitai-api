using CoachFit.Api.Models;

namespace CoachFit.Api.Storage
{
    public interface IPlanStore
    {
        Guid Save(PlanDto plan);
        bool TryGet(Guid id, out PlanDto? plan);
    }
}
