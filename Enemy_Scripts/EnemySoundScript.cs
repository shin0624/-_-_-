using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ������ ���� ���ʹ� ���� ���� ��ũ��Ʈ. ���ʹ̿� �÷��̾� �� �Ÿ��� ��������� ������ ����ȴ�.

public class EnemySoundScript : MonoBehaviour
{
    public AudioSource ado;
    public GameObject player;
    public float detectionRange = 2f;//�ִ� �����Ÿ�
    public float fieldOfViewAngle = 120f;//���ʹ� �þ߰�


    private bool IsAroundPlayer;
    

    void Start()
    {
        IsAroundPlayer = false;
        
    }

   
    void Update()
    {
        CheckPlayerDistance();
    }
    
    private void CheckPlayerDistance()
    {
        float dis = Vector3.Distance(transform.position, player.transform.position);

        if(dis <=detectionRange)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

           if(angleToPlayer <= fieldOfViewAngle /2)//�÷��̾ ���� ������ �þ߰� �����϶�
            {
                if (!IsAroundPlayer)
                {
                    Debug.Log("sound playing");
                    IsAroundPlayer = true;
                    if (!ado.isPlaying)
                    {
                        ado.Play();
                    }

                }
            }
            else
            {
                if (IsAroundPlayer)
                {
                    Debug.Log("sound not playing(angle)");
                    IsAroundPlayer = false;
                    if (ado.isPlaying)
                    {
                        ado.Stop();
                    }

                }
            }

        }
        else
        {
            if(IsAroundPlayer)
            {
                Debug.Log("sound not playing(distance)");
                IsAroundPlayer = false;
                if (ado.isPlaying)
                {
                    ado.Stop();
                }
            }
        }
      
    }

}
