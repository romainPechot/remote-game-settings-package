namespace MadBox.Exercice
{
    using UnityEngine;

    public class SingletonManager : MonoBehaviour
    {
    }

    public class SingletonManager<T> : SingletonManager where T : SingletonManager
    {
        private static T manager = null;

        public static T GetManager()
        {
            if (SingletonManager<T>.manager == null)
            {
                GameObject instance = new GameObject(typeof(T).Name);
                SingletonManager<T>.manager = instance.AddComponent<T>();
                Object.DontDestroyOnLoad(instance);
            }

            return SingletonManager<T>.manager;
        }
    }
}
