using UnityEngine;

namespace OduLib.Systems.Characters.ThreeDimensional
{
    /// <summary>
    /// <para><b>Responsabilidade</b></para>
    /// <para>
    /// Implementa movimentação em terceira pessoa com visão superior (<i>top vision</i>),
    /// onde o deslocamento é orientado pela câmera, mas sem controle direto de rotação da visão.
    /// </para>
    ///
    /// <para><b>Diferença em relação ao free look</b></para>
    /// <list type="bullet">
    /// <item>
    /// <description>A câmera é estática ou semi-estática, normalmente posicionada acima do personagem.</description>
    /// </item>
    /// <item>
    /// <description>Não há bloqueio de cursor nem controle livre de rotação de câmera.</description>
    /// </item>
    /// <item>
    /// <description>A direção de movimento continua relativa à orientação da câmera.</description>
    /// </item>
    /// </list>
    ///
    /// <para><b>Integração com estados</b></para>
    /// <para>
    /// Este modo pode ser ativado ou desativado dinamicamente através
    /// de <see cref="Enter"/> e <see cref="Exit"/>.
    /// </para>
    /// </summary>
    public class ThirdPersonTopVisionMovement : CharacterMovement
    {
        /// <summary>
        /// <para><b>Rigidbody do personagem</b></para>
        /// <para>
        /// Utilizado para movimentação física respeitando colisões.
        /// </para>
        /// </summary>
        [SerializeField] private Rigidbody _rigidbody;

        /// <summary>
        /// <para><b>Transform visual do personagem</b></para>
        /// <para>
        /// Responsável apenas pela orientação visual,
        /// permitindo rotação independente da cápsula física.
        /// </para>
        /// </summary>
        [SerializeField] private Transform _visualTransform;

        /// <summary>
        /// <para><b>Transform da câmera</b></para>
        /// <para>
        /// Utilizado como referência para converter o input
        /// em direção no espaço do mundo.
        /// </para>
        /// </summary>
        [SerializeField] private Transform _cameraTransform;

        /// <summary>
        /// <para><b>Velocidade de movimentação</b></para>
        /// <para>
        /// Define a intensidade do deslocamento por segundo.
        /// </para>
        /// </summary>
        [SerializeField] private float _speed;

        /// <summary>
        /// <para><b>Ativação do modo top vision</b></para>
        /// <para>
        /// Habilita o componente e prepara o estado
        /// para movimentação nesse modo.
        /// </para>
        /// </summary>
        public override void Enter()
        {
            base.Enter();

            enabled = true;
        }

        /// <summary>
        /// <para><b>Desativação do modo top vision</b></para>
        /// <para>
        /// Desabilita o componente e encerra o controle de movimento.
        /// </para>
        /// </summary>
        public override void Exit()
        {
            base.Exit();

            enabled = false;
        }

        private void FixedUpdate()
        {
            MoveAndLook();
        }

        /// <summary>
        /// <para><b>Execução de movimento e rotação</b></para>
        /// <para>
        /// Calcula a direção baseada na câmera, ajusta a orientação visual
        /// e move o personagem respeitando o timestep físico.
        /// </para>
        /// </summary>
        private void MoveAndLook()
        {
            Vector3 direction = CalculateDirection();

            TryChangeLookDirection(direction);

            _rigidbody.MovePosition(
                transform.position + (direction * _speed * Time.fixedDeltaTime)
            );
        }

        /// <summary>
        /// <para><b>Ajuste da direção visual</b></para>
        /// <para>
        /// Gira o modelo do personagem na direção do movimento,
        /// apenas quando há input ativo.
        /// </para>
        /// </summary>
        /// <param name="direction">
        /// Direção final de movimento no espaço do mundo.
        /// </param>
        private void TryChangeLookDirection(Vector3 direction)
        {
            if (Mathf.Abs(_movementDirection.x) > 0 || Mathf.Abs(_movementDirection.y) > 0)
            {
                var lookDirection = direction;
                lookDirection.y = 0;

                _visualTransform.forward = lookDirection;
            }
        }

        /// <summary>
        /// <para><b>Cálculo da direção de movimento</b></para>
        /// <para>
        /// Converte o input 2D em um vetor 3D relativo à câmera,
        /// mantendo o movimento restrito ao plano horizontal.
        /// </para>
        /// </summary>
        /// <returns>
        /// Vetor de direção no plano XZ.
        /// </returns>
        private Vector3 CalculateDirection()
        {
            var inputDirection = new Vector3(_movementDirection.x, 0, _movementDirection.y);
            var direction = _cameraTransform.TransformDirection(inputDirection);
            direction.y = 0;

            return direction;
        }
    }
}