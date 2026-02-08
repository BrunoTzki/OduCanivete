using UnityEngine;

namespace OduLib.Systems.Characters
{
    /// <summary>
    /// <para><b>Responsabilidade</b></para>
    /// <para>
    /// Implementa um sistema de pulo simples baseado em <see cref="Rigidbody"/>,
    /// validando se o personagem está no chão antes de aplicar a força.
    /// </para>
    ///
    /// <para><b>Fluxo de funcionamento</b></para>
    /// <list type="number">
    /// <item>
    /// <description>Um sistema externo registra a intenção de pulo via <see cref="JumpInputPerformed"/>.</description>
    /// </item>
    /// <item>
    /// <description>No <see cref="Update"/>, o estado de chão é verificado com Raycast.</description>
    /// </item>
    /// <item>
    /// <description>Se estiver no chão e houver intenção, a força de pulo é aplicada.</description>
    /// </item>
    /// </list>
    ///
    /// <para><b>Observações</b></para>
    /// <para>
    /// Este componente assume um modelo de input desacoplado da lógica de movimento,
    /// permitindo melhor controle e prevenção de múltiplos pulos por frame.
    /// </para>
    /// </summary>
    public class SimpleJump : MonoBehaviour
    {
        /// <summary>
        /// <para><b>Rigidbody do personagem</b></para>
        /// <para>
        /// Utilizado para aplicar a força vertical responsável pelo pulo.
        /// </para>
        /// </summary>
        [SerializeField] private Rigidbody _rigidbody;

        /// <summary>
        /// <para><b>Máscara de camadas consideradas como chão</b></para>
        /// <para>
        /// Usada pelo Raycast para filtrar quais colisores
        /// permitem que o personagem pule.
        /// </para>
        /// </summary>
        [SerializeField] private LayerMask _groundLayerMask;

        /// <summary>
        /// <para><b>Origem do Raycast de verificação de chão</b></para>
        /// <para>
        /// Normalmente posicionada próxima aos pés do personagem
        /// para garantir detecção precisa do solo.
        /// </para>
        /// </summary>
        [SerializeField] private Transform _jumpRaycastOrigin;

        /// <summary>
        /// <para><b>Força base do pulo</b></para>
        /// <para>
        /// Este valor é multiplicado internamente antes de ser aplicado
        /// ao <see cref="Rigidbody"/>.
        /// </para>
        /// </summary>
        [SerializeField] private float _jumpForce;

        /// <summary>
        /// <para><b>Intenção de pulo</b></para>
        /// <para>
        /// Indica que o jogador solicitou um pulo no frame atual.
        /// É resetado após a execução da lógica.
        /// </para>
        /// </summary>
        private bool _wannaJump;

        /// <summary>
        /// <para><b>Estado de contato com o chão</b></para>
        /// <para>
        /// Atualizado a cada frame através de Raycast.
        /// </para>
        /// </summary>
        private bool _isGrounded;

        /// <summary>
        /// <para><b>Registro de input de pulo</b></para>
        /// <para>
        /// Deve ser chamado pelo sistema de input quando a ação de pulo ocorrer.
        /// </para>
        /// <para>
        /// Este método não executa o pulo diretamente,
        /// apenas registra a intenção.
        /// </para>
        /// </summary>
        public void JumpInputPerformed()
        {
            _wannaJump = true;
        }

        private void Update()
        {
            JumpBehaviour();
        }

        /// <summary>
        /// <para><b>Controle principal do pulo</b></para>
        /// <para>
        /// Verifica se o personagem está no chão e se existe
        /// uma intenção de pulo registrada.
        /// </para>
        /// <para>
        /// A intenção é sempre resetada ao final do método,
        /// garantindo execução única por input.
        /// </para>
        /// </summary>
        private void JumpBehaviour()
        {
            _isGrounded = Physics.Raycast(
                _jumpRaycastOrigin.position,
                Vector3.down,
                0.2f,
                _groundLayerMask
            );

            if (_isGrounded && _wannaJump)
            {
                Jump();
            }

            _wannaJump = false;
        }

        /// <summary>
        /// <para><b>Execução do pulo</b></para>
        /// <para>
        /// Aplica uma força vertical no <see cref="Rigidbody"/>
        /// para impulsionar o personagem para cima.
        /// </para>
        /// <para>
        /// Não redefine a velocidade atual,
        /// portanto o resultado depende do estado físico corrente.
        /// </para>
        /// </summary>
        private void Jump()
        {
            _rigidbody.AddForce(Vector3.up * 100f * _jumpForce);
        }
    }
}