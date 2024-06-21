using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;
using System.Security.Cryptography;

public class Under_TimelineController : MonoBehaviour
{
    //���� ���� �� ����� Ÿ�Ӷ��� ��� ��ũ��Ʈ.
    public PlayableDirector timeline;

    
    void Start()
    {
        if(timeline!=null)
        {
            timeline.Play();
        }
        else
        {
            Debug.LogError("Timeline is not assigned");
        }
    }
}
