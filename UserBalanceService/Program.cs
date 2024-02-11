using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<UserBalance>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/balance/{userId}", (Guid userId,UserBalance userBalance) =>
    {
        userBalance.UserId = userId;
        return userBalance;
    })
    .WithName("GetUserBalance")
    .WithOpenApi();

app.MapPost("/balance/{userId}/debit", (Guid userId, [FromBody]DebitRequest request, UserBalance userBalance) =>
    {
        userBalance.Debit(request.Amount);
        return userBalance;
    })
    .WithName("Debit")
    .WithOpenApi();
app.MapPost("/balance/{userId}/credit", (Guid userId, [FromBody]CreditRequest request, UserBalance userBalance) =>
    {
        userBalance.Debit(request.Amount);
        return userBalance;
    })
    .WithName("Credit")
    .WithOpenApi();
app.Run();

record DebitRequest(decimal Amount);
record CreditRequest(decimal Amount);

public class UserBalance
{
    public Guid UserId { get; set; }
    public decimal Balance { get; private set; } = 1000;
    public void Debit(decimal amount) => Balance -= amount;
    public void Credit(decimal amount) => Balance += amount;
}