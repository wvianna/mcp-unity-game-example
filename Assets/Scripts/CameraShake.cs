using UnityEngine;

namespace NeonRush
{
    /// <summary>
    /// Tremor curto de camera usado no impacto. Guarda a pose original e restaura no fim,
    /// para que a camera volte exatamente ao enquadramento definido na cena.
    /// </summary>
    public class CameraShake : MonoBehaviour
    {
        [Tooltip("Duracao do tremor, em segundos.")]
        [SerializeField, Min(0.01f)] float duration = 0.35f;

        [Tooltip("Deslocamento maximo, em unidades.")]
        [SerializeField, Min(0f)] float magnitude = 0.22f;

        Vector3 originPosition;
        float remaining;

        void Awake()
        {
            originPosition = transform.localPosition;
        }

        void Update()
        {
            if (remaining <= 0f)
            {
                return;
            }

            remaining -= Time.deltaTime;

            if (remaining <= 0f)
            {
                transform.localPosition = originPosition;
                return;
            }

            // A queda quadratica faz o tremor morrer suavemente.
            float strength = magnitude * (remaining / duration);
            transform.localPosition = originPosition + (Vector3)(Random.insideUnitCircle * strength);
        }

        /// <summary>Inicia o tremor. Chamadas durante um tremor ativo apenas o reiniciam.</summary>
        public void Shake()
        {
            remaining = duration;
        }
    }
}
