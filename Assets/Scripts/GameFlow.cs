using UnityEngine;
using UnityEngine.UI;

namespace NeonRush
{
    /// <summary>
    /// Conduz o fluxo da partida: menu inicial, partida em andamento e tela de fim de jogo.
    ///
    /// Centraliza tres responsabilidades que precisam andar juntas:
    /// - trocar qual tela esta visivel;
    /// - bloquear a entrada do jogador fora da partida (<see cref="PlayerInput.Enabled"/>);
    /// - ligar e desligar os geradores, para que nada se mova atras do menu.
    ///
    /// O reinicio nao recarrega a cena: os estados do jogo sao estaticos e sobreviveriam a um
    /// reload, entao eles sao resetados explicitamente aqui.
    /// </summary>
    public class GameFlow : MonoBehaviour
    {
        [Header("Telas")]
        [Tooltip("Painel da tela inicial.")]
        [SerializeField] GameObject menuPanel;

        [Tooltip("Painel da tela de fim de jogo.")]
        [SerializeField] GameObject gameOverPanel;

        [Tooltip("Grupo com os textos exibidos durante a partida (pontuacao e tempo).")]
        [SerializeField] GameObject gameplayHud;

        [Header("Botoes")]
        [SerializeField] Button playButton;
        [SerializeField] Button restartButton;

        [Header("Textos")]
        [Tooltip("Texto com a pontuacao final na tela de fim de jogo.")]
        [SerializeField] Text finalScoreText;

        [Header("Mundo")]
        [Tooltip("Geradores desligados enquanto o menu esta visivel.")]
        [SerializeField] Behaviour[] generators;

        [SerializeField] LaneMover laneMover;

        void Awake()
        {
            GameState.GameOver += HandleGameOver;

            if (playButton != null)
            {
                playButton.onClick.AddListener(StartMatch);
            }

            if (restartButton != null)
            {
                restartButton.onClick.AddListener(StartMatch);
            }
        }

        void OnDestroy()
        {
            GameState.GameOver -= HandleGameOver;

            if (playButton != null)
            {
                playButton.onClick.RemoveListener(StartMatch);
            }

            if (restartButton != null)
            {
                restartButton.onClick.RemoveListener(StartMatch);
            }
        }

        void Start()
        {
            ShowMenu();
        }

        /// <summary>Apresenta a tela inicial com o jogo parado.</summary>
        public void ShowMenu()
        {
            SetScreen(menu: true, hud: false, gameOver: false);
            SetGameplayEnabled(false);
            ResetMatchState();
        }

        /// <summary>Inicia uma partida limpa. Tambem e a acao de reiniciar.</summary>
        public void StartMatch()
        {
            ResetMatchState();

            SetScreen(menu: false, hud: true, gameOver: false);
            SetGameplayEnabled(true);

            if (GameFeedback.Instance != null)
            {
                GameFeedback.Instance.StartMusic();
            }
        }

        void HandleGameOver()
        {
            SetGameplayEnabled(false);
            SetScreen(menu: false, hud: false, gameOver: true);

            if (finalScoreText != null)
            {
                finalScoreText.text = ScoreManager.Score.ToString();
            }

            if (GameFeedback.Instance != null)
            {
                GameFeedback.Instance.StopMusic();
            }

            GameFeedback.PlayerCrashed();
        }

        void ResetMatchState()
        {
            GameState.Reset();
            ScoreManager.Reset();

            if (DifficultyManager.Instance != null)
            {
                DifficultyManager.Instance.ResetToStart();
            }

            if (laneMover != null)
            {
                laneMover.ResetToCenter();
            }

            ClearGenerators();
        }

        /// <summary>
        /// Devolve ao pool tudo que estava em jogo. Sem isso, os obstaculos da partida anterior
        /// continuariam parados na cena depois do reinicio.
        /// </summary>
        void ClearGenerators()
        {
            if (generators == null)
            {
                return;
            }

            for (int i = 0; i < generators.Length; i++)
            {
                if (generators[i] is ObstacleSpawner obstacles)
                {
                    obstacles.ClearAll();
                }
                else if (generators[i] is CrystalSpawner crystals)
                {
                    crystals.ClearAll();
                }
            }
        }

        void SetGameplayEnabled(bool enabled)
        {
            // Fora da partida nenhuma acao do teclado e lida.
            PlayerInput.Enabled = enabled;

            if (generators == null)
            {
                return;
            }

            for (int i = 0; i < generators.Length; i++)
            {
                if (generators[i] != null)
                {
                    generators[i].enabled = enabled;
                }
            }
        }

        void SetScreen(bool menu, bool hud, bool gameOver)
        {
            if (menuPanel != null)
            {
                menuPanel.SetActive(menu);
            }

            if (gameplayHud != null)
            {
                gameplayHud.SetActive(hud);
            }

            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(gameOver);
            }
        }
    }
}
