using System;
using UnityEngine;

namespace Rodak.Utils.Singleton
{
    internal static class SingletonReloader
    {
        public static event Action OnReset;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            OnReset?.Invoke();
        }
    }
}