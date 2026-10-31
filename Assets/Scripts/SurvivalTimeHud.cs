using UnityEngine;
using UnityEngine.UI;

namespace NeonRush
{
    /// <summary>
    /// Exibe o tempo de sobrevivencia. Observa o <see cref="DifficultyManager"/>, sem
    /// conhecer os geradores nem o jogador.
    /// Interface provisoria: a etapa 6 substitui por uma tela completa.
    /// </summary>
    public class SurvivalTimeHud : MonoBehaviour
    {
        [Tooltip("Texto que mostra o tempo.")]
        [SerializeField] Text timeText;

        [Tooltip("Prefixo exibido antes do tempo.")]
        [SerializeField] string prefix = "TIME  ";

        [Tooltip("Casas decimais do tempo exibido.")]
        [SerializeField, Range(0, 2)] int decimals = 1;

        void Update()
        {
            if (timeText == null)
            {
                return;
            }

            DifficultyManager difficulty = DifficultyManager.Instance;
            float time = difficulty != null ? difficulty.SurvivalTime : 0f;

            timeText.text = prefix + time.ToString("F" + decimals);
        }
    }
}
