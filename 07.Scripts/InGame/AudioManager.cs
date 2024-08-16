using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    // AudioSource ������Ʈ
    private AudioSource loopAudioSource;
    private AudioSource oneShotAudioSource;

    void Awake()
    {
        // �̱��� ����
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ����� �Ŵ��� ����
        }
    }

    void Start()
    {
        // AudioSource ������Ʈ�� ��������
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length >= 2)
        {
            loopAudioSource = audioSources[0];
            oneShotAudioSource = audioSources[1];
        }
    }

    // ���带 ����ϴ� �޼���
    public void PlaySound(AudioClip clip)
    {
        if (oneShotAudioSource != null && clip != null)
        {
            oneShotAudioSource.PlayOneShot(clip);
        }
    }

    // ���带 ������ ����ϴ� �޼���
    public void PlaySoundLoop(AudioClip clip)
    {
        if (loopAudioSource != null && clip != null)
        {
            loopAudioSource.clip = clip;
            loopAudioSource.loop = true;
            loopAudioSource.Play();
        }
    }

    // ���� ���带 �����ϴ� �޼���
    public void StopSoundLoop()
    {
        if (loopAudioSource != null)
        {
            loopAudioSource.Stop();
            loopAudioSource.loop = false;
        }
    }
}
