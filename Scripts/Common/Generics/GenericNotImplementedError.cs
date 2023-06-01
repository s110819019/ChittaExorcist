using UnityEngine;

namespace ChittaExorcist.Common.Generics
{
    public static class GenericNotImplementedError<T>
    {
        public static T TryGet(T value, string name)
        {
            if (value != null)
            {
                return value;
            }
            // Debug.LogError(typeof(T) + " not implemented on " + name);
            Debug.LogError(typeof(T) + " 沒有被實作在 " + name);
            return default;
        }
    }
}