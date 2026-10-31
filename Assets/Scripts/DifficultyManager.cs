using UnityEngine;

namespace NeonRush
{
    /// <summary>
    /// Fonte unica da dificuldade da partida. Os geradores de obstaculos e de cristais
    /// leem a velocidade e o intervalo daqui, em vez de guardarem copias proprias,
    /// para que exista apenas um lugar onde a progressao e calculada.
    ///
    /// A progressao e linear e saturante: parte dos valores iniciais e chega aos limites
    /// em <see cref="rampDuration"/> segundos de jogo, permanecendo saturada depois.
    /// Os limites existem justamente para que a partida continue possivel.
    /// </summary>
    [DisallowMultipleComponent]
    public class DifficultyManager : MonoBehaviour
    {
        [Header("Velocidade")]
        [Tooltip("Velocidade inicial do cenario, em unidades por segundo.")]
        [SerializeField, Min(0f)] float initialSpeed = 12f;

        [Tooltip("Velocidade maxima. A progressao satura aqui e nunca passa.")]
        [SerializeField, Min(0f)] float maxSpeed = 26f;

        [Header("Geracao de obstaculos")]
        [Tooltip("Intervalo entre ondas no inicio da partida, em segundos.")]
        [SerializeField, Min(0.1f)] float initialSpawnInterval = 1.6f;

        [Tooltip("Intervalo minimo entre ondas. A progressao nunca desce abaixo disto.")]
        [SerializeField, Min(0.1f)] float minSpawnInterval = 0.85f;

        [Header("Geracao de cristais")]
        [Tooltip("Intervalo entre grupos de cristais no inicio da partida.")]
        [SerializeField, Min(0.1f)] float initialCrystalInterval = 1.9f;

        [Tooltip("Intervalo minimo entre grupos de cristais.")]
        [SerializeField, Min(0.1f)] float minCrystalInterval = 1.15f;

        [Header("Progressao")]
        [Tooltip("Tempo de jogo, em segundos, para a dificuldade ir do inicio ao maximo.")]
        [SerializeField, Min(1f)] float rampDuration = 75f;

        /// <summary>Instancia ativa, para leitura pelos geradores.</summary>
        public static DifficultyManager Instance { get; private set; }

        /// <summary>Velocidade atual do cenario, em unidades por segundo.</summary>
        public float Speed { get; private set; }

        /// <summary>Intervalo atual entre ondas de obstaculos, em segundos.</summary>
        public float ObstacleInterval { get; private set; }

        /// <summary>Intervalo atual entre grupos de cristais, em segundos.</summary>
        public float CrystalInterval { get; private set; }

        /// <summary>Tempo de sobrevivencia da partida em andamento, em segundos.</summary>
        public float SurvivalTime { get; private set; }

        /// <summary>Progresso normalizado da curva de dificuldade, de 0 a 1.</summary>
        public float Progress { get; private set; }

        void Awake()
        {
            Instance = this;
            ResetToStart();
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        void Update()
        {
            // Depois do fim de partida o tempo e a dificuldade congelam,
            // mantendo os valores finais coerentes com a cena parada.
            if (GameState.IsGameOver)
            {
                return;
            }

            SurvivalTime += Time.deltaTime;

            Progress = Mathf.Clamp01(SurvivalTime / rampDuration);

            Speed = Mathf.Lerp(initialSpeed, maxSpeed, Progress);
            ObstacleInterval = Mathf.Lerp(initialSpawnInterval, minSpawnInterval, Progress);
            CrystalInterval = Mathf.Lerp(initialCrystalInterval, minCrystalInterval, Progress);
        }

        /// <summary>Restaura os parametros iniciais para uma nova partida.</summary>
        public void ResetToStart()
        {
            SurvivalTime = 0f;
            Progress = 0f;
            Speed = initialSpeed;
            ObstacleInterval = initialSpawnInterval;
            CrystalInterval = initialCrystalInterval;
        }
    }
}
