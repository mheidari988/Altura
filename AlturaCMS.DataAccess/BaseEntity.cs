﻿namespace AlturaCMS.DataAccess;

/// <summary>
/// Represents the base entity class with common properties for all entities.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the date and time the entity was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the username of the user who created the entity.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time the entity was last updated.
    /// </summary>
    public DateTime? UpdatedDate { get; set; }

    /// <summary>
    /// Gets or sets the username of the user who last updated the entity.
    /// </summary>
    public string UpdatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the entity is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity is deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the date and time the entity was deleted.
    /// </summary>
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// Gets or sets the username of the user who deleted the entity.
    /// </summary>
    public string? DeletedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the version of the row for concurrency control.
    /// </summary>
    public byte[] RowVersion { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseEntity"/> class.
    /// Sets default values for properties.
    /// </summary>
    protected BaseEntity()
    {
        CreatedDate = DateTime.UtcNow;
        IsDeleted = false;
        RowVersion = [];
    }
}
