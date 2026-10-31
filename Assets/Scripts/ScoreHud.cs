using UnityEngine;
using UnityEngine.UI;

namespace NeonRush
{
    /// <summary>
    /// Exibe a pontuacao durante a partida. Observa o <see cref="ScoreManager"/>, sem
    /// conhecer a logica de coleta nem o jogador.
    /// Esta e uma interface provisoria: a etapa 6 substitui por uma tela completa.
    /// </summary>
    public class ScoreHud : MonoBehaviour
    {
        [Tooltip("Texto que mostra a pontuacao.")]
        [SerializeField] Text scoreText;

        [Tooltip("Prefixo exibido antes do numero.")]
        [SerializeField] string prefix = "CRYSTALS  ";

        [Tooltip("Sufixo exibido depois do numero.")]
        [SerializeField] string suffix = string.Empty;

        [Header("Pulso ao pontuar")]
        [Tooltip("Escala do pulso ao ganhar pontos. 1 desliga o efeito.")]
        [SerializeField, Range(1f, 1.6f)] float pulseScale = 1.15f;

        [Tooltip("Duracao do pulso, em segundos.")]
        [SerializeField, Min(0.01f)] float pulseDuration = 0.22f;

        RectTransform pulseTarget;
        float pulseRemaining;

        void Awake()
        {
            pulseTarget = scoreText != null ? scoreText.rectTransform : null;
        }

        void Update()
        {
            if (pulseTarget == null || pulseRemaining <= 0f)
            {
                return;
            }

            pulseRemaining -= Time.unscaledDeltaTime;

            if (pulseRemaining <= 0f)
            {
                pulseTarget.localScale = Vector3.one;
                return;
            }

            // Pico no instante da coleta e volta suave ao tamanho normal.
            float t = pulseRemaining / pulseDuration;
            pulseTarget.localScale = Vector3.one * Mathf.Lerp(1f, pulseScale, t);
        }

        void OnEnable()
        {
            ScoreManager.ScoreChanged += OnScoreChanged;
            OnScoreChanged(ScoreManager.Score);
        }

        void OnDisable()
        {
            ScoreManager.ScoreChanged -= OnScoreChanged;
        }

        void OnScoreChanged(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = prefix + score + suffix;
            }

            pulseRemaining = pulseDuration;
        }
    }
}
