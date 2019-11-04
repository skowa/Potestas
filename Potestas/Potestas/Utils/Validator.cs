using System;

namespace Potestas.Utils
{
    public static class Validator
    {
        public static bool IsGenericTypeNull<T>(T item)
        {
            return (!typeof(T).IsValueType || Nullable.GetUnderlyingType(typeof(T)) != null) && item == null;
        }
    }
}