using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace NeonRush
{
    /// <summary>
    /// Ponto unico de leitura do teclado do jogador.
    /// O projeto usa somente o Input System novo (Player Settings &gt; Active Input Handling = Input System),
    /// por isso nao ha uso de UnityEngine.Input.
    ///
    /// As acoes sao amostradas uma unica vez por frame por <see cref="PlayerInputPoller"/> e guardadas
    /// aqui. Todos os consumidores leem o mesmo valor, independentemente da ordem de execucao deles.
    /// Desligar <see cref="Enabled"/> bloqueia o controle do jogo (os menus usam isso na etapa 6).
    /// </summary>
    public static class PlayerInput
    {
        /// <summary>Quando falso, nenhuma acao do jogador e lida.</summary>
        public static bool Enabled { get; set; } = true;

        public static bool LaneLeft { get; private set; }

        public static bool LaneRight { get; private set; }

        public static bool Jump { get; private set; }

        public static bool Slide { get; private set; }

        /// <summary>Amostra o teclado do frame atual. Chamado apenas pelo <see cref="PlayerInputPoller"/>.</summary>
        public static void Poll()
        {
            Keyboard keyboard = Enabled ? Keyboard.current : null;

            LaneLeft = Pressed(keyboard?.aKey, keyboard?.leftArrowKey);
            LaneRight = Pressed(keyboard?.dKey, keyboard?.rightArrowKey);
            Jump = Pressed(keyboard?.spaceKey, keyboard?.upArrowKey);
            Slide = Pressed(keyboard?.sKey, keyboard?.downArrowKey);
        }

        static bool Pressed(KeyControl primary, KeyControl alternate)
        {
            return (primary != null && primary.wasPressedThisFrame)
                || (alternate != null && alternate.wasPressedThisFrame);
        }
    }
}
