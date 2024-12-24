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
   
   public int GetScenecount = 0;

    protected override void OnStart()
    {
        GetScenecount = SceneChangeManager.SceneCount;
        SceneManager.sceneLoaded += OnSceneLoaded;//���� �ε�� �� ȣ��Ǵ� �̺�Ʈ �ڵ鷯  
    }

    protected override void OnUpdate()
    {
       
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;//�̺�Ʈ �ڵ鷯 ��� ����
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)//���� ��ȯ�� �� ���� ������ ��簡 ��µ� �� �ֵ��� �ϴ� �Լ�
    {

        if(scene.name=="New1_2FloorScene")
        {
            DisplayDialogueOnce("00002");
        }
        if (scene.name=="NewUnder3F")//1�� ��
        {
            DisplayDialogueOnce("11114");
        }
        if(scene.name=="NewUnderAfter")
        {
            DisplayDialogueOnce("11115");
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
        
        if(SceneManager.GetActiveScene().name == "New1_2FloorScene")//���� ������ ��µ� ���̾�α�
        {
            if (collision.gameObject.CompareTag ("SecureRoom"))
            {
               Debug.Log("dialogue !!");
                DisplayDialogueOnce("00003");
            }

            if (collision.gameObject.CompareTag("Shutter") && GetScenecount==0)
            { 
                Debug.Log("dialogue !!");
                DisplayDialogueOnce("10009");                
            }


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

    public void IntroSceneDialogue01()//��Ʈ�ξ��� ���̾�α�(Intro#1)_First
    {
        DisplayDialogueOnce("11111");
    }
    public void IntroSceneDialogue02()//��Ʈ�ξ��� ���̾�α�(Intro#1)_Second
    {
        DisplayDialogueOnce("11112");

    }
    public void MeetWithDoctorDialogue()//2�� ���� ���� �̺�Ʈ�� ���̾�α�
    {
        DisplayDialogueOnce("11113");
    }
    public void ShamanRoarDialogue()
    {
        DisplayDialogueOnce("11116");
    }
    public void ShamanRunDialogue()
    {
        DisplayDialogueOnce("11117");
    }
    public void DoctorRunDialogue()
    {
        DisplayDialogueOnce("11118");
    }
    public void EventPhonCalling()
    {
        DisplayDialogueOnce("11119");
    }


}
