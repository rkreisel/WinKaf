using System;

namespace WinKaf
{
    public static class UtilitiesAndExtensions
    {

        public static void CheckForNull(this object o, string paramName)
        {
            if (o == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
        public static void CheckForNull<T>(this object o, string paramName, string message = "") where T : Exception
        {
            if (o == null)
            {
                var formattedMessage = string.IsNullOrWhiteSpace(message) ? paramName : $"{paramName} - {message}";
                throw (T)Activator.CreateInstance(typeof(T), new object[] { $"{formattedMessage}" });
            }
        }

        public static bool HasProperty(this object obj, string propertyName)
        {
            obj.CheckForNull(nameof(obj));
            return obj.GetType().GetProperty(propertyName) != null;
        }
    }
}
