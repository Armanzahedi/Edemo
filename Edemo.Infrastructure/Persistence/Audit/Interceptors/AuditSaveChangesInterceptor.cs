﻿using Edemo.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Edemo.Domain.Common;
using Edemo.Domain.Common.Entity;

namespace Edemo.Infrastructure.Persistence.Audit.Interceptors;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUser _currentUser;
    private readonly IDateTimeProvider _dateTime;

    public AuditSaveChangesInterceptor(
        ICurrentUser currentUser,
        IDateTimeProvider dateTime)
    {
        _currentUser = currentUser;
        _dateTime = dateTime;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplyAuditPolicy(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        ApplyAuditPolicy(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void ApplyAuditPolicy(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries().Where(entry => entry.ShouldBeAudited()).ToList())
        {
            context.Attach(new AuditEntity(entry, _currentUser.UserId?.ToString() ?? ""));
        }

        foreach (var entry in context.ChangeTracker.Entries<AuditableEntityBase>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = _currentUser.UserId;
                entry.Entity.CreatedOn = _dateTime.UtcNow;
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified ||
                entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedBy = _currentUser.UserId;
                entry.Entity.LastModifiedOn = _dateTime.UtcNow;
            }
        }
    }
}