using System.Collections.Generic;
using UnityEngine;

namespace NeonRush
{
    /// <summary>
    /// Distribui cristais pelas faixas, desloca-os em direcao ao jogador e recicla os que passam
    /// ou sao coletados.
    ///
    /// Estrategia de pooling: uma unica lista de cristais inativos, aquecida no Awake, de forma que
    /// durante a partida nenhum objeto seja instanciado.
    ///
    /// Alcançabilidade: antes de ativar um cristal, a posicao e testada contra a cena fisica.
    /// Se houver um obstaculo ocupando o local, o cristal nao e colocado, evitando
    /// colecionaveis inalcancaveis.
    /// </summary>
    [DisallowMultipleComponent]
    public class CrystalSpawner : MonoBehaviour
    {
        [Header("Prefab")]
        [SerializeField] Crystal crystalPrefab;

        [Header("Faixas")]
        [Tooltip("Largura de uma faixa, em unidades. Deve casar com o LaneMover.")]
        [SerializeField, Min(0.1f)] float laneWidth = 2.5f;

        [Tooltip("Quantidade de faixas.")]
        [SerializeField, Min(1)] int laneCount = 3;

        [Header("Movimento")]
        [Tooltip("Velocidade quando nao ha DifficultyManager na cena. Com o gerenciador presente, o valor dele prevalece.")]
        [SerializeField, Min(0f)] float moveSpeed = 12f;

        [Tooltip("Posicao Z onde o cristal nasce.")]
        [SerializeField] float spawnZ = 60f;

        [Tooltip("Posicao Z em que o cristal e reciclado.")]
        [SerializeField] float despawnZ = -8f;

        [Header("Distribuicao")]
        [Tooltip("Intervalo quando nao ha DifficultyManager na cena. Com o gerenciador presente, o valor dele prevalece.")]
        [SerializeField, Min(0.1f)] float spawnInterval = 1.9f;

        [Tooltip("Distancia minima entre grupos, em unidades.")]
        [SerializeField, Min(1f)] float minGapDistance = 16f;

        [Tooltip("Altura do centro do cristal acima do chao.")]
        [SerializeField] float crystalHeight = 0.95f;

        [Header("Pool")]
        [Tooltip("Cristais inativos criados no inicio da partida.")]
        [SerializeField, Min(0)] int poolWarmup = 8;

        [Tooltip("Raio usado para checar se a vaga esta livre de obstaculos.")]
        [SerializeField, Min(0.1f)] float clearanceRadius = 0.7f;

        readonly List<Crystal> pool = new List<Crystal>();
        readonly List<Crystal> active = new List<Crystal>();
        float distanceSinceSpawn;

        /// <summary>Cristais ativos no momento (para diagnostico).</summary>
        public int ActiveCount => active.Count;

        /// <summary>Cristais criados no total, ativos ou no pool (para diagnostico).</summary>
        public int PoolSize => active.Count + pool.Count;

        /// <summary>Devolve todos os cristais ativos ao pool. Usado ao reiniciar a partida.</summary>
        public void ClearAll()
        {
            for (int i = active.Count - 1; i >= 0; i--)
            {
                Crystal crystal = active[i];
                if (crystal != null)
                {
                    Recycle(crystal);
                }
            }

            active.Clear();
            distanceSinceSpawn = 0f;
        }

        float SpawnGap => Mathf.Max(minGapDistance, CurrentSpeed * CurrentInterval);

        /// <summary>Velocidade efetiva: a do gerenciador quando existir, senao a local.</summary>
        float CurrentSpeed => DifficultyManager.Instance != null
            ? DifficultyManager.Instance.Speed
            : moveSpeed;

        /// <summary>Intervalo efetivo: o do gerenciador quando existir, senao o local.</summary>
        float CurrentInterval => DifficultyManager.Instance != null
            ? DifficultyManager.Instance.CrystalInterval
            : spawnInterval;

        void Awake()
        {
            for (int i = 0; i < poolWarmup; i++)
            {
                Crystal warmed = Create();
                if (warmed == null)
                {
                    break;
                }

                pool.Add(warmed);
            }
        }

        void Update()
        {
            if (GameState.IsGameOver)
            {
                return;
            }

            float step = CurrentSpeed * Time.deltaTime;
            distanceSinceSpawn += step;

            MoveActive(step);

            if (distanceSinceSpawn >= SpawnGap)
            {
                distanceSinceSpawn = 0f;
                SpawnGroup();
            }
        }

        void MoveActive(float step)
        {
            for (int i = active.Count - 1; i >= 0; i--)
            {
                Crystal crystal = active[i];

                if (crystal == null)
                {
                    active.RemoveAt(i);
                    continue;
                }

                Vector3 position = crystal.transform.position;
                position.z -= step;

                // MovePosition mantem a cena fisica em sincronia (Physics.autoSyncTransforms = false).
                crystal.Body.MovePosition(position);

                if (position.z < despawnZ)
                {
                    Recycle(crystal);
                }
            }
        }

        /// <summary>
        /// Coloca cristais em uma ou duas faixas, uma unica vez por grupo.
        /// A faixa e sorteada e a vaga e validada contra obstaculos antes de nascer.
        /// </summary>
        void SpawnGroup()
        {
            int count = Random.Range(1, Mathf.Clamp(laneCount, 1, 2) + 1);
            int firstLane = Random.Range(0, laneCount);

            TrySpawn(firstLane);

            if (count > 1)
            {
                int lane = Random.Range(0, laneCount - 1);
                if (lane >= firstLane)
                {
                    lane++;
                }

                TrySpawn(lane);
            }
        }

        void TrySpawn(int lane)
        {
            float x = (lane - laneCount / 2) * laneWidth;
            Vector3 spawnPosition = new Vector3(x, crystalHeight, spawnZ);

            // Vaga precisa estar livre: cristal dentro de um obstaculo seria inalcancavel.
            Collider[] blockers = Physics.OverlapSphere(spawnPosition, clearanceRadius, ~0, QueryTriggerInteraction.Collide);
            for (int i = 0; i < blockers.Length; i++)
            {
                if (blockers[i].GetComponentInParent<Obstacle>() != null)
                {
                    return;
                }
            }

            Crystal crystal = Take();
            if (crystal == null)
            {
                return;
            }

            crystal.transform.position = spawnPosition;
            crystal.gameObject.SetActive(true);
            active.Add(crystal);
        }

        Crystal Take()
        {
            while (pool.Count > 0)
            {
                int last = pool.Count - 1;
                Crystal pooled = pool[last];
                pool.RemoveAt(last);

                if (pooled != null)
                {
                    return pooled;
                }
            }

            return Create();
        }

        void Recycle(Crystal crystal)
        {
            crystal.gameObject.SetActive(false);
            pool.Add(crystal);
            active.Remove(crystal);
        }

        void OnCollected(Crystal crystal)
        {
            if (crystal != null && active.Contains(crystal))
            {
                Recycle(crystal);
            }
        }

        Crystal Create()
        {
            if (crystalPrefab == null)
            {
                return null;
            }

            Crystal instance = Instantiate(crystalPrefab, transform);
            instance.Collected += OnCollected;
            instance.gameObject.SetActive(false);
            return instance;
        }
    }
}
