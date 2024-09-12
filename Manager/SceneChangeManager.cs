using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    // ���� �� -> ���� �� -> ���� ������ ���� ��ȯ�� ��, �ݺ��Ǵ� �������� ���� ��ġ�� ����. �� ī��Ʈ�� ���������Ͽ� ���� �����ϰ� �� ���� ���� �ݺ��Ǵ� �������� ������ġ�� �����Ѵ�.
    // ���� ��ġ�� �ϵ��ڵ��� �ƴ� spawnPoint��� �� ������Ʈ�� �������� ����. �����ϰ��� �ϴ� ��ġ�� �� ������Ʈ�� ���´�.
    // Managers Ŭ������ ����� SceneChangeManager Ÿ�� �̱��� �ν��Ͻ��� �����Ͽ� ���
    
    public static int SceneCount { get; private set; } = 0; // �� Ŭ������ ó�� �ε�� �� ���
    [SerializeField]
    private GameObject player;

    private Vector3 SecondFloorPlayer = new Vector3(0.3f, 0.4f, 0.4f);


    public void Awake()
    {
        //Managers Ŭ������ SceneChangeManager �ν��Ͻ� ����
        SceneChangeManager sceneChangeManager = Managers.SceneChange;

        //���� �ε�� �� ȣ��Ǵ� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)//�� ��ȯ �� �÷��̾� ���� ó��
    {
        if(scene.name== "New1_2FloorScene")//���� ���� ������ ��
        {
            SceneCount++;
            Debug.Log($"SceneCount : {SceneCount}");

            GameObject spawnPoint;//��������Ʈ ������Ʈ ã��
            if(SceneCount==1)//���� ù ����
            {
                spawnPoint = GameObject.Find("FirstSpawnPoint");
            }
            else//���� -> �������� ���ƿ� ��
            {
                spawnPoint = GameObject.Find("LastSpawnPoint");
            }
            if (spawnPoint != null)//��������Ʈ�� �����ϸ� �÷��̾� ��ġ �̵�
            {
                player.transform.position = spawnPoint.transform.position;
                player.transform.rotation = spawnPoint.transform.rotation;
                Debug.Log($"Spawn Point : {spawnPoint.name}");
            }
        }
        else if(scene.name== "NewUnderAfter")//���� -> ���Ϸ� �̵� ��
        {
            GameObject spawnPoint = GameObject.Find("SecondSpawnPoint");
            if(spawnPoint != null)
            {
                player.transform.position = spawnPoint.transform.position;
                player.transform.rotation = spawnPoint.transform.rotation;
                player.transform.localScale = SecondFloorPlayer;
                Debug.Log("Last Spawn Point");
             }

        }
    }
}
