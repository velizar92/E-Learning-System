namespace E_LearningSystem.Infrastructure.Extensions
{
    using System.Security.Claims;

    public static class ClaimsPrincipalExtensions
    {
        public static string Id(this ClaimsPrincipal user)
        {
            if(user == null)
            {
                return string.Empty;
            }

           return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
          
        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole("Admin");
        }
    }         
}
