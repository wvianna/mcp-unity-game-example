using System;
using UnityEngine;

namespace NeonRush
{
    /// <summary>
    /// Pontuacao da partida. Fica separada do jogador e dos cristais: quem coleta apenas
    /// informa o valor, quem exibe apenas observa.
    /// </summary>
    public static class ScoreManager
    {
        /// <summary>Pontuacao atual da partida.</summary>
        public static int Score { get; private set; }

        /// <summary>Disparado sempre que a pontuacao muda, com o novo valor.</summary>
        public static event Action<int> ScoreChanged;

        /// <summary>
        /// Zera a pontuacao no inicio de cada Play e descarta assinantes antigos,
        /// evitando que eventos fiquem pendurados quando o domain reload esta desligado.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            Score = 0;
            ScoreChanged = null;
        }

        /// <summary>Soma pontos a partida em andamento.</summary>
        public static void AddPoints(int points)
        {
            if (GameState.IsGameOver)
            {
                return;
            }

            Score += points;
            ScoreChanged?.Invoke(Score);
        }

        /// <summary>Reinicia a pontuacao para uma nova partida.</summary>
        public static void Reset()
        {
            Score = 0;
            ScoreChanged?.Invoke(Score);
        }
    }
}
