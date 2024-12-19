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
        if(other.CompareTag("PLAYER") && !IsOpen )
        {
            OpenDoor();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //�÷��̾ Ʈ���� ���� ���� �ִ� ���� ���� ������ �ʵ��� ��.
        if(other.CompareTag("PLAYER"))
        {
            IsOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if(other.CompareTag("PLAYER") && IsOpen )
        //{
            CloseDoor();
       // }

    }

    private void OpenDoor()
    {
       
        DoorAnimator.SetTrigger(OpenTriggerName);
        ado.Play();
        IsOpen = true;

    }

    private void CloseDoor()
    {
     
        DoorAnimator.SetTrigger(CloseStringName);
        ado.Play();
        IsOpen = false;

    }




}
