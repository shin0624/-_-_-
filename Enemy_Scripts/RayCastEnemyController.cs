using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class RayCastEnemyController : MonoBehaviour
{
    //ENEMY AI ��ũ��Ʈ --> package manager -> AI Navigater ����Ʈ �� ���ʹ� ������Ʈ�� NavMeshAgent ������Ʈ �߰�

    public Transform target;//���ʹ��� Ÿ��
    private NavMeshAgent agent;
    private Define.EnemyState state;//Define��ũ��Ʈ�� ���ʹ� ���� ����ü ���� ����
    public Animator anim;//�ִϸ����� ����

    private float WalkingStartTime;
    private const float WalkingDuration = 3.0f;


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
#if if_else
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
#endif
        //anim.SetFloat("MoveSpeed", agent.speed);
        switch (state)
        {
            case Define.EnemyState.IDLE:
                UpdateIdle();
                break;
            case Define.EnemyState.WALKING:
                UpdateWalking();
                break;
            case Define.EnemyState.RUNNING:
                UpdateRunning();
                break;
                // case Define.EnemyState.ATTACK:
                // UpdateAttack();
                // break;
        }


    }

    private void UpdateIdle()
    {
        agent.speed = 0;
        agent.isStopped = true;

        float distance = Vector3.Distance(transform.position, target.transform.position);

        Debug.Log("Checking if can see player...");
        if (CanSeePlayer())//�÷��̾�� Ray�� Hit�ߴٸ�
        {
            try
            {
                state = Define.EnemyState.WALKING;
                anim.SetTrigger("WALKING");
                agent.isStopped = false;
                WalkingStartTime = Time.time;//�ȱ� ���� ���۽ð� ����
                Debug.Log("idle->walking");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error during IDLE : {e.Message}");
            }
        }
        else
        {
            state = Define.EnemyState.IDLE;
        }
    }

    private void UpdateWalking()
    {
        try
        {
            if (agent.isOnNavMesh)
            {
                agent.isStopped = false;
                agent.speed = 0.5f;
                agent.destination = target.transform.position;
                Debug.Log("Walking Now...Target : Player");

                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance <= 5)
                {
                    state = Define.EnemyState.RUNNING;
                    anim.SetTrigger("RUNNING");
                    return;
#if dd
                if (distance >= 6)
                {
                    Debug.Log("too far.. Idle");
                    state = Define.EnemyState.IDLE;
                    anim.SetTrigger("IDLE");
                    agent.isStopped = true;
                }

                if (Time.time - WalkingStartTime >= WalkingDuration)
                {
                    Debug.Log("Walking for 3 seconds...walking-->idle");
                    state = Define.EnemyState.IDLE;
                    anim.SetTrigger("IDLE");
                    agent.isStopped = true;
                }
#endif
                }
                //walkingduration �ʰ� �� distance�� 3 ���Ͽ��� idle���·� ���̵��� �ذ��ϱ� ����, ���� ����
                //->running���� ���� �� walkingduration ������ �����ؾ� ��
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
        catch (System.Exception e)
        {
            Debug.LogError($"Error during WALKING : {e.Message}");
        }
    }

    private void UpdateRunning()
    {
        try
        {
            if (agent.isOnNavMesh)
            {
                agent.isStopped = false;
                agent.speed = 2.0f;//�޸��� �ӵ��� 1.5f -->0���� ���� �������� �ϰ����
                agent.destination = target.transform.position;//���ʹ��� �������� �÷��̾�� ����
                float distance = Vector3.Distance(transform.position, target.transform.position);
                Debug.Log("Running Now...Target : Player");

                if (distance > 3 && distance <= 6)
                {
                    // �÷��̾���� �Ÿ��� 3 �̻� 6 ���Ϸ� �־����� �ȱ� ���·� ��ȯ
                    state = Define.EnemyState.WALKING;
                    anim.SetTrigger("WALKING");
                    WalkingStartTime = Time.time; // Walking ���� ���� �ð� �缳��
                }
                else if (distance > 6)//�÷��̾ ���ʹ̷κ��� �ָ� �������ٸ� �ٽ� idle���·�.
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

    private bool CanSeePlayer()//���ʹ��� �þ߿� �÷��̾ ���Դ��� �Ǵ��ϱ� ���� RayCast�� ���.
    {
        Vector3 directionToPlayer = (target.position - (transform.position + Vector3.up * 1.0f)).normalized;//���ʹ� �����ǰ� �÷��̾� �������� �� : �÷��̾������ ���⺤��
        //���ʹ� ��ġ + ���⺤�� ������ ���� Ray �߻� ������ ��ü�� �Ǹ� ���������� �߻�� �� �ֵ��� ��
        float distanceToPlayer = Vector3.Distance(transform.position + Vector3.up *1.0f, target.position);//���ʹ� ~ �÷��̾� ������ �Ÿ�

        float maxViewAngle = 60.0f;//�þ� �� ���� ����(60��)
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);//���ʹ��� forward����� �÷��̾� ���� �� ���� ���
        //���ʹ��� �������� ���Ϳ� (�÷��̾� ��ġ��ǥ�� - ���ʹ� ��ġ��ǥ��)�� ���� �� ������ ����Ѵ�.

        if(angleToPlayer <=maxViewAngle)
        {
            Vector3 rayOrigin = transform.position + Vector3.up* 1.0f;//ray�� ���ʹ��� �Ӹ� ���̿��� �߻�ǵ��� ����

            Debug.DrawRay(rayOrigin, directionToPlayer * 10.0f, Color.red);
            //���ʹ̿� �÷��̾� ���� ������ ���ʹ� �þ� �� ���� ���� ���� �� �̸�
            RaycastHit hit;//����ĳ��Ʈ ���� ���� --> ���ʹ̿� �÷��̾� ������ ��ֹ� ���� Ȯ�� ����
            if(Physics.Raycast(rayOrigin, directionToPlayer, out hit, 10.0f ))//�Ű����� : ����ĳ��Ʈ ���� ����, ����� ����, out hit, ���� ����
            {
                if(hit.transform == target)//Ray�� �÷��̾�� Hit�Ǿ��ٸ� ���� ��ȯ
                {
                    Debug.Log("Ray Hit to Player");
                    return true;
                }
                else
                {
                    Debug.Log("Ray did not hit the player, hit: " + hit.transform.name);
                }
            }
            else
            {
                Debug.Log("Ray did not hit anything.");
            }
        }
        else
        {
            Debug.Log("Player is out of view angle.");
        }
        return false;// ���ʹ� �þ߿� �÷��̾ ���ٸ� ������ ��ȯ.
    }
}