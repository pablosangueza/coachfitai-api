using System.Collections.Concurrent;
using CoachFit.Api.Models;

namespace CoachFit.Api.Storage
{
    public class InMemoryPlanStore : IPlanStore
    {
        private readonly ConcurrentDictionary<Guid, PlanDto> _plans = new();

        public Guid Save(PlanDto plan)
        {
            var id = Guid.NewGuid();
            _plans[id] = plan;
            return id;
        }

        public bool TryGet(Guid id, out PlanDto? plan)
        {
            var ok = _plans.TryGetValue(id, out var p);
            plan = p;
            return ok;
        }
    }
}
