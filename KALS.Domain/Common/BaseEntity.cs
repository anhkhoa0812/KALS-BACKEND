using System.ComponentModel.DataAnnotations;

namespace KALS.Domain.Common;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
}