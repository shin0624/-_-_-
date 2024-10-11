using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class InteractEventPhonecall : Interactable
{
    private AudioSource audioSource;

    // ��ȭ �Ⱦ� �� ��� ����
    public AudioClip afterPickupPhone;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        // ������ ��� ���� ������� ����
        audioSource.Stop();

        // ���ο� Ŭ���� �����ϰ� ���
        audioSource.loop = false;
        audioSource.clip = afterPickupPhone;
        audioSource.Play();

        // ������Ʈ�� �±׸� 'Untagged'�� ����
        gameObject.tag = "Untagged";
    }
}
