using UnityEngine;

namespace OduLib.Systems.Characters
{
    /// <summary>
    /// <para><b>Responsabilidade</b></para>
    /// <para>
    /// Classe base para sistemas de movimentação de personagens.
    /// Centraliza o recebimento de inputs de movimento e pulo,
    /// delegando a execução para componentes especializados.
    /// </para>
    ///
    /// <para><b>Papel na arquitetura</b></para>
    /// <list type="bullet">
    /// <item>
    /// <description>Serve como ponto comum para diferentes modos de movimentação.</description>
    /// </item>
    /// <item>
    /// <description>Facilita extensão através de herança (<c>walk</c>, <c>run</c>, <c>fly</c>, etc).</description>
    /// </item>
    /// <item>
    /// <description>Desacopla input da lógica física real.</description>
    /// </item>
    /// </list>
    ///
    /// <para><b>Observações</b></para>
    /// <para>
    /// Esta classe não aplica movimento diretamente.
    /// Classes derivadas são responsáveis por interpretar
    /// <see cref="_movementDirection"/> e executar o deslocamento.
    /// </para>
    /// </summary>
    public class CharacterMovement : MonoBehaviour
    {
        /// <summary>
        /// <para><b>Componente de pulo</b></para>
        /// <para>
        /// Responsável por lidar com a lógica de pulo do personagem.
        /// A chamada de input é repassada para este componente.
        /// </para>
        /// </summary>
        [SerializeField] protected SimpleJump _jump;

        /// <summary>
        /// <para><b>Direção de movimento atual</b></para>
        /// <para>
        /// Representa a direção de movimento recebida via input.
        /// Normalmente interpretada por classes derivadas
        /// para aplicar deslocamento ou força.
        /// </para>
        /// </summary>
        protected Vector2 _movementDirection;

        /// <summary>
        /// <para><b>Entrada no estado de movimentação</b></para>
        /// <para>
        /// Deve ser chamado quando este modo de movimentação
        /// se torna ativo.
        /// </para>
        /// <para>
        /// Reseta o vetor de movimento para evitar
        /// deslocamentos residuais.
        /// </para>
        /// </summary>
        public virtual void Enter()
        {
            _movementDirection = Vector2.zero;
        }

        /// <summary>
        /// <para><b>Saída do estado de movimentação</b></para>
        /// <para>
        /// Deve ser chamado quando este modo de movimentação
        /// deixa de ser utilizado.
        /// </para>
        /// <para>
        /// Restaura o estado padrão do cursor,
        /// útil para transições para menus ou interfaces.
        /// </para>
        /// </summary>
        public virtual void Exit()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        /// <summary>
        /// <para><b>Registro de input de movimento</b></para>
        /// <para>
        /// Atualiza a direção de movimento baseada no input do jogador.
        /// </para>
        /// <param name="direction">
        /// Vetor normalizado ou bruto representando a intenção de movimento.
        /// </param>
        /// </summary>
        public void MoveInputPerformed(Vector2 direction)
        {
            _movementDirection = direction;
        }

        /// <summary>
        /// <para><b>Cancelamento de input de movimento</b></para>
        /// <para>
        /// Deve ser chamado quando o jogador solta o controle de movimento,
        /// garantindo que o personagem pare corretamente.
        /// </para>
        /// </summary>
        public void MoveInputCanceled()
        {
            _movementDirection = Vector2.zero;
        }

        /// <summary>
        /// <para><b>Registro de input de pulo</b></para>
        /// <para>
        /// Repassa a intenção de pulo para o componente
        /// responsável pela lógica de salto.
        /// </para>
        /// <seealso cref="SimpleJump.JumpInputPerformed"/>
        /// </summary>
        public void JumpInputPerformed()
        {
            _jump.JumpInputPerformed();
        }
    }
}