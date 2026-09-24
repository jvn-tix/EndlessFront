using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;

    [Header("SFX Audio Clips")]
    public AudioClip shootSfx;

    [Header("BGM Audio Clips")]
    public AudioClip homeMenuBGM;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Mulai memutar BGM saat game dimulai
        PlayBGM(homeMenuBGM);
    }

    // Fungsi serbaguna untuk memutar efek suara sekali jalan
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.pitch = Random.Range(0.9f, 1.1f);
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null || bgmSource == null) return;
        
            if(bgmSource.clip == clip && bgmSource.isPlaying) return;

            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
     }

    public void StopBGM()
    {
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
            bgmSource.clip = null;
        }
    }   
}