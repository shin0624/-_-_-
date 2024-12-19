using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveDisplayManager : ObjectiveController
{
    //���� �󿡼� �߻��ϴ� ��� ��ǥ(Objective)����� ������ ��ũ��Ʈ
    //DialogueDisplayManger�� ������ ������ �ۼ���.
    private Dictionary<string, bool> ObjectDisplay = new Dictionary<string, bool>();//��ǥ�� �ߺ� ��µǴ� ���� �����ϱ� ���� ��ǥ��ȣ(���� + a)�� ��� ���θ� ��ųʸ��� ����

    protected override void OnStart()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;//���� �ε�� �� ȣ��Ǵ� �̺�Ʈ �ڵ鷯 
    }

    protected override void OnUpdate()
    {
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
        if(!ObjectDisplay.ContainsKey(newObjectiveID) || !ObjectDisplay[newObjectiveID])//��µ� ��ǥ�� KEY��, ��¿��θ� VALUE�� ��ųʸ��� �����ϱ� ������, �ѹ� ��µ� ��ǥ�� ������� ������ �� �ִ�.
        {
            DisplayObjective(newObjectiveID, objectiveID);
            ObjectDisplay[newObjectiveID] = true;
            // ��� ������ VALUE�� FALSE�� ���� ����.
        }
    }

    private void OnCollisionEnter(Collision collision)//Ư�� ���, Ư�� ������ �� �浹 �� ��Ÿ���� ���̾�α� ǥ��
    {

        if (SceneManager.GetActiveScene().name == "1_2FloorScene")//���� ������ ��µ� ���̾�α�
        {
            if (collision.gameObject.CompareTag("1F_Start"))
            {
                ExploreFirstFloor()//���� Ž���ϱ�
            }
            else if (collision.gameObject.CompareTag(""))
            {
                FindToolForCrackedWall()//�ݰ� ���� �μ� �� �ִ� ������ ã��
            }
            else if (collision.gameObject.CompareTag(""))
            {
                FindSecurityRoom();//�ܼ��� ���� ���Ƚ� ��й�ȣ �˾Ƴ���
            }
            else if (collision.gameObject.CompareTag("2F_Start"))
            {
                ExploreSecondFloor();//2�� Ž���ϱ�
            }
            else if (collision.gameObject.CompareTag(""))
            {
                FindPathToLightRoom()//�� ���� ������ ���ϴ� �� ã��
            }
        }
        else if (SceneManager.GetActiveScene().name == "")//���� ������ ��µ� ���̾�α�
        {
            if (collision.gameObject.CompareTag(""))
            {
                ExploreBasement()//���� Ž���ϱ�
            }
            else if (collision.gameObject.CompareTag(""))
            {
                Escape();//Ż���� ��� ã��
            }
        }
    }

    void ExploreFirstFloor()//���� Ž���ϱ�
    {
        DisplayObjectiveOnce("001a", "001b");
    }

    void FindToolForCrackedWall()//�ݰ� ���� �μ� �� �ִ� ������ ã��
    {
        DisplayObjectiveOnce("002a", "002b");
    }

    void FindSecurityRoom()//�ܼ��� ���� ���Ƚ� ��й�ȣ �˾Ƴ���
    {
        DisplayObjectiveOnce("003a", "003b");
    }

    void ExploreSecondFloor()//2�� Ž���ϱ�
    {
        DisplayObjectiveOnce("004a", "004b");
    }

    void FindPathToLightRoom()//�� ���� ������ ���ϴ� �� ã��
    {
        DisplayObjectiveOnce("005a", "005b");
    }

    void ExploreBasement()//���� Ž���ϱ�
    {
        DisplayObjectiveOnce("101a", "101b");
    }

    void Escape()//Ż���� ��� ã��
    {
        DisplayObjectiveOnce("999a", "999b");
    }
}
