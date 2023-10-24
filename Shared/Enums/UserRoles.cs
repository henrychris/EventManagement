namespace Shared.Enums;

// These classes must be in the same location
// as the strings must match when used for Role checks.
//
public enum UserRoles
{
    User,
    Admin,
}

public static class UserRoleStrings
{
    public const string User = "User";
    public const string Admin = "Admin";
    public const string AdminAndUserRoles = "Admin,User";

}