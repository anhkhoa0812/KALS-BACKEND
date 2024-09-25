using System.ComponentModel.DataAnnotations;
using KALS.Domain.Common;

namespace KALS.Domain.Entity;

public class Role: BaseEntity
{
    [MaxLength(250)]
    public string Name { get; set; }
}