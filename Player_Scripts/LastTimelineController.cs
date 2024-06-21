using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LastTimelineController : MonoBehaviour
{
    //2�� �⵵�� -> ���� ������ ��ȯ�Ǵ� ��ũ��Ʈ.

    public string targetSceneName = "EndScene";

    public VideoPlayer videoplayer; // �ִϸ��̼� ����� ���� ����
    private bool playerInRange = false;

    void Start()
    {
        if (videoplayer != null)
        {
            videoplayer.loopPointReached += OnVideoFinished;//�ִϸ��̼� ��� ���� �� �� ��ȯ �̺�Ʈ ����.
        }
    }

    void Update()
    {
        if(playerInRange)
        {
            PlayVideo();
            Debug.Log("LastAnimation Start!");
            playerInRange = false;//�ߺ� ��� ����.  
        }     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {

            Debug.Log("playerinrange = true");
            playerInRange = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            Debug.Log("Player exited the trigger.");
            playerInRange = false;
        }
    }

    private void PlayVideo()
    {
        if (videoplayer != null)
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
