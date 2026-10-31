using System;
using UnityEngine;

namespace NeonRush
{
    /// <summary>
    /// Estado global minimo da partida, compartilhado entre os sistemas de jogo.
    /// A etapa 6 usa o evento <see cref="GameOver"/> para exibir a tela final,
    /// sem que os obstaculos precisem conhecer a interface.
    /// </summary>
    public static class GameState
    {
        /// <summary>Verdadeiro a partir da primeira colisao valida da partida.</summary>
        public static bool IsGameOver { get; private set; }

        /// <summary>Disparado uma unica vez por partida, quando o jogo termina.</summary>
        public static event Action GameOver;

        /// <summary>
        /// Zera o estado no inicio de cada Play. Alem de restaurar a partida, evita que
        /// eventos fiquem pendurados quando o domain reload esta desligado.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            IsGameOver = false;
            GameOver = null;
        }

        /// <summary>Encerra a partida. Chamadas repetidas sao ignoradas.</summary>
        public static void MarkGameOver()
        {
            if (IsGameOver)
            {
                return;
            }

            IsGameOver = true;
            GameOver?.Invoke();
        }

        /// <summary>
        /// Reabre a partida para um novo inicio. Mantem os assinantes de <see cref="GameOver"/>
        /// intactos, para que a interface continue ouvindo o fim da proxima partida.
        /// </summary>
        public static void Reset()
        {
            IsGameOver = false;
        }
    }
}
