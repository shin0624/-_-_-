using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPhonecall : MonoBehaviour
{
    private AudioSource audioSource;

    // ��ȭ�� ����
    public AudioClip phonecall;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = phonecall;
    }

    public void PlayPhonecall()
    {
        // ������Ʈ�� �±׸� 'Interactable'�� ����
        gameObject.tag = "Interactable";

        audioSource.Play();
    }
}
