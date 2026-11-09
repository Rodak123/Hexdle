using System;
using UnityEngine;

namespace Rodak.Utils.Singleton
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        private static Lazy<T> instance = CreateLazyInstance();

        static Singleton()
        {
            SingletonReloader.OnReset += Reset;
        }

        private static void Reset()
        {
            instance = CreateLazyInstance();
        }

        private static Lazy<T> CreateLazyInstance()
        {
            return new(() =>
            {
                return Activator.CreateInstance(typeof(T), true) as T;
            });
        }

        public static T Instance => instance.Value;
    }
}