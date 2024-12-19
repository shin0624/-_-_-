using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DFSEnemyAI : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private Define.EnemyState state;
    public Animator anim;

    private float walkingStartTime; // �ȱ� ������ �ð�
    private const float WalkingDuration = 3.0f; // �ȱ� �Ѱ� �ð�

    private Stack<Vector3> dfsStack = new Stack<Vector3>(); // DFS Ž���� ���� ����
    private HashSet<Vector3> visitedPositions = new HashSet<Vector3>(); // �湮�� ��ġ�� �����ϴ� �ؽ��� ����. �ؽ����̹Ƿ� �ߺ� ���x

    void Start() // �ʱ� ���´� IDLE
    {
        state = Define.EnemyState.IDLE;
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = false;
    }

    void Update()
    {
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
            case Define.EnemyState.SEARCHING:
                UpdateSearching();
                break;
            case Define.EnemyState.ATTACK:
                UpdateAttack();
                break;
        }
    }

    private void UpdateIdle()
    {
        agent.speed = 0;
        agent.isStopped = true;
        if (CanSeePlayer()) // �÷��̾ ���ʹ��� �ü��� ������ walking���·� ����
        {
            SetState(Define.EnemyState.WALKING, "WALKING");
            walkingStartTime = Time.time; // �ȴ� �ð��� ������ �����Ѵ�.
        }
    }

    private void UpdateWalking()
    {
        if (agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.speed = 0.3f;
            SetState(Define.EnemyState.WALKING, "WALKING");
            if(CanSeePlayer())
            {
                agent.destination = target.transform.position; // ���ʹ��� ��ǥ�� �÷��̾�� ����
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance <= 5)
                {
                    SetState(Define.EnemyState.RUNNING, "RUNNING");
                    return;
                }
                if (Time.time - walkingStartTime >= WalkingDuration && distance >= 10) // ���ʹ̰� 3�� �̻� ������, �÷��̾ �þ� �ݰ� ���� ���ٸ� Ž�� ���·� ����
                {
                    SetState(Define.EnemyState.SEARCHING, "SEARCHING");
                    dfsStack.Clear(); // ���� �ʱ�ȭ �� ���� ��θ� ���ÿ� push.
                    visitedPositions.Clear(); // ������ ��ġ Ŭ����.
                    dfsStack.Push(transform.position); // ���� ��ġ�� ���ÿ� push.
                }
            }
           
        }
        else
        {
            Debug.Log("Agent is not on a NavMesh.");
        }
    }

    private void UpdateRunning()
    {
        if (agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.speed = 1.5f;
            agent.destination = target.transform.position;
            float distance = Vector3.Distance(transform.position, target.transform.position);

            //if (distance > 3 && distance <= 6) ---> 06.03 ���� : �Ÿ��� ��������� �����ϵ��� ����. ���� ����� ���� �߰� ����
            //{
              //  SetState(Define.EnemyState.WALKING, "WALKING");
                //walkingStartTime = Time.time;
            //}
            if(distance<=2)
            {
                SetState(Define.EnemyState.ATTACK, "ATTACK");
                if (distance > 2)
                    SetState(Define.EnemyState.RUNNING, "RUNNING");
                
            }
            else if (distance > 6)
            {
                SetState(Define.EnemyState.SEARCHING, "SEARCHING");
                dfsStack.Clear();
                visitedPositions.Clear();
                dfsStack.Push(transform.position);
            }
        }
        else
        {
            Debug.LogError("Agent is not on a NavMesh.");
        }
    }

    private void UpdateSearching()
    {
        if (agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.speed = 0.3f;

            if (CanSeePlayer())
            {
                SetState(Define.EnemyState.WALKING, "WALKING");
                walkingStartTime = Time.time;
                return;
            }

            if (!agent.hasPath || agent.remainingDistance < 0.5f) // ���ʹ̰� �������� ������ ���� �ʰų�, ���� ����� ���� �Ÿ��� 0.5 �̸��� ���
            {
                if (dfsStack.Count > 0) // ���ÿ� ��Ұ� �����ϸ�
                {
                    Vector3 currentPos = dfsStack.Pop(); // ���� �������� ���ÿ� �߰��� ��ġ�� pop�Ͽ� �湮 ��ġ�� ǥ��
                    visitedPositions.Add(currentPos); // ���� ��ġ�� �ؽü¿� �߰��Ͽ� �ߺ� �湮�� ����.

                    Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
                    // �� �� �� �� 4�������� Ž��
                    foreach (Vector3 direction in directions)
                    {
                        // �� �������� 5���� ������ ���ο� ��ġ�� ����Ѵ�.
                        Vector3 newPos = currentPos + direction * 0.5f;
                        if (NavMesh.SamplePosition(newPos, out NavMeshHit hit, 5.0f, NavMesh.AllAreas) && !visitedPositions.Contains(hit.position))
                        {
                            // ��ȿ�� ��ġ�� ���� �̹湮�̶�� �ش� ��ġ�� ���ÿ� �߰�.
                            dfsStack.Push(hit.position);
                        }
                    }
                    agent.destination = currentPos; // ���ʹ��� �������� ���ο� ��ġ�� ����.
                }
                else
                {
                    // Ž���� �� ��ģ ��� idle���·� ���ư�.
                    SetState(Define.EnemyState.IDLE, "IDLE");
                }
            }
        }
    }

    private void UpdateAttack()
    {
        if(agent.isOnNavMesh)
        {
            SetState(Define.EnemyState.ATTACK, "ATTACK");
            agent.isStopped = false;
            agent.speed = 1.5f;
            agent.destination = target.transform.position;
            float distance = Vector3.Distance(transform.position, target.transform.position);

        }
    }

    private bool CanSeePlayer() // ���ʹ��� �þ߿� �÷��̾ ���Դ��� �Ǵ��ϱ� ���� RayCast�� ���.
    {
        Vector3 directionToPlayer = (target.position - (transform.position + Vector3.up * 1.0f)).normalized; // ���ʹ� �����ǰ� �÷��̾� �������� �� : �÷��̾������ ���⺤��
        float distanceToPlayer = Vector3.Distance(transform.position + Vector3.up * 1.0f, target.position); // ���ʹ� ~ �÷��̾� ������ �Ÿ�

        float maxViewAngle = 60.0f; // �þ� �� ���� ����(60��)
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer); // ���ʹ��� forward����� �÷��̾� ���� �� ���� ���

        if (angleToPlayer <= maxViewAngle)
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 1.0f; // ray�� ���ʹ��� �Ӹ� ���̿��� �߻�ǵ��� ����
            Debug.DrawRay(rayOrigin, directionToPlayer * 10.0f, Color.red);

            if (Physics.Raycast(rayOrigin, directionToPlayer, out RaycastHit hit, 10.0f)) // �Ű����� : ����ĳ��Ʈ ���� ����, ����� ����, out hit, ���� ����
            {
                if (hit.transform == target) // Ray�� �÷��̾�� Hit�Ǿ��ٸ� ���� ��ȯ
                {
                    Debug.Log("Ray Hit to Player");
                    return true;
                }
                else
                {
                    Debug.Log("Ray did not hit the player, hit: " + hit.transform.name);
                    SetState(Define.EnemyState.SEARCHING, "SEARCHING");
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
        return false; // ���ʹ� �þ߿� �÷��̾ ���ٸ� ������ ��ȯ.
    }

    private void SetState(Define.EnemyState newState, string animationTrigger)
    {
        state = newState;
        anim.SetTrigger(animationTrigger);
    }

    public Define.EnemyState GetState()//�ܺο��� ���ʹ� ���¸� ���������� �� �� ���
    {
        return state;
    }
}
