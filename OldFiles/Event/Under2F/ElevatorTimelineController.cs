using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ElevatorTimelineController : MonoBehaviour
{
    //���������� �ƽ� ��� �� y�� �� ������ ���������� �۵��� �����ϴ� ��ũ��Ʈ. elevator ������Ʈ�� ������
    //Ÿ�Ӷ��� ���(���������� �ö󰡴� �ƽ�) -> �÷��̾�� ������ Wrap Mode�� Ȱ��ȭ�Ͽ�, �ƽ� ���� �Ŀ��� ������ Ű���� ������� ���ư��� �ʰ� �� -> �÷��̾ Ÿ�� �ö󰡾� �ϹǷ�, ������ٵ� �߰� �� �ƽ� ���� �� ������ ����

    [SerializeField]
    private PlayableDirector timeline;
    [SerializeField]
    private Camera cam2;// �ó׸ӽ� ����ī�޶� �Ҵ��ϰ� �����Ͽ�, Ÿ�Ӷ����� ����� �Ŀ� ����ī�޶� ��Ȱ��ȭ.

    private Rigidbody rb; // Ÿ�Ӷ��� ���� �Ŀ��� ������������ �������� �����Ǿ�� �ϹǷ�,
   
    private void Awake()
    {
        timeline = GetComponent<PlayableDirector>();
        rb = gameObject.GetComponent<Rigidbody>();

        if(timeline==null)
        {
            Debug.Log(" Elevator Event Timeline is null !");
        }
        if(rb==null)
        {
            Debug.Log(" Elevator RigidBody is null !");
        }
    }

    private void OnEnable()
    {
        timeline.stopped += OnTimelineFinished;
        

    }

    private void OnDisable()// �̺�Ʈ ���� ����
    {
        timeline.stopped -= OnTimelineFinished;//Ÿ�Ӷ��� ���� �̺�Ʈ �ڵ鷯 �߰�
    }

    private void OnTriggerEnter(Collider other)//�÷��̾ ���������� �ݶ��̴��� ���� �� Ȱ��ȭ 
    {
        if(other.gameObject.CompareTag("PLAYER"))
        {
            Debug.Log("Player In HERE!");
            TimelinePlay();//Ÿ�Ӷ��� ���

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PLAYER"))
        {
            Debug.Log("Player left the elevator area.");
        }
    }


    private void TimelinePlay()// Ÿ�Ӷ��� �÷��� ����
    {
        timeline.Play();
        Debug.Log("elevator timeline start");
    }

    private void OnTimelineFinished(PlayableDirector director)//Ÿ�Ӷ��� ���� �� ������������ ������ ����
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY;

        if(cam2 != null)
        {
            cam2.gameObject.SetActive(false);//�ƽ��� ������ ����ī�޶� Ȱ��ȭ �����̸� ��Ȱ��ȭ�� �ٲ�.
        }


    }

}
