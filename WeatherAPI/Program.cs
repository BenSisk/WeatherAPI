using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Token Bucket Rate Limiter
builder.Services.AddRateLimiter(LimiterOptions =>
    {
        // If out of tokens, return 429 status code (Too Many Requests)
        LimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        // WeatherAPI Token Bucket Rate Limiter
        LimiterOptions.AddTokenBucketLimiter(policyName: "WeatherAPI", PolicyOptions =>
        {
            // 20 tokens max
            PolicyOptions.TokenLimit = 20;

            // 5 more tokens per 5 seconds
            PolicyOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(5);
            PolicyOptions.TokensPerPeriod = 5;

            // dont need to manually update
            PolicyOptions.AutoReplenishment = true;
        });
    }
);
    


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();
