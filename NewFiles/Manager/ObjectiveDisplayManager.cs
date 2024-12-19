using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveDisplayManager : ObjectiveController
{
    //���� �󿡼� �߻��ϴ� ��� ��ǥ(Objective)����� ������ ��ũ��Ʈ
    //DialogueDisplayManger�� ������ ������ �ۼ���.

    private Dictionary<string, bool> ObjectDisplay = new Dictionary<string, bool>();//��ǥ�� �ߺ� ��µǴ� ���� �����ϱ� ���� ��ǥ��ȣ(���� + a)�� ��� ���θ� ��ųʸ��� ����

    public int SceneCountStatic ;//��ī��Ʈ�� �����´�.



    protected override void OnStart()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;//���� �ε�� �� ȣ��Ǵ� �̺�Ʈ �ڵ鷯 
    }

    protected override void OnUpdate()
    {

    }
    private void Update()
    {
        SceneCountStatic = SceneChangeManager.SceneCount;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;//�̺�Ʈ�ڵ鷯 ��� ����
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)//���� ��ȯ�� �� ���� ������ ��ǥ�� ��µ� �� �ֵ��� �ϴ� �Լ�
    {
    

    }

    public void DisplayObjectiveOnce(string newObjectiveID, string objectiveID)//���� ��ǥ�� �ѹ��� ��µ� �� �ֵ��� �ϴ� �޼���.�߻�Ŭ������ DisplayObjective�� ����Ͽ� ��ǥ�� ����ϰ�, �׶��׶� ��µ� ��ǥ TF���� VALUE�� �Ѵ�
    {
        if (!ObjectDisplay.ContainsKey(newObjectiveID) || !ObjectDisplay[newObjectiveID])//��µ� ��ǥ�� KEY��, ��¿��θ� VALUE�� ��ųʸ��� �����ϱ� ������, �ѹ� ��µ� ��ǥ�� ������� ������ �� �ִ�.
        {
            DisplayObjective(newObjectiveID, objectiveID);
            ObjectDisplay[newObjectiveID] = true;
            // ��� ������ VALUE�� FALSE�� ���� ����.
        }
    }



    // Ʈ���� �浹 ó��
    private void OnTriggerEnter(Collider other)
    {
        if (SceneManager.GetActiveScene().name == "New1_2FloorScene") // Ư�� �������� ����
        {
            if (other.CompareTag("1F_Start"))
            {
                FindSecurityRoom();
            }
            else if (other.CompareTag("2F_Start"))
            {
                FindSecondFloor();
            }
            else if (other.CompareTag("ClearTrigger") && SceneCountStatic >=1)
            {
                GameClearObjective();
            }
        }
        else if (SceneManager.GetActiveScene().name == "NewUnderAfter")
        {
            if (other.CompareTag("Under3F_Start"))
            {
                SpawnInChapel();
            }
            else if (other.CompareTag("Under2ndFloorTrigger"))
            {
                EscapeUnderground();
            }
            else if(other.CompareTag("Under2F_Start"))
            {
                AfterElavator();
            }
        }
        else if(SceneManager.GetActiveScene().name == "NewUnder3F")
        {
            if(other.CompareTag("Under3F_Start"))
            {
                ShamanRoomEnter();
            }
        }
    }



    void FindSecurityRoom()//�ܼ��� ���� ���Ƚ� ��й�ȣ �˾Ƴ���
    {
        Debug.Log("Now Objective");
        DisplayObjectiveOnce("001a", "001b");
       
    }

    void FindSecondFloor()//2�� Ž���ϱ�
    {
        DisplayObjectiveOnce("002a", "002b");
    
    }

    void GameClearObjective()//���� Ŭ����
    {
        DisplayObjectiveOnce("003a", "003b");
    }

    void SpawnInChapel()
    {
        DisplayObjectiveOnce("004a", "004b");
    }

    void EscapeUnderground()
    {
        DisplayObjectiveOnce("005a", "005b");
    }
    
    void AfterElavator()
    {
        DisplayObjectiveOnce("006a", "006b");
    }
    
    void ShamanRoomEnter()
    {
        DisplayObjectiveOnce("007a","007b");
    }
}
