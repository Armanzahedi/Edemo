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

app.MapGet("/balance/{userId}", (Guid userId, UserBalance userBalance) =>
    {
        userBalance.UserId = userId;
        return userBalance;
    })
    .WithName("GetUserBalance")
    .WithOpenApi();

app.MapPost("/balance/{userId}/debit", (Guid userId, [FromBody] DebitRequest request, UserBalance userBalance) =>
    {
        var  debitId = userBalance.Debit(request.Amount);
        return new DebitResponse(debitId,request.Amount);
    })
    .WithName("Debit")
    .WithOpenApi();

app.MapPost("/balance/{userId}/debit/release", (Guid userId, [FromBody] ReleaseDebitRequest request, UserBalance userBalance) =>
    {
        userBalance.ReleaseDebit(request.DebitId);
        return userBalance;
    })
    .WithName("ReleaseDebit")
    .WithOpenApi();

app.MapPost("/balance/{userId}/debit/verify", (Guid userId, [FromBody] VerifyDebitRequest request, UserBalance userBalance) =>
    {
        userBalance.VerifyDebit(request.DebitId);
        return userBalance;
    })
    .WithName("VerifyDebit")
    .WithOpenApi();
app.MapPost("/balance/{userId}/credit", (Guid userId, [FromBody] CreditRequest request, UserBalance userBalance) =>
    {
        userBalance.Debit(request.Amount);
        return userBalance;
    })
    .WithName("Credit")
    .WithOpenApi();
app.Run();

public record DebitRequest(decimal Amount);

public record DebitResponse(Guid DebitId, decimal Amount);

public record VerifyDebitRequest(Guid DebitId);

public record ReleaseDebitRequest(Guid DebitId);

record CreditRequest(decimal Amount);

public class UserBalance
{
    public Guid UserId { get; set; }
    public decimal Balance { get; private set; } = 1000;
    public decimal BlockedBalance { get; private set; } = 0;
    public Dictionary<Guid, decimal> Blocks { get; private set; } = new();
    public void Credit(decimal amount) => Balance += amount;

    public Guid Debit(decimal amount)
    {
        var debitId = Guid.NewGuid();
        Balance -= amount;
        BlockedBalance += amount;
        Blocks.Add(debitId, amount);

        return debitId;
    }

    public void ReleaseDebit(Guid debitId)
    {
        if (Blocks.TryGetValue(debitId, out var value))
        {
            BlockedBalance -= value;
            Balance += value;
            Blocks.Remove(debitId);
        }
    }

    public void VerifyDebit(Guid debitId)
    {
        if (Blocks.TryGetValue(debitId, out var value))
        {
            BlockedBalance -= value;
            Blocks.Remove(debitId);
        }
    }
}