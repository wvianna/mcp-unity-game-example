using UnityEngine;

namespace NeonRush
{
    /// <summary>
    /// Resolve gravidade, salto e deslize temporario do jogador.
    /// O deslize reduz a altura do CharacterController por um tempo fixo e depois restaura
    /// exatamente a altura e o centro originais guardados no Awake.
    /// Assim como o LaneMover, nao chama Move(): apenas expoe VerticalVelocity.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [DefaultExecutionOrder(-50)]
    public class JumpAndSlide : MonoBehaviour
    {
        [Header("Salto")]
        [Tooltip("Altura maxima do salto, em unidades.")]
        [SerializeField, Min(0f)] float jumpHeight = 1.6f;

        [Tooltip("Aceleracao da gravidade, em unidades por segundo ao quadrado (negativa).")]
        [SerializeField] float gravity = -22f;

        [Tooltip("Velocidade vertical negativa aplicada quando o jogador esta no chao, para manter contato.")]
        [SerializeField] float groundStick = -3f;

        [Tooltip("Velocidade de queda maxima aplicada ao jogador.")]
        [SerializeField] float terminalFallSpeed = -30f;

        [Header("Deslize")]
        [Tooltip("Duracao do deslize, em segundos.")]
        [SerializeField, Min(0.05f)] float slideDuration = 0.7f;

        [Tooltip("Altura do CharacterController durante o deslize. A base do collider fica no mesmo lugar.")]
        [SerializeField, Min(0.1f)] float slideHeight = 0.9f;

        [Header("Visual")]
        [Tooltip("Raiz visual achatada durante o deslize (normalmente o GameObject do robo). Opcional.")]
        [SerializeField] Transform visualRoot;

        [Tooltip("Escala Y aplicada em visualRoot durante o deslize.")]
        [SerializeField, Range(0.2f, 1f)] float slideSquash = 0.5f;

        CharacterController controller;
        float verticalVelocity;
        float slideTimer;

        // Valores originais, usados para restaurar o collider ao fim do deslize.
        float standHeight;
        float standCenterY;
        Vector3 visualStandScale;

        /// <summary>Verdadeiro enquanto o deslize estiver ativo.</summary>
        public bool IsSliding => slideTimer > 0f;

        /// <summary>Verdadeiro quando o CharacterController detecta contato com o chao.</summary>
        public bool IsGrounded => controller.isGrounded;

        /// <summary>Velocidade vertical atual (positiva ao subir).</summary>
        public float VerticalVelocity => verticalVelocity;

        void Awake()
        {
            controller = GetComponent<CharacterController>();
            standHeight = controller.height;
            standCenterY = controller.center.y;

            if (visualRoot != null)
            {
                visualStandScale = visualRoot.localScale;
            }
        }

        void Update()
        {
            UpdateSlideTimer();

            if (PlayerInput.Slide && controller.isGrounded && !IsSliding)
            {
                StartSlide();
            }

            if (PlayerInput.Jump && controller.isGrounded && !IsSliding)
            {
                // v = sqrt(2 * g * h): velocidade necessaria para atingir jumpHeight.
                verticalVelocity = Mathf.Sqrt(2f * jumpHeight * -gravity);
                GameFeedback.PlayerJumped();
            }

            if (controller.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = groundStick;
            }

            verticalVelocity = Mathf.Max(verticalVelocity + gravity * Time.deltaTime, terminalFallSpeed);
        }

        void UpdateSlideTimer()
        {
            if (slideTimer <= 0f)
            {
                return;
            }

            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
            {
                EndSlide();
            }
        }

        void StartSlide()
        {
            slideTimer = slideDuration;
            ApplyColliderHeight(slideHeight);

            if (visualRoot != null)
            {
                visualRoot.localScale = new Vector3(visualStandScale.x, visualStandScale.y * slideSquash, visualStandScale.z);
            }

            GameFeedback.PlayerSlid();
        }

        void EndSlide()
        {
            slideTimer = 0f;
            ApplyColliderHeight(standHeight);

            // Restaura o centro exato guardado no Awake, nao um valor recalculado.
            controller.center = new Vector3(controller.center.x, standCenterY, controller.center.z);

            if (visualRoot != null)
            {
                visualRoot.localScale = visualStandScale;
            }
        }

        /// <summary>Ajusta a altura mantendo a base do collider ancorada no mesmo ponto.</summary>
        void ApplyColliderHeight(float height)
        {
            controller.height = height;
            controller.center = new Vector3(controller.center.x, height * 0.5f, controller.center.z);
        }
    }
}
