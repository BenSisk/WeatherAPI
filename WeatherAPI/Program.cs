using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddRateLimiter(LimiterOptions =>
    {
        LimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        LimiterOptions.AddTokenBucketLimiter(policyName: "WeatherAPI", PolicyOptions =>
        {
            PolicyOptions.TokenLimit = 20;
            PolicyOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(5);
            PolicyOptions.TokensPerPeriod = 5;
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
