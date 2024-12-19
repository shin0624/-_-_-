using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ���� --> ���� �� �̵� ��ũ��Ʈ
public class TeleportSceneManager : MonoBehaviour
{
    public string targetSceneName = "1_2FloorScene"; //���� �� �̸�

    private bool playerInRange = false;//�÷��̾ ���� ���� �ִ��� ����

    private float holdTime = 0f;// E ��ư�� ������ �ִ� �ð�

    public float requireHoldTime = 3f;//��ư�� ������ �ϴ� �ּ� �ð�

    //�÷��̾ �� ��ó�� ���� ��ȣ�ۿ� ��ư�� 3�ʰ� Ŭ���϶�� ������ �Բ� ���� �ð��� �������� ǥ�õ�.
    public Text holdMessage; //���� �޽���

    //Ű �̺�Ʈ �߰�
    public string requiredItemName = "Under_Key";//�ʿ��� ���� ������ �̸�
    private Inventory playerInventory;//�÷��̾� �κ��丮�� �ҷ��´�.
    private DialogueDisplayManager dialogueDisplayManager;

    //public Vector3 newSpawnPosition = new Vector3(-16.6749992f, -2.4059999f, 23f);//���ο� ���� ��ġ

    void Start()
    {
        //���� �� ui��� ��Ȱ��ȭ.
        holdMessage.gameObject.SetActive(false);
      

        dialogueDisplayManager = FindObjectOfType<DialogueDisplayManager>();
    }



    void Update()
    {
        try
        {
            if(playerInRange)
            {
                holdMessage.gameObject.SetActive(true);
               

                if(Input.GetKey(KeyCode.E))
                {
                    holdTime += Time.deltaTime; //�÷��̾ ���� ���� ���� �� ��ȣ�ۿ� ��ư�� ������ holdtime�� ������.
                   

                    if(holdTime >= requireHoldTime)//holdtime�� requireHoldTime�� �ʰ��ϸ� ���� ������ �̵�.
                    {
                        //Ű�� ������ �ִٸ� ���� ���� ���� ������ �̵��� �� �ִ�.
                        if(playerInventory!=null && playerInventory.HasItem(requiredItemName))
                        {
                            dialogueDisplayManager.PlayerUsingKey();//Ű�� ������ �ִٴ� ���̾�α� ���.
                            SceneManager.LoadScene(targetSceneName);
                           
                           
                        }
                        else
                        {
                            dialogueDisplayManager.PlayerNotPickUpKey();//Ű�� ������ ���� �ʴٴ� ���̾�α� ���.
                        }
                    }
                }
                else
                {
                    holdTime = 0f;//��ȣ�ۿ� ��ư�� ������ ������ holdtime�� 0���� �ʱ�ȭ.
                    
                }
            }
            else
            {
                holdMessage.gameObject.SetActive(false);
            
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
            playerInventory = other.GetComponent<Inventory>();//�÷��̾ �����ϸ� �κ��丮�� �޾ƿ´�. ���⼭ Ű ���� üũ�� �̷���� ��.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("PLAYER"))
        {
            playerInRange= false;//�÷��̾ ���� ����� playerInRange�� false�� �ǰ� holdtime�� �ʱ�ȭ��.
            holdTime = 0f;

        }
    }

}
