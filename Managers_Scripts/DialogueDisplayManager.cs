using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

//���� �󿡼� �߻��ϴ� ��� ���̾�α� ����� ������ ��ũ��Ʈ. �÷��̾� �׼�, Ʈ���� �̺�Ʈ ���� �޾� �׶� �׶� ������ ��縦 ����Ѵ�.
public class DialogueDisplayManager : DialogController
{
    private Dictionary<string, bool> displayDialogues = new Dictionary<string, bool>();//���̾�αװ� �ߺ� ��µǴ� ���� �����ϱ�����, ���̾�α� ǥ�ÿ��ο� ���̾�α� ��ȣ���� ��ųʸ��� �����Ѵ�.
    public AutoShutterScript autoShutter;//1���� ���� ���ٿ� �̺�Ʈ ���̾�α� ��¿�
    private DFSEnemyAI enemyAI;//�������� ���ʹ� �̺�Ʈ ���̾�α� ��¿�

    

    protected override void OnStart()
    {
        enemyAI = FindObjectOfType<DFSEnemyAI>();//���ʹ�ai ��ũ��Ʈ ������ ��´�. 
    }

    protected override void OnUpdate()
    {
       
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name== "UndergroundScene")
        {
            CanSeeEnemy();
        }
    }

    public void DisplayDialogueOnce(string dialogueID)//���̾�αװ� �ѹ��� ��µ� �� �ֵ��� �ϴ� �޼���. �߻�Ŭ������ startdialouge�� ����Ͽ� ���̾�α׸� ����ϰ�, �׶��׶� ��µ� ���̾�α� TF���� VALUE�� �Ѵ�. 
    {
        if(!displayDialogues.ContainsKey(dialogueID) || !displayDialogues[dialogueID]) //��µ� ���̾�α׸� KEY��, ��¿��θ� VALUE�� ��ųʸ��� �����ϱ� ������, �ѹ� ��µ� ���̾�α��� ������� ������ �� �ִ�.
        {
            //���̾�α� ��� ������ VALUE�� FALSE�� ���� ����.
            StartDialogue(dialogueID);
            displayDialogues[dialogueID] = true;
        }
    }    


    private void OnCollisionEnter(Collision collision)//Ư�� ���, Ư�� ������ �� �浹 �� ��Ÿ���� ���̾�α� ǥ��
    {
        
        if(SceneManager.GetActiveScene().name == "1_2FloorScene")//���� ������ ��µ� ���̾�α�
        {
            if (collision.gameObject.CompareTag("1F_Start"))
            {
               
                DisplayDialogueOnce("00002");
            }

            if (collision.gameObject.CompareTag ("SecureRoom"))
            {
               
                DisplayDialogueOnce("00003");
            }

            if (collision.gameObject.CompareTag("Shutter"))
            { 
                DisplayDialogueOnce("10009");
            }

            if (autoShutter.RaiseShutter == true)
            {
                DisplayDialogueOnce("10010");
            }
        }
        else if(SceneManager.GetActiveScene().name == "UndergroundScene")//���� ������ ��µ� ���̾�α�
        {
            if (collision.gameObject.CompareTag("coffin"))
            {
                Debug.Log("Spawn on the coffin");
                DisplayDialogueOnce("00005");
            }
            Inventory inventory = collision.gameObject.GetComponent<Inventory>();
            if(inventory != null)
            {
                PlayerPickUpKey(inventory);//�÷��̾ Ű�� �����ϸ� ������ ���̾�α�
            }
            
            
            
        }
 
    }

    void CanSeeEnemy()
    {
        if(enemyAI!=null)
        {
            Define.EnemyState currentState = enemyAI.GetState();//���ʹ��� ���¸� �����´�.
            switch(currentState)
            {
                case Define.EnemyState.WALKING://Ž������ ���ʹ̸� �߰����� ��
                    DisplayDialogueOnce("00006");
                    break;
                case Define.EnemyState.RUNNING:
                    DisplayDialogueOnce("00007");
                    break;
            }
        }
    }

    void PlayerPickUpKey(Inventory playerInventory)
    {
        if(playerInventory.HasItem("Under_Key"))
        {
            DisplayDialogueOnce("10008");
        }
    }

    public void PlayerUsingKey()
    {
        DisplayDialogueOnce("00009");
    }

    public void PlayerNotPickUpKey()
    {
        DisplayDialogueOnce("00008");
    }
    public void SecondSpawn()
    {
        DisplayDialogueOnce("10011");
    }
}
