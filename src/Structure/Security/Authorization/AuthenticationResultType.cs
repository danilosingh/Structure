namespace Structure.Security.Authorization
{
    public enum AuthenticationResultType 
    {
        Success = 1,
        InvalidUserNameOrEmailAddress,
        InvalidPassword,
        UserIsNotActive,
        InvalidTenancyName,
        TenantIsNotActive,
        UserEmailIsNotConfirmed,
        UnknownExternalLogin,
        LockedOut,
        UserPhoneNumberIsNotConfirmed,
        UnauthenticatedUser,
        InvalidRefreshToken,
    }
}
