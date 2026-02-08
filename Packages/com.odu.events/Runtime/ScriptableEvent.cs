#if CINEMACHINE_PRESENT
using Unity.Cinemachine;
#endif

using System.Collections.Generic;
using UnityEngine;

namespace OduLib.Canivete.Events
{
    /// <summary>
    /// <para><b>Responsabilidade</b></para>
    /// <para>
    /// Define um evento baseado em <see cref="ScriptableObject"/>,
    /// permitindo a criação de sinais reutilizáveis e desacoplados
    /// entre sistemas.
    /// </para>
    ///
    /// <para><b>Padrão arquitetural</b></para>
    /// <para>
    /// Este script segue o padrão de <i>Scriptable Events</i>,
    /// onde a lógica de resposta ao evento é implementada
    /// em classes derivadas, enquanto a referência ao evento
    /// pode ser compartilhada via Inspector.
    /// </para>
    ///
    /// <para><b>Vantagens</b></para>
    /// <list type="bullet">
    /// <item>
    /// <description>Desacoplamento entre quem dispara e quem reage ao evento.</description>
    /// </item>
    /// <item>
    /// <description>Reutilização do mesmo evento em múltiplos contextos.</description>
    /// </item>
    /// <item>
    /// <description>Facilidade de debug e inspeção no Editor.</description>
    /// </item>
    /// </list>
    /// </summary>
    public class ScriptableEvent : ScriptableObject
    {
        /// <summary>
        /// <para><b>Disparo simples do evento</b></para>
        /// <para>
        /// Deve ser sobrescrito para executar lógica
        /// sem parâmetros adicionais.
        /// </para>
        /// </summary>
        public virtual void Invoke()
        {
        }

#if CINEMACHINE_PRESENT
        /// <summary>
        /// <para><b>Disparo do evento com câmera virtual</b></para>
        /// <para>
        /// Variante do evento que recebe uma
        /// <see cref="CinemachineCamera"/> como contexto.
        /// </para>
        ///
        /// <para><b>Compilação condicional</b></para>
        /// <para>
        /// Este método só estará disponível quando o símbolo
        /// <c>CINEMACHINE_PRESENT</c> estiver definido.
        /// </para>
        /// </summary>
        /// <param name="virtualCamera">
        /// Câmera virtual associada ao evento.
        /// </param>
        public virtual void Invoke(CinemachineCamera virtualCamera)
        {
        }
#endif

        /// <summary>
        /// <para><b>Disparo do evento com lista de objetos</b></para>
        /// <para>
        /// Variante do evento que recebe uma coleção de
        /// <see cref="GameObject"/> como contexto.
        /// </para>
        /// </summary>
        /// <param name="gameObjects">
        /// Lista de objetos associados ao evento.
        /// </param>
        public virtual void Invoke(List<GameObject> gameObjects)
        {
        }
    }
}
