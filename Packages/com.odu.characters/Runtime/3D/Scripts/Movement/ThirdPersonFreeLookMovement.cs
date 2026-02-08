using UnityEngine;

namespace OduLib.Systems.Characters.ThreeDimensional
{
    /// <summary>
    /// <para><b>Responsabilidade</b></para>
    /// <para>
    /// Implementa movimentação em terceira pessoa com câmera livre (<i>free look</i>),
    /// onde a direção do movimento é relativa à orientação da câmera.
    /// </para>
    ///
    /// <para><b>Comportamento principal</b></para>
    /// <list type="bullet">
    /// <item>
    /// <description>Move o personagem utilizando <see cref="Rigidbody.MovePosition"/>.</description>
    /// </item>
    /// <item>
    /// <description>Converte o input 2D em um vetor 3D relativo à câmera.</description>
    /// </item>
    /// <item>
    /// <description>Alinha o visual do personagem com a direção de deslocamento.</description>
    /// </item>
    /// </list>
    ///
    /// <para><b>Integração com estados</b></para>
    /// <para>
    /// Esta classe é ativada e desativada via <see cref="Enter"/> e <see cref="Exit"/>,
    /// permitindo troca dinâmica de modos de movimentação.
    /// </para>
    /// </summary>
    public class ThirdPersonFreeLookMovement : CharacterMovement
    {
        /// <summary>
        /// <para><b>Rigidbody do personagem</b></para>
        /// <para>
        /// Utilizado para movimentação física através de MovePosition,
        /// garantindo compatibilidade com colisões.
        /// </para>
        /// </summary>
        [SerializeField] private Rigidbody _rigidbody;

        /// <summary>
        /// <para><b>Transform visual do personagem</b></para>
        /// <para>
        /// Representa apenas a malha ou modelo visual,
        /// permitindo rotação independente do collider.
        /// </para>
        /// </summary>
        [SerializeField] private Transform _visualTransform;

        /// <summary>
        /// <para><b>Transform da câmera</b></para>
        /// <para>
        /// Usado como referência para calcular a direção do movimento
        /// relativa ao olhar do jogador.
        /// </para>
        /// </summary>
        [SerializeField] private Transform _cameraTransform;

        /// <summary>
        /// <para><b>Velocidade de movimento</b></para>
        /// <para>
        /// Define a intensidade do deslocamento por segundo.
        /// </para>
        /// </summary>
        [SerializeField] private float _speed;

        /// <summary>
        /// <para><b>Ativação do modo free look</b></para>
        /// <para>
        /// Habilita o componente, reseta o input de movimento
        /// e bloqueia o cursor para controle da câmera.
        /// </para>
        /// </summary>
        public override void Enter()
        {
            base.Enter();

            enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        /// <summary>
        /// <para><b>Desativação do modo free look</b></para>
        /// <para>
        /// Desabilita o componente e restaura o estado padrão do cursor.
        /// </para>
        /// </summary>
        public override void Exit()
        {
            base.Exit();

            enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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
        /// Rotaciona o modelo visual apenas quando há input de movimento,
        /// evitando rotações indesejadas quando parado.
        /// </para>
        /// </summary>
        /// <param name="direction">
        /// Direção final de movimento no espaço do mundo.
        /// </param>
        private void TryChangeLookDirection(Vector3 direction)
        {
            if (Mathf.Abs(_movementDirection.x) <= 0 && Mathf.Abs(_movementDirection.y) <= 0)
            {
                return;
            }

            var lookDirection = direction;
            lookDirection.y = 0;

            _visualTransform.forward = lookDirection;
        }

        /// <summary>
        /// <para><b>Cálculo da direção de movimento</b></para>
        /// <para>
        /// Converte o input bidimensional em um vetor tridimensional
        /// relativo à orientação atual da câmera.
        /// </para>
        /// </summary>
        /// <returns>
        /// Vetor de direção normalizado no plano XZ.
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