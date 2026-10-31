using UnityEngine;

namespace NeonRush
{
    /// <summary>
    /// Mantem em qual das tres faixas o jogador esta e resolve a posicao X desejada
    /// com transicao suave. Nao move o CharacterController: quem aplica o deslocamento
    /// e o <see cref="PlayerMotor"/>, para que exista um unico Move() por frame.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class LaneMover : MonoBehaviour
    {
        [Header("Faixas")]
        [Tooltip("Largura de uma faixa, em unidades. Os centros ficam em x = (indice - centro) * largura.")]
        [SerializeField, Min(0.1f)] float laneWidth = 2.5f;

        [Tooltip("Quantidade de faixas. A prova de conceito usa 3.")]
        [SerializeField, Min(1)] int laneCount = 3;

        [Header("Transicao")]
        [Tooltip("Tempo aproximado, em segundos, para percorrer uma faixa.")]
        [SerializeField, Min(0.01f)] float laneChangeDuration = 0.14f;

        int targetLane;
        float currentX;
        float lateralVelocity;

        /// <summary>Indice da faixa alvo (0 = esquerda).</summary>
        public int TargetLane => targetLane;

        /// <summary>Posicao X ja suavizada, pronta para o motor aplicar.</summary>
        public float CurrentX => currentX;

        void Awake()
        {
            targetLane = laneCount / 2;
            currentX = transform.position.x;
        }

        void Update()
        {
            if (PlayerInput.LaneLeft)
            {
                targetLane = Mathf.Max(0, targetLane - 1);
            }

            if (PlayerInput.LaneRight)
            {
                targetLane = Mathf.Min(laneCount - 1, targetLane + 1);
            }

            // O clamp abaixo e a garantia de que o jogador nunca sai dos limites laterais.
            float targetX = LaneToX(targetLane);
            float halfTrack = (laneCount / 2) * laneWidth;
            targetX = Mathf.Clamp(targetX, -halfTrack, halfTrack);

            currentX = Mathf.SmoothDamp(currentX, targetX, ref lateralVelocity, laneChangeDuration);
        }

        float LaneToX(int lane)
        {
            return (lane - laneCount / 2) * laneWidth;
        }

        /// <summary>
        /// Devolve o jogador para a faixa central sem transicao. Usado ao iniciar ou reiniciar
        /// a partida, para que o estado lateral nao vaze de uma partida para a seguinte.
        /// </summary>
        public void ResetToCenter()
        {
            targetLane = laneCount / 2;
            currentX = LaneToX(targetLane);
            lateralVelocity = 0f;
        }
    }
}
