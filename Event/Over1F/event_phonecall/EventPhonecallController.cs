using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPhonecallController : MonoBehaviour
{
    // EventPhonecall�� ����
    public EventPhonecall eventPhonecall;

    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))// �÷��̾� �±װ� ���� ���ӿ�����Ʈ�� �浹 �� 
        {
            eventPhonecall.PlayPhonecall();
        }
    }
}
