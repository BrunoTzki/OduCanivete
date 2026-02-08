using System;
using UnityEditor;

namespace OduLib.Canivete.EditorGeneral
{
    namespace OduLib.Systems.Editor
    {
        /// <summary>
        /// <para><b>Responsabilidade</b></para>
        /// <para>
        /// Centraliza eventos globais relacionados às transições de modo do Unity Editor
        /// (<i>Edit Mode</i> ↔ <i>Play Mode</i>).
        /// </para>
        ///
        /// <para><b>Quando é inicializado</b></para>
        /// <para>
        /// Graças ao atributo <see cref="InitializeOnLoad"/>, esta classe é carregada
        /// automaticamente sempre que o Unity Editor recompila scripts ou é iniciado.
        /// </para>
        ///
        /// <para><b>Ciclo de vida monitorado</b></para>
        /// <list type="bullet">
        /// <item>
        /// <description>Entrada e saída do Edit Mode.</description>
        /// </item>
        /// <item>
        /// <description>Entrada e saída do Play Mode.</description>
        /// </item>
        /// </list>
        ///
        /// <para><b>Padrão arquitetural</b></para>
        /// <para>
        /// Atua como um <i>Event Hub</i> estático do Editor,
        /// permitindo que sistemas desacoplados reajam às mudanças
        /// de estado sem depender diretamente de <see cref="EditorApplication"/>.
        /// </para>
        /// </summary>
        [InitializeOnLoad]
        public static class PrePlayEvents
        {
            /// <summary>
            /// <para><b>Evento: entrou no Edit Mode</b></para>
            /// <para>
            /// Disparado após o Unity finalizar a transição
            /// para o modo de edição.
            /// </para>
            /// </summary>
            public static Action EnteredEditMode;

            /// <summary>
            /// <para><b>Evento: saindo do Edit Mode</b></para>
            /// <para>
            /// Disparado imediatamente antes do Unity
            /// iniciar a transição para o Play Mode.
            /// </para>
            /// </summary>
            public static Action ExitingEditMode;

            /// <summary>
            /// <para><b>Evento: entrou no Play Mode</b></para>
            /// <para>
            /// Disparado após o Unity concluir a entrada
            /// no modo de execução.
            /// </para>
            /// </summary>
            public static Action EnteredPlayMode;

            /// <summary>
            /// <para><b>Evento: saindo do Play Mode</b></para>
            /// <para>
            /// Disparado imediatamente antes do Unity
            /// retornar ao Edit Mode.
            /// </para>
            /// </summary>
            public static Action ExitingPlayMode;

            /// <summary>
            /// <para><b>Inicialização estática</b></para>
            /// <para>
            /// Registra o callback de mudança de estado do Play Mode
            /// no <see cref="EditorApplication"/>.
            /// </para>
            /// <para>
            /// A remoção prévia do delegate garante que não haja
            /// múltiplas inscrições após recompilações.
            /// </para>
            /// </summary>
            static PrePlayEvents()
            {
                EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
                EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            }

            /// <summary>
            /// <para><b>Dispatcher de mudanças de Play Mode</b></para>
            /// <para>
            /// Recebe notificações do Unity Editor e redireciona
            /// para eventos semânticos de alto nível.
            /// </para>
            /// </summary>
            /// <param name="mode">
            /// Estado atual da transição de Play Mode.
            /// </param>
            private static void OnPlayModeStateChanged(PlayModeStateChange mode)
            {
                switch (mode)
                {
                    case PlayModeStateChange.EnteredEditMode:
                        EnterEditMode();
                        break;

                    case PlayModeStateChange.ExitingEditMode:
                        ExitEditMode();
                        break;

                    case PlayModeStateChange.EnteredPlayMode:
                        EnterPlayMode();
                        break;

                    case PlayModeStateChange.ExitingPlayMode:
                        ExitPlayMode();
                        break;
                }
            }

            /// <summary>
            /// <para><b>Disparo de saída do Edit Mode</b></para>
            /// <para>
            /// Notifica todos os ouvintes registrados.
            /// </para>
            /// </summary>
            private static void ExitEditMode()
            {
                ExitingEditMode?.Invoke();
            }

            /// <summary>
            /// <para><b>Disparo de entrada no Edit Mode</b></para>
            /// <para>
            /// Notifica todos os ouvintes registrados.
            /// </para>
            /// </summary>
            private static void EnterEditMode()
            {
                EnteredEditMode?.Invoke();
            }

            /// <summary>
            /// <para><b>Disparo de saída do Play Mode</b></para>
            /// <para>
            /// Notifica todos os ouvintes registrados.
            /// </para>
            /// </summary>
            private static void ExitPlayMode()
            {
                ExitingPlayMode?.Invoke();
            }

            /// <summary>
            /// <para><b>Disparo de entrada no Play Mode</b></para>
            /// <para>
            /// Notifica todos os ouvintes registrados.
            /// </para>
            /// </summary>
            private static void EnterPlayMode()
            {
                EnteredPlayMode?.Invoke();
            }
        }
    }
}