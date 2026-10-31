using System;
using UnityEngine;

namespace NeonRush
{
    /// <summary>
    /// Cristal colecionavel. Gira e flutua visualmente, e ao ser tocado pelo jogador
    /// soma pontos uma unica vez e volta para o pool.
    /// A deteccao usa trigger pelo mesmo motivo dos obstaculos: quem se move e o cristal,
    /// e o CharacterController do jogador nao recebe OnControllerColliderHit nesse caso.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Crystal : MonoBehaviour
    {
        [Header("Valor")]
        [Tooltip("Pontos somados ao coletar.")]
        [SerializeField] int points = 10;

        [Header("Animacao")]
        [Tooltip("Velocidade de rotacao, em graus por segundo.")]
        [SerializeField] float spinSpeed = 110f;

        [Tooltip("Amplitude da flutuacao vertical, em unidades. Aplicada apenas ao visual.")]
        [SerializeField] float floatAmplitude = 0.12f;

        [Tooltip("Velocidade da flutuacao.")]
        [SerializeField] float floatSpeed = 2.4f;

        [Tooltip("Raiz visual que gira e flutua. Nao altera o collider de coleta.")]
        [SerializeField] Transform visual;

        /// <summary>Corpo rigido cinematico usado para deslocar o cristal.</summary>
        public Rigidbody Body { get; private set; }

        /// <summary>Verdadeiro depois de coletado, ate voltar ao pool.</summary>
        public bool IsCollected { get; private set; }
/// <summary>Disparado uma unica vez, quando o jogador coleta este cristal.</summary>
        public event Action<Crystal> Collected;

        
        Vector3 visualStartLocalPosition;
        float floatPhase;

        void Awake()
        {
            Body = GetComponent<Rigidbody>();

            if (visual == null)
            {
                visual = transform.childCount > 0 ? transform.GetChild(0) : null;
            }

            if (visual != null)
            {
                visualStartLocalPosition = visual.localPosition;
            }
        }

        void OnEnable()
        {
            IsCollected = false;

            // Fase aleatoria evita que todos os cristais flutuem em sincronia.
            floatPhase = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
        }

        void Update()
        {
            if (visual == null)
            {
                return;
            }

            visual.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.Self);

            float offsetY = Mathf.Sin(Time.time * floatSpeed + floatPhase) * floatAmplitude;
            visual.localPosition = visualStartLocalPosition + new Vector3(0f, offsetY, 0f);
        }

        void OnTriggerEnter(Collider other)
        {
            if (IsCollected || GameState.IsGameOver)
            {
                return;
            }

            if (other.GetComponentInParent<PlayerMotor>() == null)
            {
                return;
            }

            // Marca antes de qualquer outra coisa: garante contagem unica mesmo que
            // varios colliders do jogador entrem no trigger no mesmo frame.
            IsCollected = true;
            ScoreManager.AddPoints(points);
            GameFeedback.CrystalCollected();
            Collected?.Invoke(this);
        }
    }
}
