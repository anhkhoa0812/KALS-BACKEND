using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KALS.Domain.Common;
using KALS.Domain.Enums;

namespace KALS.Domain.Entities;

public class User: BaseEntity
{
    [MaxLength(250)]
    public string UserName { get; set; }
    [MaxLength(250)]
    public string Password { get; set; }
    [MaxLength(13)]
    public string PhoneNumber { get; set; }
    [MaxLength(250)]
    public string FullName { get; set; }
    public RoleEnum Role { get; set; }
}