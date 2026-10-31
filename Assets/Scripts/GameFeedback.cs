using UnityEngine;
using UnityEngine.UI;

namespace NeonRush
{
    /// <summary>
    /// Ponto unico de retorno audiovisual do jogo: flash na tela, tremor de camera e sons.
    ///
    /// Os clipes de audio sao opcionais e configuraveis no Inspector. O projeto nao possui
    /// arquivos de audio, portanto os campos ficam vazios por padrao e o componente apenas
    /// ignora a reproducao. Basta arrastar os clipes depois para o som entrar em vigor,
    /// sem nenhuma alteracao de codigo.
    ///
    /// As chamadas estaticas mantem os chamadores (cristal, jogador) ignorantes de como o
    /// retorno e implementado.
    /// </summary>
    public class GameFeedback : MonoBehaviour
    {
        [Header("Flash na tela")]
        [Tooltip("Imagem que cobre a tela inteira, sem capturar cliques.")]
        [SerializeField] Image flashImage;

        [Tooltip("Cor do flash ao coletar um cristal.")]
        [SerializeField] Color collectColor = new Color(0.45f, 1f, 0.6f, 0.18f);

        [Tooltip("Duracao do flash de coleta, em segundos.")]
        [SerializeField, Min(0.01f)] float collectFlashDuration = 0.18f;

        [Tooltip("Cor do flash no impacto.")]
        [SerializeField] Color crashColor = new Color(1f, 0.2f, 0.45f, 0.45f);

        [Tooltip("Duracao do flash de impacto, em segundos.")]
        [SerializeField, Min(0.01f)] float crashFlashDuration = 0.45f;

        [Header("Camera")]
        [Tooltip("Tremor disparado no impacto.")]
        [SerializeField] CameraShake cameraShake;

        [Header("Audio (opcional)")]
        [Tooltip("Fonte de musica de fundo.")]
        [SerializeField] AudioSource musicSource;

        [Tooltip("Fonte dos efeitos sonoros.")]
        [SerializeField] AudioSource sfxSource;

        [Tooltip("Musica de fundo. Deixe vazio se nao houver arquivo.")]
        [SerializeField] AudioClip musicClip;

        [Tooltip("Som ao coletar cristal. Deixe vazio se nao houver arquivo.")]
        [SerializeField] AudioClip collectClip;

        [Tooltip("Som de impacto. Deixe vazio se nao houver arquivo.")]
        [SerializeField] AudioClip crashClip;

        [Tooltip("Som de salto. Deixe vazio se nao houver arquivo.")]
        [SerializeField] AudioClip jumpClip;

        [Tooltip("Som de deslize. Deixe vazio se nao houver arquivo.")]
        [SerializeField] AudioClip slideClip;

        [Tooltip("Volume dos efeitos sonoros.")]
        [SerializeField, Range(0f, 1f)] float sfxVolume = 0.6f;

        [Tooltip("Volume da musica.")]
        [SerializeField, Range(0f, 1f)] float musicVolume = 0.35f;

        /// <summary>Instancia ativa, usada pelas chamadas estaticas.</summary>
        public static GameFeedback Instance { get; private set; }

        Color flashBaseColor;
        float flashRemaining;
        float flashDuration;

        void Awake()
        {
            Instance = this;

            if (flashImage != null)
            {
                flashBaseColor = flashImage.color;
                flashImage.color = Clear(flashBaseColor);
                flashImage.raycastTarget = false;
            }

            if (sfxSource != null)
            {
                sfxSource.playOnAwake = false;
                sfxSource.volume = sfxVolume;
            }

            if (musicSource != null)
            {
                musicSource.playOnAwake = false;
                musicSource.loop = true;
                musicSource.volume = musicVolume;
            }
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        void Update()
        {
            UpdateFlash();
        }

        /// <summary>Liga a musica de fundo, se houver clipe configurado.</summary>
        public void StartMusic()
        {
            if (musicSource != null && musicClip != null && !musicSource.isPlaying)
            {
                musicSource.clip = musicClip;
                musicSource.Play();
            }
        }

        /// <summary>Para a musica de fundo.</summary>
        public void StopMusic()
        {
            if (musicSource != null && musicSource.isPlaying)
            {
                musicSource.Stop();
            }
        }

        /// <summary>Retorno de coleta de cristal.</summary>
        public static void CrystalCollected()
        {
            GameFeedback feedback = Instance;
            if (feedback == null)
            {
                return;
            }

            feedback.PlaySfx(feedback.collectClip);
            feedback.DoFlash(feedback.collectColor, feedback.collectFlashDuration);
        }

        /// <summary>Retorno de impacto contra obstaculo.</summary>
        public static void PlayerCrashed()
        {
            GameFeedback feedback = Instance;
            if (feedback == null)
            {
                return;
            }

            feedback.PlaySfx(feedback.crashClip);
            feedback.DoFlash(feedback.crashColor, feedback.crashFlashDuration);

            if (feedback.cameraShake != null)
            {
                feedback.cameraShake.Shake();
            }
        }

        /// <summary>Retorno de salto.</summary>
        public static void PlayerJumped()
        {
            GameFeedback feedback = Instance;
            if (feedback != null)
            {
                feedback.PlaySfx(feedback.jumpClip);
            }
        }

        /// <summary>Retorno de deslize.</summary>
        public static void PlayerSlid()
        {
            GameFeedback feedback = Instance;
            if (feedback != null)
            {
                feedback.PlaySfx(feedback.slideClip);
            }
        }

        void PlaySfx(AudioClip clip)
        {
            if (sfxSource != null && clip != null)
            {
                sfxSource.PlayOneShot(clip, sfxVolume);
            }
        }

        void DoFlash(Color color, float duration)
        {
            if (flashImage == null)
            {
                return;
            }

            flashBaseColor = color;
            flashDuration = duration;
            flashRemaining = duration;
            flashImage.color = color;
        }

        void UpdateFlash()
        {
            if (flashImage == null || flashRemaining <= 0f)
            {
                return;
            }

            flashRemaining -= Time.unscaledDeltaTime;

            if (flashRemaining <= 0f)
            {
                flashRemaining = 0f;
                flashImage.color = Clear(flashBaseColor);
                return;
            }

            float alpha = flashBaseColor.a * (flashRemaining / flashDuration);
            flashImage.color = new Color(flashBaseColor.r, flashBaseColor.g, flashBaseColor.b, alpha);
        }

        static Color Clear(Color color)
        {
            return new Color(color.r, color.g, color.b, 0f);
        }
    }
}
