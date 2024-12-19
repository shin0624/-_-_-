using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    // AudioSource ������Ʈ
    private AudioSource audioSource;

    void Awake()
    {
        // �̱��� ����
        // �ڵ� ��� ���Ǽ��� ����
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
        audioSource = GetComponent<AudioSource>();
    }

    // ���带 ����ϴ� �޼���
    // AudioManager.Instance.PlaySound(AudioClip); ���� ���
    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void StopSound(AudioClip clip)//���� ��ž
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop(); // ���� ��� ���� ���� ����
            audioSource.loop = false; // ���� ����
            audioSource.clip = null; // Ŭ�� ����
        }
    }

    public void LoopSound(AudioClip clip)//���� ����. playOnShout�� �ܹ߼��̶� ���� �ȵ�
    {
        if (audioSource != null && clip != null)
        {
           if(audioSource.isPlaying && audioSource.clip==clip)
            {
                return;//�̹� �ش� Ŭ���� ��� ���̸� ����� �ߺ����� �������� �ʵ���.
            }
            audioSource.clip = clip;//������ҽ��� Ŭ�� ����.
            audioSource.loop = true;//���� ����
            audioSource.Play();
        }
    }
}
