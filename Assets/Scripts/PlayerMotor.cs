using UnityEngine;

namespace NeonRush
{
    /// <summary>
    /// Unico responsavel por mover o CharacterController do jogador.
    /// Combina a posicao lateral suavizada do <see cref="LaneMover"/> com a velocidade
    /// vertical do <see cref="JumpAndSlide"/> em um unico Move() por frame.
    /// O eixo Z fica travado: nesta prova de conceito o jogador permanece parado e o cenario
    /// e que se desloca em direcao a ele (etapa 3).
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(LaneMover))]
    [RequireComponent(typeof(JumpAndSlide))]
    public class PlayerMotor : MonoBehaviour
    {
        CharacterController controller;
        LaneMover laneMover;
        JumpAndSlide jumpAndSlide;

        void Awake()
        {
            controller = GetComponent<CharacterController>();
            laneMover = GetComponent<LaneMover>();
            jumpAndSlide = GetComponent<JumpAndSlide>();
        }

        void Update()
        {
            Vector3 position = transform.position;

            Vector3 delta = new Vector3(
                laneMover.CurrentX - position.x,
                jumpAndSlide.VerticalVelocity * Time.deltaTime,
                0f);

            controller.Move(delta);
        }
    }
}
