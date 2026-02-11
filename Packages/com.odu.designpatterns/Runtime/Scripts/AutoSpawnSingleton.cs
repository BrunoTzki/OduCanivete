using UnityEngine;

namespace OduLib.Canivete.DesignPatterns
{
    /// <summary>
    /// Classe Singleton que cria automaticamente uma instância se nenhuma for encontrada na cena.
    /// Estende Singleton<T> e garante que sempre haverá uma instância disponível.
    /// </summary>
    /// <typeparam name="T">O tipo de componente que deve derivar de MonoBehaviour</typeparam>
    public class AutoSpawnSingleton<T> : Singleton<T> where T : Component
    {
        private static T _instance;

        /// <summary>
        /// Retorna uma instância da classe Singleton encontrada no primeiro objeto da cena que possuir essa classe em seu script
        /// </summary>
        public new static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                    if (_instance == null)
                    {
                        GameObject instanceObject = new GameObject(typeof(T).Name);
                        _instance = instanceObject.AddComponent<T>();
                    }                
                }
                return _instance;
            }
        }
    }
}
