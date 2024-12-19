using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameClearController : MonoBehaviour
{
    // 1�� ���� �� ���� Ŭ���� Ÿ�Ӷ��� ��� -> ���� Ŭ���� ĵ���� ��� ->  ����

    [SerializeField]
    private PlayableDirector Timeline;

    public int SceneCountStatic; // ���� �ٲ�� ���� ������ ��ī��Ʈ(��ü�����Ŵ����� �ִ� static����)

    private void Awake()
    {
       

        Timeline = GetComponent<PlayableDirector>();
        if(Timeline==null)
        {
            Debug.Log("Clear Timeline is NULL");
        }
    }
    private void Update()
    {
        SceneCountStatic = SceneChangeManager.SceneCount;// �� ī��Ʈ�� ���� ���� ������, �ѹ� �����ϸ� �� ���� �����Ǿ�����ϱ� ������Ʈ������ �� ��ȭ�� ��� ������
    }

    private void OnEnable()//Ÿ�Ӷ��� ���� �� �� ��ȯ �̺�Ʈ �ڵ鷯 ���
    {
        Timeline.stopped += OnTimelineFinished;
    }

    private void OnDisable()
    {
        Timeline.stopped-= OnTimelineFinished;
    }

    private void OnTriggerEnter(Collider other)// ���� 1���� �ι�°�� ���� �� && �÷��̾ Ʈ���� ������ ����� �� Ÿ�Ӷ��� ���
    {
        
        if(other.CompareTag("PLAYER") && SceneCountStatic==2 || SceneCountStatic==3 )
        {
            ClearTimelinePlay();
        }
    }

    private void ClearTimelinePlay()
    {
        Timeline.Play();
    }

    private void OnTimelineFinished(PlayableDirector director)//Ÿ�Ӷ��� ���� �� ���� ����
    {
        Application.Quit();

    }

}
