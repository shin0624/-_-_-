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

    private Vector3 UnderFloorPlayer = new Vector3(0.3f, 0.4f, 0.4f);


    public void Awake()
    {
        //Managers Ŭ������ SceneChangeManager �ν��Ͻ� ����
        SceneChangeManager sceneChangeManager = Managers.SceneChange;
        //���� �ε�� �� ȣ��Ǵ� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)//�� ��ȯ �� �÷��̾� ���� ó��
    {
        SceneCount++;
        Debug.Log("SceneCount : " + SceneCount);
        GameObject SpawnPoint;

        if(scene.name=="New1_2FloorScene")
        {
            if(SceneCount==1)
            {
                SpawnPoint = GameObject.Find("FirstSpawnPoint");
            }
            else
            {
                SpawnPoint = GameObject.Find("LastSpawnPoint");  
            }
            SPSetting(SpawnPoint);//���� �� �÷��̾��� �����̼� �������� ������ҿ� �����.
        }
        if(scene.name=="NewUnder3F")
        {
            SpawnPoint  = GameObject.Find("SecondSpawnPoint");
            SPSettingUnder(SpawnPoint);//���ϴ� �÷��̾� ������ ������ �ʿ��ϹǷ� �޼��带 ���� �߰�
        }
        if(scene.name =="NewUnderAfter")
        {
            SpawnPoint  = GameObject.Find("ThirdSpawnPoint");
            SPSettingUnder(SpawnPoint);
        }
    }

    public void SPSettingUnder(GameObject sp)
    {
          if(sp != null)
            {
                player.transform.position = sp.transform.position;
                player.transform.rotation = sp.transform.rotation;
                player.transform.localScale = UnderFloorPlayer;
                Debug.Log($"Spawn Point : {sp.name}");
             }
    }
    public void SPSetting(GameObject sp)
    {
         if (sp != null)//��������Ʈ�� �����ϸ� �÷��̾� ��ġ �̵�
            {
                player.transform.position = sp.transform.position;
                player.transform.rotation = sp.transform.rotation;
                Debug.Log($"Spawn Point : {sp.name}");
            }
    }
}
