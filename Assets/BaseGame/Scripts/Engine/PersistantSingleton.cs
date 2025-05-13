using UnityEngine;

namespace LCPS.SlipForge.Engine
{
    using UnityEngine;

    public abstract class PersistantSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static bool _applicationIsQuitting = false;

        // Property to access the singleton instance
        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    return null;
                }

                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        var singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";
                    }
                }

                return _instance;
            }
        }

        // Method for manual instance retrieval (for consistency)
        public static T GetInstance()
        {
            return Instance;
        }

        // This is called when the object is initialized
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }

            if(this == _instance)
            {
                OnAwake();
            }
        }

        // Ensure that the singleton doesn't persist after quitting the application
        private void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        // Abstract method to be implemented by derived classes
        protected abstract void OnAwake();
    }

}