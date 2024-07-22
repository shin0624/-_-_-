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

                FindSecurityRoom();

            }
        }
    }


    void FindSecurityRoom()//�ܼ��� ���� ���Ƚ� ��й�ȣ �˾Ƴ���
    {
        DisplayObjectiveOnce("001a", "001b");
    }

}
