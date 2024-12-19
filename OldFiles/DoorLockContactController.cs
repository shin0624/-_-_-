using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorLockContactController : MonoBehaviour
{
    public string DoorLockPrefabPath = "PREFABS_MAKESELF/DoorLock/DoorLockCanvas";//ui ������ ���
    private GameObject DoorLockUIInstance;// ui ������ �ν��Ͻ�
    

    void Start()
    {
        GameObject DoorLockPrefab = Resources.Load<GameObject>(DoorLockPrefabPath);//���� ���� �� ������ ��ο��� �������� �ε�

        if (DoorLockPrefab==null)//��üũ
        {
            Debug.LogError("DoorLockCanvas is NULL.");
            return;
        }
        
        DoorLockUIInstance = Instantiate(DoorLockPrefab);//������ �ν��Ͻ� ���� �� ��Ȱ��ȭ
        DoorLockUIInstance.SetActive(false);//�ʱ⿡�� ��Ȱ��ȭ

        DoorLockController.OnDoorLockClosed += DeactivateDoorLockUI;//DoorLockController�� �̺�Ʈ�� ����--> ESCŰ�� ������ �� UI�� ��Ȱ��ȭ ó��
    }
    private void OnDestroy()
    {
        DoorLockController.OnDoorLockClosed -= DeactivateDoorLockUI;//�̺�Ʈ ���� ����
    }

    private void OnTriggerStay(Collider other)//�浹���� + EŰ�� ������ ui�� Ȱ��ȭ�Ǿ�� �ϹǷ�, �������� Ű �Է� ������ �ʿ�.
                                              //OnCollisionEnter�� �浹 ������ ȣ��Ǳ� ������ ������
    {
        if(other.gameObject.CompareTag("PLAYER"))//�÷��̾ ������ �� ��
        {
            if (Input.GetKeyDown(KeyCode.E)) // ��ȣ�ۿ� ��ư�� ������ Ȱ��ȭ
            {
                if(DoorLockUIInstance!=null)//ui�ν��Ͻ��� �����ϴ� ���
                {
                    ActivateDoorLockUI();
                }
                else//ui�ν��Ͻ��� ���� ���. Ȥ�� �̹� ��й�ȣ�� ������ �� �ٽ� ������Ʈ���� �ٰ��ͼ� eŰ�� ������ ���� ������ ����.
                {
                    Debug.Log("You Have Already Unlocked Password.");
                }
                
            }

            
        }
    }

    private void ActivateDoorLockUI()// UI Ȱ��ȭ �޼���
    {
        //Time.timeScale = 0f;//���ӽð� �Ͻ� ����
        DoorLockUIInstance.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        

    }
    private void DeactivateDoorLockUI()
    {
        //Time.timeScale = 1f;//���� �簳
        DoorLockUIInstance.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
