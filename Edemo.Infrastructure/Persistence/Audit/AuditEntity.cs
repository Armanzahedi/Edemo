﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Edemo.Infrastructure.Persistence.Audit;

public class AuditEntity
{
    private AuditEntity()
    {
    }

    private readonly Dictionary<string, object?> _tempOldValues = new();
    private readonly Dictionary<string, object?> _tempNewValues = new();

    public AuditEntity(EntityEntry entry, string? userId = null)
    {
        foreach (PropertyEntry property in entry.Properties)
        {
            if (property.IsAuditable())
            {
                if (property.IsTemporary)
                    continue;

                string propertyName = property.Metadata.Name;

                switch (entry.State)
                {
                    case EntityState.Added:
                        _tempNewValues[propertyName] = property.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        _tempOldValues[propertyName] = property.OriginalValue;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            _tempOldValues[propertyName] = property.OriginalValue;
                            _tempNewValues[propertyName] = property.CurrentValue;
                        }

                        break;
                }
            }
        }
        
        this.OldValues = _tempOldValues.Count == 0 ? "" : JsonConvert.SerializeObject(_tempOldValues);
        this.NewValues = _tempNewValues.Count == 0 ? "" : JsonConvert.SerializeObject(_tempNewValues);
        this.DateTimeOffset = DateTimeOffset.Now;
        this.EntityState = entry.State;
        TableName = entry.Metadata.GetTableName();
        DisplayName = entry.Metadata.DisplayName();
        ByUser = userId;
    }

    public long Id { get; set; }
    public string OldValues { get; set; }
    public string NewValues { get; set; }
    public DateTimeOffset DateTimeOffset { get; set; }
    public EntityState EntityState { get; set; }
    public string? TableName { get; set; }
    public string DisplayName { get; set; }
    public string? ByUser { get; set; }
}