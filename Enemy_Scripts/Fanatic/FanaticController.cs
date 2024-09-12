using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Android;

public class FanaticController : MonoBehaviour
{
    //���ŵ� ĳ���� ��Ʈ�ѷ�

    [SerializeField]
    private Transform Player; // �÷��̾��� Ʈ������
    [SerializeField]
    private NavMeshAgent agent;// Bake�� NavMesh���� Ȱ���� ���ʹ�
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private const float ChaseRange = 12.0f;// �÷��̾� �߰ݰ��� ����
    [SerializeField]
    private const float DetectionRange = 8.0f;//�÷��̾� Ž�� �Ÿ�
    [SerializeField]
    private const float AttackRange = 1.0f;// ���� ���� ����

    private Define.EnemyState state;//���ʹ� ������Ʈ ���� ����
    private float DistanceToPlayer;//�÷��̾���� �Ÿ��� ������ ���� ����

    private Stack<Vector3> dfsStack = new Stack<Vector3>(); // ���̿켱Ž�� ����
    private HashSet<Vector3> visitedPositions = new HashSet<Vector3>();// �湮�� ��ġ�� ������ �ؽ���

    void Start()
    {
        state = Define.EnemyState.IDLE; // �ʱ���� : idle
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped= true;
        BeginDFS();//ó���� DFS Ž�� ����
    }
    
    void Update()
    {
        DistanceToPlayer = Vector3.Distance(transform.position, Player.position);// �Ÿ� ����� �� �޼��尡 �ƴ� Update������ ����. �ﰢ������ ���ʹ� ���°� ����Ī�Ǿ�� �ϹǷ�, �� ������ ����ϴ� ���� ������ �� ����.

        switch(state)
        {
            case Define.EnemyState.IDLE:
            case Define.EnemyState.WALKING:
                UpdateDFS();//Ž���� ���� ���� ���ƴٴѴ�.
                break;
            case Define.EnemyState.RUNNING:
                UpdateChase();//�÷��̾ �߰�
                break;
            case Define.EnemyState.ATTACK:
                UpdateAttack();//�÷��̾ ���� 
                break;
        }
    }

    private void BeginDFS()// dfsŽ���� �����ϴ� �޼���
    {
        dfsStack.Clear();// ���� �ʱ�ȭ
        visitedPositions.Clear();//�湮��� �ؽ��� �ʱ�ȭ
        dfsStack.Push(transform.position);// ���ʹ��� ���� ��ġ���� Ž�� ����

        SetState(Define.EnemyState.WALKING, "WALKING");//�ɾ�ٴϸ� Ž�� ����
    }

    private void UpdateDFS()
    {
        if(agent.isOnNavMesh && dfsStack.Count>0)//���ʹ̰� bake�� navmesh�� �ְ�, Ž���� ���۵Ǿ��� ��
        {
            agent.isStopped = false;
            agent.speed = 0.2f;
            if(DistanceToPlayer <=DetectionRange)// �����س��� Ž�� �ݰ� ���� �÷��̾ �ִٸ� --> �߰�(running)
            {
                SetState(Define.EnemyState.RUNNING, "RUNNING");
                return;
            }
            
            //Ž�� ����
            if(!agent.hasPath || agent.remainingDistance < 3.0f)// �ʱ� ��θ� �������� �ʰų� ��ǥ������ ������ ���
            {
                Vector3 CurrentPos = dfsStack.Pop();//���ÿ��� ��ġ�� ������ �̵�
                visitedPositions.Add(CurrentPos);//�湮�� ��ġ�� ���.

                //�����¿� 4�������� Ž���� ����.
                Vector3[] directions = {Vector3.forward, Vector3.back, Vector3.left, Vector3.right};
                foreach (Vector3 direction in directions)
                {   
                    Vector3 NewPos = CurrentPos + direction * 1.1f ;// 1.5m �������� ���ο� ��ġ�� ���
                    //SamplePosition((Vector3 sourcePosition, out NavmeshHit hit, float maxDistance, int areaMask)
                    // ���������� �޼��� : areaMask�� �ش��ϴ� NavMesh �߿���, maxDistance �ݰ� ������ sourcePosition�� �ֱ����� ��ġ�� ã�� hit�� ��´�.
                    if (NavMesh.SamplePosition(NewPos, out NavMeshHit hit, 3.0f, NavMesh.AllAreas ) && !visitedPositions.Contains(hit.position)) //1.5m �̳�����, ���� ������ ����� ��ġ�� �ֱ����� ��ġ�� ã�� hit�� ��´�.
                    {
                        dfsStack.Push(hit.position);//��ȿ�� ��ġ�̸� ���ÿ� �߰�   
                    }
                }
                agent.destination= CurrentPos;//�̵��� ��ġ ����
                Debug.Log($"Destination : {CurrentPos}  / agent Dest : {agent.destination}");

                if(agent.destination == CurrentPos)
                {
                    Debug.Log("Same destination is set repeatedly. Confirmation required");
                }  
            }
        }
    }

    private void UpdateChase()// �߰ݻ��� �޼���
    {
        if(agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.speed = 0.7f;
            agent.destination = Player.position;

            if (DistanceToPlayer > ChaseRange)//�÷��̾ ���� ���� ������ ������
            {
                BeginDFS();//�ٽ� Ž�� ����
            }
            else if(DistanceToPlayer <AttackRange)//�÷��̾���� �Ÿ��� AttackRange ���Ϸ� �پ��� ���ݻ��·� ��ȭ
            {
                UpdateAttack();
                return;
            }
        }
    }

    private void SetState(Define.EnemyState NewState, string AnimationTrigger)
    {
        if(state!=NewState)//���ʿ��� ���� ����(running���¿��� �� running�� �����ϴ� ��� ��) �ּ�ȭ
        {
            state = NewState;
            anim.SetTrigger(AnimationTrigger);//�ִϸ��̼� ���� ��ȯ
        }
    }

    private void UpdateAttack()
    {
        if(agent.isOnNavMesh)
        {
            agent.isStopped = true;// �÷��̾ ������ �ٰ����� ���缭 ����
            SetState(Define.EnemyState.ATTACK, "ATTACK");

            if (DistanceToPlayer > AttackRange)//�÷��̾ ���� ���� ���� ������ ������
            {
                UpdateChase();// �ٽ� �i�� ���� -> �ٽ� ���� ������ ������ �����ϰ�, �״�� ������ ����� Ž������ ��ȯ.
                return;
            }
        }
    }
}
