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

app.MapPost("/balance/{userId}/block", (Guid userId, [FromBody] BlockRequest request, UserBalance userBalance) =>
    {
        var blockId = userBalance.Block(request.Amount);
        return new BlockResponse(blockId,request.Amount);
    })
    .WithName("Block")
    .WithOpenApi();

app.MapPost("/balance/{userId}/unblock", (Guid userId, [FromBody] ReleaseBlockRequest request, UserBalance userBalance) =>
    {
        userBalance.ReleaseBlock(request.BlockId);
        return userBalance;
    })
    .WithName("ReleaseBalance")
    .WithOpenApi();

app.MapPost("/balance/{userId}/debit", (Guid userId, [FromBody] DebitBlockRequest request, UserBalance userBalance) =>
    {
        userBalance.Debit(request.BlockId);
        return userBalance;
    })
    .WithName("DebitBlock")
    .WithOpenApi();
app.MapPost("/balance/{userId}/credit", (Guid userId, [FromBody] CreditRequest request, UserBalance userBalance) =>
    {
        userBalance.Debit(request.Amount);
        return userBalance;
    })
    .WithName("Credit")
    .WithOpenApi();
app.Run();

public record BlockRequest(decimal Amount);

public record BlockResponse(Guid BlockId, decimal Amount);

public record DebitBlockRequest(Guid BlockId);
public record DebitRequest(decimal Amount);

public record ReleaseBlockRequest(Guid BlockId);

record CreditRequest(decimal Amount);

public class UserBalance
{
    public Guid UserId { get; set; }
    public decimal Balance { get; private set; } = 1000;
    public decimal BlockedBalance { get; private set; } = 0;
    public Dictionary<Guid, decimal> Blocks { get; private set; } = new();
    public void Debit(decimal amount) => Balance -= amount;
    public void Credit(decimal amount) => Balance += amount;

    public Guid Block(decimal amount)
    {
        var blockId = Guid.NewGuid();
        Balance -= amount;
        BlockedBalance += amount;
        Blocks.Add(blockId, amount);

        return blockId;
    }

    public void ReleaseBlock(Guid blockId)
    {
        if (Blocks.TryGetValue(blockId, out var value))
        {
            BlockedBalance -= value;
            Balance += value;
            Blocks.Remove(blockId);
        }
    }

    public void Debit(Guid blockId)
    {
        if (Blocks.TryGetValue(blockId, out var value))
        {
            BlockedBalance -= value;
            Blocks.Remove(blockId);
        }
    }
}