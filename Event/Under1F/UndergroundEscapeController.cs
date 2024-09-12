using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundEscapeController : MonoBehaviour
{
    // ���������� -> ���� 1������ Ż���� ���� ��ũ��Ʈ

    private BoxCollider boxCollier;//�ڽ� �ݶ��̴��� Ʈ���Ÿ� �޾ƾ� �ϹǷ�.

    void Start()
    {
        boxCollier = GetComponent<BoxCollider>();
        boxCollier.isTrigger = true;//Ʈ����üũ
        if(boxCollier==null || !boxCollier.isTrigger)
        {
            Debug.Log("BoxCollider is Error! Please Check Collider`s Trigger or Component");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PLAYER"))
        {
            LoadingSceneManager.LoadScene("New1_2FloorScene");
        }
    }
}
