using CoachFit.Api;    
using CoachFit.Api.Models;
using CoachFit.Api.Services;
using CoachFit.Api.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
const string FrontendCors = "FrontendCors";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(FrontendCors, p => p
        .WithOrigins(
            "https://pablosangueza.github.io",
            "https://psmart-code.github.io",
            "http://localhost:5000", "https://localhost:5001",
            "http://localhost:5173", "http://localhost:5174"
        )
        .AllowAnyHeader()
        .AllowAnyMethod());
});

// DI
builder.Services.AddSingleton<IPlanService, PlanService>();
builder.Services.AddSingleton<IPlanStore, InMemoryPlanStore>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(FrontendCors);

// Health
app.MapGet("/", () => Results.Ok(new { name = "CoachFitAI API", status = "ok" }));

// Generate plan
app.MapPost("/api/plan/generate", (IntakeRequest intake, IPlanService service, IPlanStore store) =>
{
    var errs = IntakeValidator.Validate(intake);
    if (errs.Count > 0) return Results.BadRequest(new { errors = errs });

    var plan = service.GeneratePlan(intake);
    var id = store.Save(plan);
    return Results.Ok(new PlanSavedResponse { PlanId = id, Plan = plan });
});

// Get plan by id
app.MapGet("/api/plan/{id:guid}", (Guid id, IPlanStore store) =>
    store.TryGet(id, out var plan) ? Results.Ok(plan) : Results.NotFound(new { message = "Plan not found" })
);

// Sample plan
app.MapGet("/api/plan/sample", (IPlanService svc) =>
    Results.Ok(svc.GeneratePlan(new IntakeRequest(30, "male", 175, 78, "moderate", "lose")))
);

// Checkout stubs
app.MapGet("/api/checkout/price", () => Results.Ok(new PriceResponse(7, "USD")));
app.MapPost("/api/checkout/verify", (CheckoutVerifyRequest req) =>
{
    var ok = !string.IsNullOrWhiteSpace(req.PaymentToken);
    return Results.Ok(new CheckoutVerifyResponse { Success = ok, Message = ok ? "Payment verified" : "Invalid token" });
});

app.Run();
