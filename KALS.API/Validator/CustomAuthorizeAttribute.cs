using KALS.API.Utils;
using KALS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace KALS.API.Validator;

public class CustomAuthorizeAttribute: AuthorizeAttribute
{
    public CustomAuthorizeAttribute(params RoleEnum[] roleEnums)
    {
        var allowedRoleAsString = roleEnums.Select(x => x.GetDescriptionFromEnum());
        Roles = string.Join(",", allowedRoleAsString);
    }
}