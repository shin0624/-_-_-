using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeleportSceneManager : MonoBehaviour
{
    public string targetSceneName = "1_2FloorScene"; //���� �� �̸�
    private bool playerInRange = false;//�÷��̾ ���� ���� �ִ��� ����
    private float holdTime = 0f;// E ��ư�� ������ �ִ� �ð�
    public float requireHoldTime = 3F;//��ư�� ������ �ϴ� �ּ� �ð�

    //�÷��̾ �� ��ó�� ���� ��ȣ�ۿ� ��ư�� 3�ʰ� Ŭ���϶�� ������ �Բ� ���� �ð��� �������� ǥ�õ�.
    public Text holdMessage; //���� �޽���
    public Slider holdProgressBar;// ��ư ���� �ð��� ���� �ö󰡴� ������

    void Start()
    {
        //���� �� ui��� ��Ȱ��ȭ.
        holdMessage.gameObject.SetActive(false);
        holdProgressBar.gameObject.SetActive(false);
    }



    void Update()
    {
        try
        {
            if(playerInRange)
            {
                holdMessage.gameObject.SetActive(true);
                holdProgressBar.gameObject.SetActive(true);

                if(Input.GetKey(KeyCode.E))
                {
                    holdTime += Time.deltaTime; //�÷��̾ ���� ���� ���� �� ��ȣ�ۿ� ��ư�� ������ holdtime�� ������.
                    holdProgressBar.value = holdTime / requireHoldTime;

                    if(holdTime >= requireHoldTime)//holdtime�� requireHoldTime�� �ʰ��ϸ� ���� ������ �̵�.
                    {
                        SceneManager.LoadScene(targetSceneName);
                        Debug.Log("Open the Door");
                    }
                }
                else
                {
                    holdTime = 0f;//��ȣ�ۿ� ��ư�� ������ ������ holdtime�� 0���� �ʱ�ȭ.
                    holdProgressBar.value = 0f;
                }
            }
            else
            {
                holdMessage.gameObject.SetActive(false);
                holdProgressBar.gameObject.SetActive(false);
            }
        }
        catch(SystemException e)
        {
            Debug.LogError($"Error In LastDoor : {e.Message}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PLAYER"))
        {
            playerInRange = true;//�÷��̾ �� ��ó�� �����ϸ� playerInRange�� true�� ������.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("PLAYER"))
        {
            playerInRange= false;//�÷��̾ ���� ����� playerInRange�� false�� �ǰ� holdtime�� �ʱ�ȭ��.
            holdTime = 0f;
            holdProgressBar.value = 0f;
        }
    }

}
