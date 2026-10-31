using UnityEngine;

namespace NeonRush
{
    /// <summary>Tipo de obstaculo gerado.</summary>
    public enum ObstacleType
    {
        /// <summary>Bloqueia a faixa inteira: so pode ser evitado trocando de faixa.</summary>
        Blocker = 0,

        /// <summary>Baixo, rente ao chao: deve ser ultrapassado com salto.</summary>
        Low = 1,

        /// <summary>Suspenso: deve ser ultrapassado com deslize.</summary>
        High = 2,
    }

    /// <summary>
    /// Marcador de obstaculo. O <see cref="ObstacleSpawner"/> usa este componente para reconhecer
    /// um obstaculo ao detectar colisao com o jogador e para devolver o objeto ao pool correto.
    /// O tipo em tempo de execucao e atribuido na criacao/retirada do pool.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Obstacle : MonoBehaviour
    {
        /// <summary>Tipo deste obstaculo em tempo de execucao.</summary>
        public ObstacleType Type { get; set; }

        /// <summary>
        /// Corpo rigido cinematico. O deslocamento passa por ele (e nao por transform.position)
        /// para que o PhysX mantenha a posicao dos colliders atualizada: com
        /// Physics.autoSyncTransforms desligado, mover o transform nao atualiza a cena fisica.
        /// </summary>
        public Rigidbody Body { get; private set; }

        void Awake()
        {
            Body = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Deteccao de colisao. O collider do obstaculo e um trigger e quem se move e o obstaculo,
        /// por isso o evento nasce aqui: o CharacterController do jogador e passivo e nao recebe
        /// OnControllerColliderHit quando e outro corpo que entra nele.
        /// Como o trigger respeita a geometria real, cada tipo continua exigindo a acao correta:
        /// pular o obstaculo baixo e deslizar sob o suspenso deixam de sobrepor.
        /// </summary>
        void OnTriggerEnter(Collider other)
        {
            if (GameState.IsGameOver)
            {
                return;
            }

            if (other.GetComponentInParent<PlayerMotor>() != null)
            {
                GameState.MarkGameOver();
            }
        }
    }
}
