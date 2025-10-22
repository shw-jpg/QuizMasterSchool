using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;

    [SerializeField] AudioClip startMusic;
    [SerializeField] AudioClip quizMusic;
    [SerializeField] AudioClip endMusic;
    [SerializeField] AudioClip clickSound;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "StartScene": PlayBGM(startMusic); break;
            case "QuizScene": PlayBGM(quizMusic); break;
            case "EndScene": PlayBGM(endMusic); break;
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayClickSound()
    {
        PlaySFX(clickSound);
    }
}
