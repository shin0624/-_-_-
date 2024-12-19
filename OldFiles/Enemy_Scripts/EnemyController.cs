using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class EnemyController : MonoBehaviour
{
    //ENEMY AI ��ũ��Ʈ --> package manager -> AI Navigater ����Ʈ �� ���ʹ� ������Ʈ�� NavMeshAgent ������Ʈ �߰�

    public Transform target;//���ʹ��� Ÿ��
    NavMeshAgent agent;
    Define.EnemyState state;//Define��ũ��Ʈ�� ���ʹ� ���� ����ü ���� ����
    public Animator anim;//�ִϸ����� ����

    float WalkingStartTime;
    float WalkingDuration = 3.0f;





    void Start()
    {
        /*
        agent = GetComponent<NavMeshAgent>();//���ʹ̿��� �׺������ ������Ʈ ����
        target = GameObject.Find("PLAYER").transform;//�÷��̾��� ��ġ�� Ÿ������ ����
        agent.destination = target.transform.position;//���ʹ��� �������� �÷��̾�� ����
        */
        state = Define.EnemyState.IDLE;//���ʹ� ù ���� : ���޻���

        agent = GetComponent<NavMeshAgent>();//���ʹ� ����
        agent.isStopped = true;

    }


    void Update()
    {
        if (state == Define.EnemyState.IDLE)
        {
            UpdateIdle();

        }
        else if (state == Define.EnemyState.RUNNING)
        {
            UpdateRunning();
        }
        else if (state == Define.EnemyState.WALKING)
        {
            UpdateWalking();
        }
    }

    private void UpdateIdle()
    {
        agent.speed = 0;
        agent.isStopped = true;

        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance>4 && distance<7)//�÷��̾�� ���ʹ��� �Ÿ���4�̻� 7����(4<distance<7) ���ʹ̰� �ȱ� ����
        {
            try
            {
                agent.isStopped = false;
                state = Define.EnemyState.WALKING;
                anim.SetTrigger("WALKING");
                Debug.Log("idle->walking");
                

                WalkingStartTime = Time.time;
            }
            catch(System.Exception e)
            {
                Debug.LogError($"Error during IDLE : {e.Message}");
            }
    
            
        }
        else
            state = Define.EnemyState.IDLE;
    }

    private void UpdateWalking()
    {
       
        try
        {
            agent.isStopped = false;
            agent.speed = 0.5f;
            
            if (agent.isOnNavMesh)
            {
                agent.destination = target.transform.position;
                Debug.Log("Walking Now...Target : Player");
                float distance = Vector3.Distance(transform.position, target.transform.position);
              
                if (distance <=3)     
                {
                    state = Define.EnemyState.RUNNING;
                    anim.SetTrigger("RUNNING");
                   
                }


                if (Time.time - WalkingStartTime >= WalkingDuration)
                {
                    Debug.Log("Walking for 3 seconds...walking-->idle");
                    state = Define.EnemyState.IDLE;
                    anim.SetTrigger("IDLE");
                    agent.isStopped = true;
                }
            }
            else
            {
                Debug.Log("Agent is not on a NaviMesh.");
            }
            
        }
        catch(System.Exception e)
        {
            Debug.LogError($"Error during WALKING : {e.Message}");
        }
       

    }

    private void UpdateRunning()
    {
        try 
        {
            agent.isStopped = false;
            agent.speed = 1.5f;//�޸��� �ӵ��� 1.5f -->0���� ���� �������� �ϰ����
            if(agent.isOnNavMesh)
            {
                agent.destination = target.transform.position;//���ʹ��� �������� �÷��̾�� ����
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance== 2)
                {
                    //state = Define.EnemyState.ATTACK;//���� ���� �ȸ���.
                    //anim.SetTrigger("ATTACK");
                    Debug.Log("distance = 2");
                }
                else if (distance >= 6)//�÷��̾ ���ʹ̷κ��� �ָ� �������ٸ� �ٽ� idle���·�.
                {
                     state = Define.EnemyState.IDLE;
                     anim.SetTrigger("IDLE");
                     agent.isStopped = true;                
                }
            }
            else
            {
                Debug.LogError("Agent is not on a NavMesh.");
            }
           
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in UpdateRunning method: {e.Message}");
        }

 
    }
#if lookaround
    private void UpdateLookAround()
    {
        agent.speed = 0;
        state = Define.EnemyState.LOOK_AROUND;
        anim.SetTrigger("Look Around");

        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance == 2)//�Ÿ��� 2�� �Ǵ� ���� running���� ���� ����
        {
            state = Define.EnemyState.RUNNING;
            anim.SetTrigger("RUNNING");
        }

    }
#endif
   
}