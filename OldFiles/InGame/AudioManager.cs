/*
 * AudioManager.cs
 * �� ��ũ��Ʈ�� ���� ������ �÷��̾� ������� ����
 * �� ���� ������ AudioSource�� ���� �ܹ߼� �� ���� ��� ���带 ó��
 */
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // AudioSource ������Ʈ
    private AudioSource loopAudioSource;
    private AudioSource oneShotAudioSource;

    void Start()
    {
        // AudioSource ������Ʈ�� ��������
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length >= 2)
        {
            // ù ��° AudioSource�� ���� ��������� ����
            loopAudioSource = audioSources[0];
            // �� ��° AudioSource�� �ܹ߼� ��������� ����
            oneShotAudioSource = audioSources[1];
        }
    }

    // �ܹ߼� ���带 ����ϴ� �޼���
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

    // ���� ��� ���� ���� ���带 �����ϴ� �޼���
    public void StopSoundLoop()
    {
        if (loopAudioSource != null)
        {
            loopAudioSource.Stop();
            loopAudioSource.loop = false;
        }
    }
}
