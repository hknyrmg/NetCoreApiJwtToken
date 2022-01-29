using System.Collections.Generic;
using System.Linq;
using TokenBasedAuth_NetCore.Entities;

namespace TokenBasedAuth_NetCore.Utilities.Helper
{
    public static class UserExtensionMethods
    {
        public static IEnumerable<User> WithoutPasswords(this IEnumerable<User> users)
        {
            if (users == null) return null;

            return users.Select(x => x.WithoutPassword());
        }

        public static User WithoutPassword(this User user)
        {
            if (user == null) return null;

            user.Password = null;
            return user;
        }
    }
}
