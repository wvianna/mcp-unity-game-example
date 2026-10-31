using UnityEngine;

namespace NeonRush
{
    /// <summary>
    /// Amostra o teclado antes de qualquer outro componente que leia <see cref="PlayerInput"/>.
    /// Garante que LaneMover, JumpAndSlide e futuros consumidores vejam exatamente o mesmo
    /// resultado no mesmo frame.
    /// </summary>
    [DefaultExecutionOrder(-200)]
    public class PlayerInputPoller : MonoBehaviour
    {
        void Update()
        {
            PlayerInput.Poll();
        }
    }
}
