#nullable disable
using System.Reflection;

namespace StaffNook.Infrastructure.Logging
{
    public static class StringHashHelper
    {
        private static readonly MethodInfo GetNonRandomizedHashCode;

        static StringHashHelper()
        {
            GetNonRandomizedHashCode = typeof(string).GetMethod("GetNonRandomizedHashCode", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static int GetStringHash(string str)
        {
            // ReSharper disable once PossibleNullReferenceException
            return (int)GetNonRandomizedHashCode.Invoke(str, Array.Empty<object>());
        }
    }
}