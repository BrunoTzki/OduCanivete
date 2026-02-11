#define EVENTS_PRESENT

using System;
using System.Collections;
using UnityEngine;

#if DESIGNPATTERNS_PRESENT
using OduLib.Canivete.DesignPatterns;
#endif

namespace OduLib.Canivete.Events
{
#if DESIGNPATTERNS_PRESENT
    /// <summary>
    /// <para><b>Responsabilidade</b></para>
    /// <para>
    /// Executor centralizado de <see cref="Coroutine"/>s,
    /// projetado para existir como instância única global
    /// através do padrão <c>Singleton</c>.
    /// </para>
    /// </summary>
    public class CoroutineRunner : AutoSpawnSingleton<CoroutineRunner>
    {
#else
    /// <summary>
    /// <para><b>Responsabilidade</b></para>
    /// <para>
    /// Executor centralizado de <see cref="Coroutine"/>s,
    /// permitindo disparar rotinas assincronas a partir
    /// de classes que não herdam de <see cref="MonoBehaviour"/>.
    /// </para>
    /// </summary>
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner _instance;
        public static CoroutineRunner Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<CoroutineRunner>();
                    if (_instance == null)
                    {
                        GameObject runnerObject = new GameObject("CoroutineRunner");
                        _instance = runnerObject.AddComponent<CoroutineRunner>();
                    }                
                }
                return _instance;
            }
        }
#endif
        /// <summary>
        /// <para><b>Ciclo de vida</b></para>
        /// <para>
        /// Garante que o objeto responsável pela execução
        /// de corrotinas persista entre trocas de cena.
        /// </para>
        ///
        /// <para>
        /// Essencial para delays, timers e sequências
        /// que não devem ser interrompidas por carregamento
        /// de cenas.
        /// </para>
        /// </summary>
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// <para><b>Execução direta de corrotina</b></para>
        /// <para>
        /// Inicia uma <see cref="IEnumerator"/> como
        /// <see cref="Coroutine"/> gerenciada por este runner.
        /// </para>
        ///
        /// <para><b>Uso comum</b></para>
        /// <para>
        /// Ideal para sistemas estáticos, ScriptableObjects
        /// ou serviços de domínio que não podem chamar
        /// <see cref="MonoBehaviour.StartCoroutine"/>.
        /// </para>
        /// </summary>
        /// <param name="method">
        /// Método enumerador que define a corrotina.
        /// </param>
        public void RunCoroutine(IEnumerator method)
        {
            StartCoroutine(method);
        }

        /// <summary>
        /// <para><b>Execução atrasada de ação</b></para>
        /// <para>
        /// Agenda a execução de um método após um
        /// intervalo de tempo em segundos.
        /// </para>
        ///
        /// <para><b>Observação</b></para>
        /// <para>
        /// Quando <paramref name="seconds"/> é igual a zero,
        /// a execução ocorre no próximo frame,
        /// garantindo consistência com o loop da Unity.
        /// </para>
        /// </summary>
        /// <param name="method">
        /// Ação a ser executada após o atraso.
        /// </param>
        /// <param name="seconds">
        /// Tempo de espera em segundos.
        /// </param>
        public void WaitToRun(Action method, float seconds)
        {
            StartCoroutine(WaitToRunCoroutine(method, seconds));
        }

        /// <summary>
        /// <para><b>Corrotina interna de atraso</b></para>
        /// <para>
        /// Implementa a lógica de espera antes da execução
        /// da ação fornecida.
        /// </para>
        /// </summary>
        /// <param name="method">
        /// Método a ser invocado após a espera.
        /// </param>
        /// <param name="seconds">
        /// Duração da espera.
        /// </param>
        private IEnumerator WaitToRunCoroutine(Action method, float seconds)
        {
            if (seconds == 0)
            {
                // Aguarda um frame para manter previsibilidade
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(seconds);
            }

            method?.Invoke();
        }
    }
}