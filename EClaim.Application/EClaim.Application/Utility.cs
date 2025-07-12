using EClaim.Application.Enum;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EClaim.Application
{
    public class Utility
    {
        public static List<string> StatusOptions = new() { "Submitted", "Reviewed", "Approved", "Rejected" };
        public static List<string> Roles = new() { "Claimant", "Adjuster", "Approver", "Admin" };
        public static List<string> ClaimTypeOptions = new() { "Health", "Vehicle", "Property" };
        public static List<string> UserVerified = new() { "Yes", "No" };

        public static string GetDisplayName<TEnum>(TEnum value) where TEnum : System.Enum
        {
            var member = typeof(TEnum).GetMember(value.ToString()).FirstOrDefault();
            if (member != null)
            {
                var displayAttr = member.GetCustomAttribute<DisplayAttribute>();
                return displayAttr?.Name ?? value.ToString();
            }
            return value.ToString();
        }
    }
}
