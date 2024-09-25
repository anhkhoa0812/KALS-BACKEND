using System.ComponentModel.DataAnnotations;
using KALS.Domain.Common;
using KALS.Domain.Enums;

namespace KALS.Domain.Entity;

public class Role: BaseEntity
{
    public RoleEnum Name { get; set; }
}