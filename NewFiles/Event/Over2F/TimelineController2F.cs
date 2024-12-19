using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController2F : MonoBehaviour
{
    // 2������ �߻��ϴ� Ÿ�Ӷ��� �̺�Ʈ�� ���� ��ũ��Ʈ. �� ������ ���� E Ű�� ������ Ÿ�Ӷ����� ����ǰ�, Ÿ�Ӷ��� ���� �� ���� ������ �̵��ϰ� �� ��.

    [SerializeField]
    private PlayableDirector timeline;

    private void Awake()
    {
        timeline = GetComponent<PlayableDirector>();//�÷��̾�� ���� ������Ʈ�� ã�´�.
        if (timeline == null)
        {
            Debug.Log("Timeline(2F) is NULL!");
        }
    }

    private void OnEnable()
    {
        timeline.stopped += OnTimelineFinished;//Ÿ�Ӷ��� ���� �̺�Ʈ �ڵ鷯 �߰�
    }

    private void OnTriggerStay(Collider other)//�浹���� + EŰ�� ������ ui�� Ȱ��ȭ�Ǿ�� �ϹǷ�, �������� Ű �Է� ������ �ʿ�.
                                              //OnCollisionEnter�� �浹 ������ ȣ��Ǳ� ������ ������
    {
        if (other.gameObject.CompareTag("PLAYER"))//�÷��̾ ������ �� ��
        {
            if(timeline!=null)
            {
                InputManager.OnInteractKeyPressed += TimelinePlay;// E Ű �Է��� �̺�Ʈ�� ó��
            }
            else
            {
                Debug.Log("Timeline(2F) is NULL!");
            }
            

        }
    }

    private void OnTriggerExit(Collider other)//�÷��̾ Ʈ���� ������ ����� ��
    {
        if (other.CompareTag("PLAYER"))
        {
            InputManager.OnInteractKeyPressed -= TimelinePlay;
        }
    }

    private void TimelinePlay()//Ÿ�Ӷ��� �÷��� ����
    {
        timeline.Play();
    }

    private void OnTimelineFinished(PlayableDirector director)//Ÿ�Ӷ��� ���� �� ���� ������ �̵���.
    {
        LoadingSceneManager.LoadScene("NewUnder3F");//�ε����� ���� ȣ�� �� �ش� ���� ȣ��.
    }
}
