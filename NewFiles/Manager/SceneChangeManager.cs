using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    
    private Vector3 UnderFloorPlayer = new Vector3(0.4f, 0.7f, 0.4f);
    private Vector3 Under3FPlayer = new Vector3(1.0f, 1.6f, 0.9f);
    //private quaternion Under3FPlayerRotation = Quaternion.Euler(-90.0f,0f,90f );
    public static int SceneCount = 0;
    public int GetCount {get {return SceneCount;}}
    private void Awake()
    {
        SceneCount = PlayerPrefs.GetInt("SceneCount", 0);// ���� �� ī��Ʈ ���� �ε�. ������ �⺻�� 0.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) // �� �̵� ���� : New1_2FloorScene -> NewUnder3F -> NewUnderAfter -> New1_2FloorScene -> ����
    {
        GameObject spawnPoint = null;

        // ���� ���� ���� ���� ����
        if(scene.name == "New1_2FloorScene")//���� ���� �� ���� ��. ���� ���� �ι� �ݺ��ϸ鼭 �� ���� ���� �ٸ� ��Ȳ�� ����Ǿ�� ��
        {
            spawnPoint = GetSavedSpawnPoint("New1_2FloorScene", "FirstSpawnPoint", "LastSpawnPoint");
            SetSpawnPosition(spawnPoint);
        }
        else if(scene.name == "NewUnder3F")//���� 3��(������)���� ��
        {
            spawnPoint = GameObject.Find("SecondSpawnPoint");
            SetSpawnPositionWithScaleUnder3F(spawnPoint);
        }
        else if(scene.name == "NewUnderAfter")//���� 2��(��������, ���ź���, ������) ���� ��
        {
            spawnPoint = GameObject.Find("ThirdSpawnPoint");
            SetSpawnPositionWithScaleUnder2F(spawnPoint);
            IncrementSceneCount();//���Ϸ� ���� �� �� ī��Ʈ�� �������ѳ��´�.
        }
    }

    private GameObject GetSavedSpawnPoint(string sceneName, string firstSpawnPointName, string lastSpawnPointName)
    {
        //SceneCount ���� ���� �ٸ� ���� ����Ʈ�� ����
        string savedPointName = (SceneCount == 0) ? firstSpawnPointName : lastSpawnPointName;//SceneCount�� 0�̸�(������ ó�� ���۵ǰ� ó�� 1������ ���� ��) firstSpawnPoint�� ����, �� ��찡 �ƴϸ� LastSpawnPoint�� ������.
         Debug.Log($"SceneCount: {SceneCount}, Saved Spawn Point: {savedPointName}");
         return GameObject.Find(savedPointName);
    }

    // ���� 2��, ���� 3���� �� ũ�Ⱑ �ٸ��� ������, ������ �÷��̾��� �������� ���� �ٸ��� �ؾ� �Ѵ�.
    public void SetSpawnPositionWithScaleUnder2F(GameObject sp)//NewUnderAfter(����2��)���� ������ ��
    {
        if(sp != null)
        {
            player.transform.position = sp.transform.position;
            player.transform.rotation = sp.transform.rotation;
            player.transform.localScale = UnderFloorPlayer;
            Debug.Log($"Spawn Point : {sp.name}");
            SaveSpawnPoint(sp); // ���� ��ġ ����
        }
    }
    public void SetSpawnPositionWithScaleUnder3F(GameObject sp)//NewUnder3F(����3��)���� ������ ��
    {
        if(sp != null)
        {
            player.transform.position = sp.transform.position;
            //player.transform.rotation = Under3FPlayerRotation;
            player.transform.rotation = sp.transform.rotation;
            player.transform.localScale = Under3FPlayer;
            Debug.Log($"Spawn Point : {sp.name}");
            SaveSpawnPoint(sp); // ���� ��ġ ����
        }
    }
    public void SetSpawnPosition(GameObject sp)//New1_2Floor(���� ���� ��)���� ������ ��
    {
        if (sp != null)
        {
            player.transform.position = sp.transform.position;
            player.transform.rotation = sp.transform.rotation;
            Debug.Log($"Spawn Point : {sp.name}");
            SaveSpawnPoint(sp); // ���� ��ġ ����
        }
    }

    private void SaveSpawnPoint(GameObject spawnPoint)// �� �̸��� ���������� PlayerPrefs�� �����Ѵ�. 
    {
        // ���� �� �̸��� ���� ���� �̸��� ����
        string sceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString($"{sceneName}_SpawnPoint", spawnPoint.name);
        PlayerPrefs.SetInt("SceneCount", SceneCount);
        PlayerPrefs.Save();
    }

    public void IncrementSceneCount()//��ī��Ʈ ���� �޼���. 
    {
        SceneCount++;
        PlayerPrefs.SetInt("SceneCount", SceneCount);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit() // ���� ���� �� �� ī��Ʈ �� �ʱ�ȭ. PlayerPrefs���� ���� ���� �Ŀ��� ������Ʈ���� �����ֱ⿡, �ʱ�ȭ �ʿ�. ��� ���� ���� �߿��� ���� ������ ��.
    {
        PlayerPrefs.DeleteKey("SceneCount");
        PlayerPrefs.Save();    
    }
}
