using System.Collections.Generic;
using UnityEngine;

namespace NeonRush
{
    /// <summary>
    /// Gera ondas de obstaculos, desloca-os em direcao ao jogador e recicla os que passam.
    ///
    /// Estrategia de pooling: cada tipo tem sua propria lista de objetos inativos, aquecida no Awake.
    /// Durante a partida nenhum obstaculo e instanciado, desde que a onda ativa nunca exceda a reserva.
    ///
    /// Solubilidade garantida: uma onda bloqueia no maximo (laneCount - 1) faixas, portanto sempre
    /// existe pelo menos uma faixa livre, qualquer que seja o tipo sorteado.
    /// </summary>
    [DisallowMultipleComponent]
    public class ObstacleSpawner : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] Obstacle blockerPrefab;
        [SerializeField] Obstacle lowPrefab;
        [SerializeField] Obstacle highPrefab;

        [Header("Faixas")]
        [Tooltip("Largura de uma faixa, em unidades. Deve casar com o LaneMover.")]
        [SerializeField, Min(0.1f)] float laneWidth = 2.5f;

        [Tooltip("Quantidade de faixas. Com 3, cada onda bloqueia no maximo 2.")]
        [SerializeField, Min(2)] int laneCount = 3;

        [Header("Movimento")]
        [Tooltip("Velocidade quando nao ha DifficultyManager na cena. Com o gerenciador presente, o valor dele prevalece.")]
        [SerializeField, Min(0f)] float moveSpeed = 12f;

        [Tooltip("Posicao Z onde a onda nasce (a frente do jogador).")]
        [SerializeField] float spawnZ = 60f;

        [Tooltip("Posicao Z em que o obstaculo e reciclado (atras do jogador).")]
        [SerializeField] float despawnZ = -8f;

        [Header("Geracao")]
        [Tooltip("Intervalo quando nao ha DifficultyManager na cena. Com o gerenciador presente, o valor dele prevalece.")]
        [SerializeField, Min(0.1f)] float spawnInterval = 1.6f;

        [Tooltip("Distancia minima entre ondas, em unidades. Sempre respeitada.")]
        [SerializeField, Min(1f)] float minGapDistance = 14f;

        [Header("Pool")]
        [Tooltip("Objetos inativos criados por tipo no inicio da partida.")]
        [SerializeField, Min(0)] int poolWarmupPerType = 4;

        readonly List<Obstacle>[] pools = new List<Obstacle>[3];
        readonly List<Obstacle> active = new List<Obstacle>();
        float distanceSinceSpawn;

        /// <summary>Quantidade de obstaculos ativos no momento (para diagnostico).</summary>
        public int ActiveCount => active.Count;

        /// <summary>Devolve todos os obstaculos ativos ao pool. Usado ao reiniciar a partida.</summary>
        public void ClearAll()
        {
            for (int i = active.Count - 1; i >= 0; i--)
            {
                Obstacle obstacle = active[i];
                if (obstacle != null)
                {
                    Release(obstacle);
                }
            }

            active.Clear();
            distanceSinceSpawn = 0f;
        }

        /// <summary>Distancia, em unidades, entre duas ondas consecutivas.</summary>
        float SpawnGap => Mathf.Max(minGapDistance, CurrentSpeed * CurrentInterval);

        /// <summary>Velocidade efetiva: a do gerenciador quando existir, senao a local.</summary>
        float CurrentSpeed => DifficultyManager.Instance != null
            ? DifficultyManager.Instance.Speed
            : moveSpeed;

        /// <summary>Intervalo efetivo: o do gerenciador quando existir, senao o local.</summary>
        float CurrentInterval => DifficultyManager.Instance != null
            ? DifficultyManager.Instance.ObstacleInterval
            : spawnInterval;

        void Awake()
        {
            for (int type = 0; type < pools.Length; type++)
            {
                pools[type] = new List<Obstacle>();

                for (int i = 0; i < poolWarmupPerType; i++)
                {
                    Obstacle warmed = Create((ObstacleType)type);
                    if (warmed == null)
                    {
                        break;
                    }

                    pools[type].Add(warmed);
                }
            }
        }

        void Update()
        {
            // Depois do fim de partida os obstaculos congelam, mantendo a cena coerente.
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
                SpawnWave();
            }
        }

        void MoveActive(float step)
        {
            for (int i = active.Count - 1; i >= 0; i--)
            {
                Obstacle obstacle = active[i];

                if (obstacle == null)
                {
                    active.RemoveAt(i);
                    continue;
                }

                Vector3 position = obstacle.transform.position;
                position.z -= step;

                // MovePosition mantem a cena fisica em sincronia (ver Obstacle.Body).
                obstacle.Body.MovePosition(position);

                if (position.z < despawnZ)
                {
                    active.RemoveAt(i);
                    Release(obstacle);
                }
            }
        }

        /// <summary>
        /// Sorteia uma onda com 1 ou 2 obstaculos em faixas distintas.
        /// Nunca ocupa todas as faixas, o que garante que sempre ha rota possivel.
        /// </summary>
        void SpawnWave()
        {
            int maxObstacles = Mathf.Max(1, laneCount - 1);
            int obstacleCount = Random.Range(1, maxObstacles + 1);

            int firstLane = Random.Range(0, laneCount);
            Spawn(firstLane, RandomType());

            for (int placed = 1; placed < obstacleCount; placed++)
            {
                // Sorteia entre as faixas restantes, sem repetir a primeira.
                int lane = Random.Range(0, laneCount - 1);
                if (lane >= firstLane)
                {
                    lane++;
                }

                Spawn(lane, RandomType());
            }
        }

        void Spawn(int lane, ObstacleType type)
        {
            Obstacle obstacle = Take(type);
            if (obstacle == null)
            {
                return;
            }

            float x = (lane - laneCount / 2) * laneWidth;
            obstacle.transform.position = new Vector3(x, 0f, spawnZ);
            obstacle.gameObject.SetActive(true);
            active.Add(obstacle);
        }

        ObstacleType RandomType()
        {
            return (ObstacleType)Random.Range(0, 3);
        }

        Obstacle Take(ObstacleType type)
        {
            List<Obstacle> pool = pools[(int)type];

            if (pool.Count > 0)
            {
                int last = pool.Count - 1;
                Obstacle pooled = pool[last];
                pool.RemoveAt(last);

                if (pooled != null)
                {
                    return pooled;
                }
            }

            return Create(type);
        }

        void Release(Obstacle obstacle)
        {
            obstacle.gameObject.SetActive(false);
            pools[(int)obstacle.Type].Add(obstacle);
        }

        Obstacle Create(ObstacleType type)
        {
            Obstacle prefab = PrefabFor(type);
            if (prefab == null)
            {
                return null;
            }

            Obstacle instance = Instantiate(prefab, transform);
            instance.Type = type;
            instance.gameObject.SetActive(false);
            return instance;
        }

        Obstacle PrefabFor(ObstacleType type)
        {
            switch (type)
            {
                case ObstacleType.Low:
                    return lowPrefab;
                case ObstacleType.High:
                    return highPrefab;
                default:
                    return blockerPrefab;
            }
        }
    }
}
