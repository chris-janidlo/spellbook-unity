using System;
using UnityEngine;

namespace crass
{
    public class Singleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static bool _allowReset = false;

        static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new Exception(
                        "Singleton not initialized; you must set it before trying to get it."
                    );
                }
                return _instance;
            }
            private set
            {
                if (!_allowReset && _instance != null)
                {
                    throw new Exception("Singleton already initialized; you cannot set it twice.");
                }
                _instance = value;
            }
        }

        protected void SingletonOverwriteInstance(T instance)
        {
            SingletonSetInstance(instance, true);
        }

        protected void SingletonSetPersistantInstance(T instance)
        {
            if (SingletonGetInstance() == instance)
            {
                return;
            }
            else if (SingletonGetInstance() == null)
            {
                SingletonSetInstance(instance, false);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected void SingletonSetInstance(T instance, bool allowReset)
        {
            Instance = instance;
            _allowReset = allowReset;
        }

        protected T SingletonGetInstance()
        {
            return _instance;
        }
    }
}
