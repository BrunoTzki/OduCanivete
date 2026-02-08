#define DESIGNPATTERNS_PRESENT

using UnityEngine;

namespace OduLib.Canivete.DesignPatterns
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        /// <summary>
        /// Retorna uma instância da classe Singleton encontrada no primeiro objeto da cena que possuir essa classe em seu script
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();                   
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else
            {
                if (_instance != this)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
