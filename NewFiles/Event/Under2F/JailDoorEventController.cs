using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class JailDoorEventController : MonoBehaviour
{
    // ���� 2�� ������ �̺�Ʈ ��Ʈ�ѷ� : �÷��̾ ���� 2���� �����ϸ� ���� ������ ��� �������� ������ �̻������� �߻��Ѵ�.
    private GameObject player;
    private JailNeonLampsSound[] JNLSList;//��ũ��Ʈ�� �����ؾ� �ϴ� ������ �������̹Ƿ�, �迭Ÿ������ ����
    private JailDoorOpen[] JDOPENList;//��ũ��Ʈ�� �����ؾ� �ϴ� ����� ������ �̹Ƿ�, �迭 Ÿ������ ����

    private AudioSource Under2ndBGM;
    public bool isEnter = false;


    void Start()
    {
        //JNLS = FindObjectOfType<JailNeonLampsSound>();//��ũ��Ʈ�� �پ��ִ� �׿·����� ã�´�.
        //-->�Ǽ� : ���� ���� �����ϸ�, ��ũ��Ʈ���� ù ��°�� �߰ߵ� JNLS������Ʈ�� �����ϰ� �Ǿ�, ���� ���� �� ù��° �׿·����� �����Ÿ��� ��.
        //�� ������ �ذ��ϱ� ����, �׿� ������ ��� ã�� ������ �ø�Ŀ�� �޼��带 ȣ���ϵ��� �ؾ� ��.

        JNLSList = FindObjectsOfType<JailNeonLampsSound>();//��ũ��Ʈ�� ���� ��� �׿·����� ã�´�.
        JDOPENList = FindObjectsOfType<JailDoorOpen>();
        Under2ndBGM = GetComponent<AudioSource>();
        //Find Object Of Type�� �ƴ϶�, Find Objects Of Type�ӿ� ����
    }

    
    void Update()
    {
  
    }

    public void OnTriggerEnter(Collider other)
    {
        Under2ndBGM.Play();
       if(other.CompareTag("PLAYER"))//�÷��̾� �±װ� ���� ���ӿ�����Ʈ�� �浹 �� 
        {
            isEnter = true;
           
            foreach (JailNeonLampsSound JNLS in JNLSList)// ����Ʈ ���ڷ� ���� ��� �������� ������ ��ȯ�ϴ� foreach ���
            {
                JNLS.StartFlickering(0.2f);
            }   
            foreach(JailDoorOpen JDOPEN in JDOPENList)
            {
                JDOPEN.StartOpenDoorBust();       
            }
        }
    }

    // public void OnTriggerExit(Collider other)
    // {
    //     if(other.CompareTag("PLAYER"))
    //     {
    //     Under2ndBGM.Stop();
    //     isEnter = false;
           
    //         foreach (JailNeonLampsSound JNLS in JNLSList)// ����Ʈ ���ڷ� ���� ��� �������� ������ ��ȯ�ϴ� foreach ���
    //         {
    //             JNLS.StopFlickering();
    //         }   
    //     }

    // }
}
