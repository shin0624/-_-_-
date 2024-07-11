using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

// �� ��ȯ�� ���氪���� ������ ����ϴ� ��ũ��Ʈ. 1�� -> ���� -> 1������ �� ��ȯ ��, ���� �ٸ� ��ġ���� �����Ǿ�� �ϱ� ������ ������ �ʿ�
public class SceneManagerEX : MonoBehaviour
{
    public static SceneManagerEX SceneManagerInstance { get; private set; }//�� �Ŵ��� �ν��Ͻ��� ����.
    public static int SceneCount { get; private set; } = 0;//�ʱ�ȭ->SceneManagerEXŬ������ ó�� �ε�� ������ ���
    public  GameObject player;

    public Vector3 LastSpawnPosition = new Vector3(-16.9419994f, -2.5f, 25.1469994f);//���� ���� ��ġ
    public Vector3 FirstSpawnPosition = new Vector3(-57.2439995f, -2.38499999f, 16.757f);//ù ���� ��ġ

    public Vector3 UnderSpawnPosition = new Vector3(-303.083008f, -43.3740005f, -18.6930008f);//���Ͻ���������
    public Quaternion UnderSpawnRotation =   Quaternion.Euler(271.706604f, 20.3141384f, 161.09494f);//���Ͻ��������̼�
    public Vector3 UnderSpawnScale = new Vector3(0.35261631f, 0.683449626f, 0.422265708f);//���Ͻ���������

    public void Awake()//�ʱ�ȭ
    {
      
       if(SceneManagerInstance ==null)
        {
            SceneManagerInstance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;//�̺�Ʈ�ڵ鷯 ��� 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        if(SceneManagerInstance==this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;//�̺�Ʈ �ڵ鷯 ����
        }
    }
    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == "UndergroundScene")
        {
            player.transform.position = UnderSpawnPosition;
            Under_TimelineController.Under_TimelineController_Instance?.PlayTimeline();
        }

    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //1�� ���� �ε�� �� �� ī��Ʈ �� ���� �� ���� ��ġ ����
        if(scene.name== "1_2FloorScene")
        {
            SceneCount++;//��ī��Ʈ�� ���� �ε�� ������ ������ ��.
            Debug.Log($"scenecount : {SceneCount}");

            if(SceneCount==2)
            {
                player.transform.position = LastSpawnPosition;
                


            }
            else if(SceneCount==1)
            {
                player.transform.position = FirstSpawnPosition;
            }
        }
        else if(scene.name == "UndergroundScene")
        {
            
            player.transform.position = UnderSpawnPosition;
            Under_TimelineController.Under_TimelineController_Instance?.PlayTimeline();
        }
    }


}
