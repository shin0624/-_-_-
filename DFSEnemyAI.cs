/*
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.AI;

public class DFS_ENEMY_AI : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private Define.EnemyState state;
    public Animator anim;

    private float WalkingStartTime;//�ȱ� ������ �ð�
    private const float WalkingDurtaion = 3.0f;//�ȱ� �Ѱ� �ð�

    private Stack<Vector3> dfsStack = new Stack<Vector3>();// DFS Ž���� ���� ����
    private HashSet<Vector3> visitedPositions = new HashSet<Vector3>();//�湮�� ��ġ�� �����ϴ� �ؽ��� ����. �ؽ����̹Ƿ� �ߺ� ���x


    void Start()//�ʱ� ���´� IDLE
    {
        state = Define.EnemyState.IDLE;
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped= true;
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
        }
    }

    private void UpdateIdle()
    {
        agent.speed = 0;
        agent.isStopped= true;
        if(CanSeePlayer())//�÷��̾ ���ʹ��� �ü��� ������ walking���·� ����
        {
            try
            {
                state = Define.EnemyState.WALKING;
                anim.SetTrigger("WALKING");
                agent.isStopped = false;
                WalkingStartTime = Time.time;//�ȴ� �ð��� ������ �����Ѵ�.
            }
            catch(SystemException e)
            {
                Debug.LogError($"Error during IDLE : {e.Message}");
            }
           
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
                agent.destination = target.transform.position;//���ʹ��� ��ǥ�� �÷��̾�� ����
                float distance = Vector3.Distance(transform.position, target.transform.position);
                
                if(distance<=5)
                {
                    state = Define.EnemyState.RUNNING;
                    anim.SetTrigger("RUNNING");
                    return;
                }
                if(Time.time - WalkingStartTime >= WalkingDurtaion && distance>=10)// ���ʹ̰� 3�� �̻� ������, �÷��̾ �þ� �ݰ� ���� ���ٸ� Ž�� ���·� ����
                {
                    state = Define.EnemyState.SEARCHING;
                    anim.SetTrigger("SEARCHING");
                    agent.isStopped = false;

                    dfsStack.Clear();//���� �ʱ�ȭ �� ���� ��θ� ���ÿ� push.
                    visitedPositions.Clear();//������ ��ġ Ŭ����.
                    dfsStack.Push(transform.position);//���� ��ġ�� ���ÿ� push.
                }

            }
            else
            {
                Debug.Log("Agent is not on a NaviMesh.");
            }
        }
        catch(SystemException e)
        {
            Debug.LogError($"Error during WALKING : {e.Message}");
        }
    }

    private void UpdateRunning()
    {
        try
        {
            if(agent.isOnNavMesh)
            {
              agent.isStopped = false;
                agent.speed = 1.5f;
                agent.destination = target.transform.position;
                float distance = Vector3.Distance(transform.position, target.transform.position);

                if(distance > 3 && distance <=6)
                {
                    state = Define.EnemyState.WALKING;
                    anim.SetTrigger("WALKING");
                    WalkingStartTime = Time.time;
                }
                else if(distance >6)
                {
                    state = Define.EnemyState.SEARCHING;
                    anim.SetTrigger("SEARCHING");
                    agent.isStopped = false;

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
        catch(SystemException e)
        {
            Debug.LogError($"Error in UpdateRunning method: {e.Message}");
        }
    }

    private void UpdateSearching()
    {
        try
        {
            if(agent.isOnNavMesh)
            {
                agent.isStopped = false;
                agent.speed = 0.3f;

                if(CanSeePlayer())
                {
                    state = Define.EnemyState.WALKING;
                    anim.SetTrigger("WALKING");
                    WalkingStartTime = Time.time;
                    return;
                }
                if(!agent.hasPath || agent.remainingDistance <0.5)// ���ʹ̰� �������� ������ ���� �ʰų�, ���� ����� ���� �Ÿ��� 0.5 �̸��� ���
                {
                    if(dfsStack.Count>0)// ���ÿ� ��Ұ� �����ϸ�
                    {
                        Vector3 currentPos = dfsStack.Pop();//���� �������� ���ÿ� �߰��� ��ġ�� pop�Ͽ� �湮 ��ġ�� ǥ��
                        visitedPositions.Add(currentPos);//���� ��ġ�� �ؽü¿� �߰��Ͽ� �ߺ� �湮�� ����.

                        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
                        // �� �� �� �� 4�������� Ž��
                        foreach(Vector3 direction in directions)
                        {
                            //�� �������� 5���� ������ ���ο� ��ġ�� ����Ѵ�.
                            Vector3 newPos = currentPos + direction * 0.5f;
                            NavMeshHit hit;
                            if(NavMesh.SamplePosition(newPos, out hit, 5.0f, NavMesh.AllAreas) && !visitedPositions.Contains(hit.position))
                            {
                                //���Ӱ� ���� ��ġ�� Bake �� NavMesh �� ���� ��ȿ�� �κ����� Ȯ��. ��, ���ʹ̰� �̵��� �� �ִ� �κ����� Ȯ��. Hit�� ��ȿ�� ��ġ ������ ��ȯ.
                                //��ȿ�� ��ġ�� ���� �̹湮�̶�� �ش� ��ġ�� ���ÿ� �߰�. 
                                dfsStack.Push(hit.position);
                            }
                        }
                        agent.destination = currentPos;//���ʹ��� �������� ���ο� ��ġ�� ����.
                    }
                    else
                    {
                        //Ž���� �� ��ģ ��� idle���·� ���ư�.
                        state = Define.EnemyState.IDLE;
                        anim.SetTrigger("IDLE");
                    }
                }
            }
        }
        catch(SystemException e)
        {
            Debug.LogError($"Error in UpdateSearching method: {e.Message}");
        }
    }

    private bool CanSeePlayer()//���ʹ��� �þ߿� �÷��̾ ���Դ��� �Ǵ��ϱ� ���� RayCast�� ���.
    {
        Vector3 directionToPlayer = (target.position - (transform.position + Vector3.up * 1.0f)).normalized;//���ʹ� �����ǰ� �÷��̾� �������� �� : �÷��̾������ ���⺤��
        //���ʹ� ��ġ + ���⺤�� ������ ���� Ray �߻� ������ ��ü�� �Ǹ� ���������� �߻�� �� �ֵ��� ��
        float distanceToPlayer = Vector3.Distance(transform.position + Vector3.up * 1.0f, target.position);//���ʹ� ~ �÷��̾� ������ �Ÿ�

        float maxViewAngle = 60.0f;//�þ� �� ���� ����(60��)
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);//���ʹ��� forward����� �÷��̾� ���� �� ���� ���
        //���ʹ��� �������� ���Ϳ� (�÷��̾� ��ġ��ǥ�� - ���ʹ� ��ġ��ǥ��)�� ���� �� ������ ����Ѵ�.

        if (angleToPlayer <= maxViewAngle)
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 1.0f;//ray�� ���ʹ��� �Ӹ� ���̿��� �߻�ǵ��� ����

            Debug.DrawRay(rayOrigin, directionToPlayer * 10.0f, Color.red);
            //���ʹ̿� �÷��̾� ���� ������ ���ʹ� �þ� �� ���� ���� ���� �� �̸�
            RaycastHit hit;//����ĳ��Ʈ ���� ���� --> ���ʹ̿� �÷��̾� ������ ��ֹ� ���� Ȯ�� ����
            if (Physics.Raycast(rayOrigin, directionToPlayer, out hit, 10.0f))//�Ű����� : ����ĳ��Ʈ ���� ����, ����� ����, out hit, ���� ����
            {
                if (hit.transform == target)//Ray�� �÷��̾�� Hit�Ǿ��ٸ� ���� ��ȯ
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

*/
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

            if (distance > 3 && distance <= 6)
            {
                SetState(Define.EnemyState.WALKING, "WALKING");
                walkingStartTime = Time.time;
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
}
