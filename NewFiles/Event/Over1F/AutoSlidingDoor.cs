using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AutoSlidingDoor : MonoBehaviour
{
    // ���� �����̵� ��� ����� ��ũ��Ʈ
    // �÷��̾ ������ �ٰ����� ���� ������, 3�� �� ������.
   
    [SerializeField]
    private Animator DoorAnimator;//���� ���� �ݴ� �ִϸ�����
    [SerializeField]
    private string OpenTriggerName = "SlidingDoorOpen";//���� �̸����� ������ Ʈ����
    [SerializeField]
    private string CloseStringName = "SlidingDoorClose";
    [SerializeField]
    private AudioSource ado;

    private bool IsOpen = false;//���� ���ȴ��� ����
    private Coroutine doorCoroutine;
    


    void Start()
    {
        if (DoorAnimator == null)
        {
            DoorAnimator = gameObject.GetComponent<Animator>();
        }

        if (ado == null)
        {
            ado = gameObject.GetComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)// ���� �̹� �����ִ� ��� OpenDoor()�� ȣ������ �ʾƾ� ��.
    {
        if(other.CompareTag("PLAYER")|| other.CompareTag("Player") && !IsOpen )
        {
            OpenDoor();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //�÷��̾ Ʈ���� ���� ���� �ִ� ���� ���� ������ �ʵ��� ��.
        if(other.CompareTag("PLAYER")|| other.CompareTag("Player"))
        {
            IsOpen = true;
            if(doorCoroutine!=null) 
                StopCoroutine(doorCoroutine); 
                doorCoroutine = null;//���ݱ� �ڷ�ƾ �������̶�� �ߴ�
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("PLAYER") || other.CompareTag("Player") &&IsOpen && doorCoroutine==null)
        {
            doorCoroutine = StartCoroutine(CloseDoorWithDelay(2.0f));
        }

    }

    private void OpenDoor()
    {
       
        DoorAnimator.SetTrigger(OpenTriggerName);
        ado.Play();
        IsOpen = true;

    }

    private IEnumerator CloseDoorWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //�÷��̾ �ٽ� ������ �ʾҴٸ� ���� �ݴ´�.
        if(!IsOpen)
        {
            yield break;//���� �̹� �����ֵ��� �ڷ�ƾ ����
        }
            DoorAnimator.SetTrigger(CloseStringName);
            ado.Play();
            IsOpen=false;
            doorCoroutine = null;
    }




}
