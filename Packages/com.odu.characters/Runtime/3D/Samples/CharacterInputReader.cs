using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OduLib.Systems.Characters
{
    /// <summary>
    /// <para><b>Responsabilidade</b></para>
    /// <para>
    /// Atua como leitor e despachante de inputs do <see cref="UnityEngine.InputSystem"/>.
    /// Converte ações do jogador em eventos de alto nível e repassa comandos
    /// diretamente para o <see cref="CharacterMovement"/> ativo.
    /// </para>
    ///
    /// <para><b>Fluxo de entrada</b></para>
    /// <list type="number">
    /// <item>
    /// <description>O <see cref="ThirdPersonMovement"/> captura os inputs brutos.</description>
    /// </item>
    /// <item>
    /// <description>Eventos públicos são disparados para observadores externos.</description>
    /// </item>
    /// <item>
    /// <description>Os comandos também são repassados para <see cref="CurrentCharacterMovement"/>.</description>
    /// </item>
    /// </list>
    ///
    /// <para><b>Padrão arquitetural</b></para>
    /// <para>
    /// Este componente funciona como um <i>Input Adapter</i>,
    /// desacoplando o sistema de input da lógica de movimentação.
    /// </para>
    /// </summary>
    public class CharacterInputReader : MonoBehaviour
    {
        /// <summary>
        /// <para><b>Movimentação atualmente ativa</b></para>
        /// <para>
        /// Referência para o sistema de movimentação que receberá
        /// diretamente os comandos de input.
        /// </para>
        /// <para>
        /// Pode ser trocado em tempo de execução para suportar
        /// diferentes modos de movimento.
        /// </para>
        /// </summary>
        public CharacterMovement CurrentCharacterMovement;

        /// <summary>
        /// <para><b>Evento de movimento realizado</b></para>
        /// <para>
        /// Disparado quando o jogador fornece uma direção de movimento.
        /// </para>
        /// </summary>
        public Action<Vector2> MovePerformed;

        /// <summary>
        /// <para><b>Evento de cancelamento de movimento</b></para>
        /// <para>
        /// Disparado quando o input de movimento é interrompido.
        /// </para>
        /// </summary>
        public Action MoveCanceled;

        /// <summary>
        /// <para><b>Evento de troca de tipo ou modo</b></para>
        /// <para>
        /// Normalmente utilizado para alternar estados de movimentação
        /// ou comportamento do personagem.
        /// </para>
        /// </summary>
        public Action ChangeTypePerformed;

        /// <summary>
        /// <para><b>Evento de pulo</b></para>
        /// <para>
        /// Disparado quando o input de pulo é executado.
        /// </para>
        /// </summary>
        public Action JumpPerformed;

        /// <summary>
        /// <para><b>Mapa de ações de input</b></para>
        /// <para>
        /// Classe gerada automaticamente pelo New Input System,
        /// responsável por definir as ações e bindings.
        /// </para>
        /// </summary>
        private ThirdPersonMovement inputActions;

        private void OnEnable()
        {
            inputActions = new ThirdPersonMovement();

            inputActions.Movement.Move.performed += InputPerform;
            inputActions.Movement.Move.canceled += InputCancel;
            inputActions.Movement.Change.performed += Change;
            inputActions.Movement.Jump.performed += Jump;

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Movement.Move.performed -= InputPerform;
            inputActions.Movement.Move.canceled -= InputCancel;
            inputActions.Movement.Change.performed -= Change;
            inputActions.Movement.Jump.performed -= Jump;

            inputActions.Disable();
        }

        /// <summary>
        /// <para><b>Troca de tipo ou estado</b></para>
        /// <para>
        /// Invocado quando a ação de mudança é executada pelo jogador.
        /// </para>
        /// </summary>
        /// <param name="context">
        /// Contexto do callback fornecido pelo Input System.
        /// </param>
        private void Change(InputAction.CallbackContext context)
        {
            ChangeTypePerformed?.Invoke();
        }

        /// <summary>
        /// <para><b>Input de pulo</b></para>
        /// <para>
        /// Dispara o evento de pulo e repassa a intenção
        /// para o sistema de movimentação atual.
        /// </para>
        /// </summary>
        /// <param name="context">
        /// Contexto do callback fornecido pelo Input System.
        /// </param>
        private void Jump(InputAction.CallbackContext context)
        {
            JumpPerformed?.Invoke();
            CurrentCharacterMovement.JumpInputPerformed();
        }

        /// <summary>
        /// <para><b>Input de movimento realizado</b></para>
        /// <para>
        /// Lê o vetor de direção e o repassa para ouvintes externos
        /// e para o sistema de movimentação ativo.
        /// </para>
        /// </summary>
        /// <param name="callbackContext">
        /// Contexto do callback contendo o valor do input.
        /// </param>
        private void InputPerform(InputAction.CallbackContext callbackContext)
        {
            var direction = callbackContext.ReadValue<Vector2>();

            MovePerformed?.Invoke(direction);
            CurrentCharacterMovement.MoveInputPerformed(direction);
        }

        /// <summary>
        /// <para><b>Cancelamento de input de movimento</b></para>
        /// <para>
        /// Notifica ouvintes e garante que o personagem
        /// interrompa o deslocamento.
        /// </para>
        /// </summary>
        /// <param name="callbackContext">
        /// Contexto do callback fornecido pelo Input System.
        /// </param>
        private void InputCancel(InputAction.CallbackContext callbackContext)
        {
            MoveCanceled?.Invoke();
            CurrentCharacterMovement.MoveInputCanceled();
        }
    }
}