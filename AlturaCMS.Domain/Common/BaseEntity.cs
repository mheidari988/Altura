using System.ComponentModel.DataAnnotations;

namespace AlturaCMS.Domain.Common;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }

    [Required]
    [ConcurrencyCheck]
    public byte[] RowVersion { get; set; }

    [Required]
    [MaxLength(450)]
    public string CreatedBy { get; set; } = string.Empty;

    [MaxLength(450)]
    public string UpdatedBy { get; set; } = string.Empty;

    [MaxLength(450)]
    public string DeletedBy { get; set; } = string.Empty;

    // Constructor to set default values
    protected BaseEntity()
    {
        CreatedDate = DateTime.UtcNow;
        IsDeleted = false;
        RowVersion = [];
    }
}
