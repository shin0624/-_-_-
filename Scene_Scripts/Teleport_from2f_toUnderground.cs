using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Teleport_from2f_toUnderground : MonoBehaviour
{
    //���� �⵵�� -> ���� ����� �̵� ��ũ��Ʈ (����->1�� �̵� ��ũ��Ʈ�� ��ư �̺�Ʈ�� �����Ƿ� �� ��ũ��Ʈ�� ���� ����)

    public string targetSceneName = "UndergroundScene";

    private bool playerInRange = false;//�÷��̾ �� ������ �ִ��� ����

    private float holdTime = 0f;//�÷��̾ �� ��ó���� ����ϴ� �ð�

    public float requireHoldtime = 2f;//�÷��̾ �⵵�� ��(�� �ö��̴��� �´��� ����)�� 3�� �־��ٸ� ���� ������ �̵�(���� �ƽ� �߰� ����)

    public VideoPlayer videoplayer; // �ִϸ��̼� ����� ���� ����
   
    void Start()
    {
        if (videoplayer != null)
        {
            videoplayer.loopPointReached += OnVideoFinished;//�ִϸ��̼� ��� ���� �� �� ��ȯ �̺�Ʈ ����.
        }
    }

    void Update()
    {
            if (playerInRange)//�÷�� ������ �ִٸ�
            {
                    holdTime += Time.deltaTime;// �ð��� �ö󰡸� ���� �ð��� �������� �� ���Ϸ� �̵� --> ���������� ��� ��� �ƽ� ����
                    if (holdTime >= requireHoldtime)
                    {
                        PlayVideo();
                        Debug.Log("TimeLineAnimation Start!");
                        playerInRange= false;//�ߺ� ��� ����.            
                    }
            }
            else
            {
                    holdTime = 0f;//�÷��̾ �������� ����� �ٽ� �ʱ�ȭ
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            Debug.Log("playerinrange = true");
            playerInRange = true;//�÷��̾ �� ��ó�� �����ϸ� true

        }
  
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            Debug.Log("Player exited the trigger.");
            playerInRange = false;
            holdTime = 0f;
        }
    }

   private void PlayVideo()
    {
        if(videoplayer != null)
        {
            videoplayer.Play();//�ִϸ��̼� ���
        }
        else
        {
            Debug.LogWarning("videoPlayer is not assigned");
        }
    }
    private void OnVideoFinished(VideoPlayer vp)//�ִϸ��̼� ���� �� �� ��ȯ.
    {
        Debug.Log("Video Finished. change Scene");
        SceneManager.LoadScene(targetSceneName);
    }

}
