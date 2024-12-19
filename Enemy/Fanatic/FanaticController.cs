using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FanaticController : MonoBehaviour
{
    //���ŵ� ĳ���� ��Ʈ�ѷ�
//--------------------------���ʹ� �ൿ ���� ����-----------------------------------------
    [SerializeField] private Transform Player;//�÷��̾��� Ʈ������
    [SerializeField] private NavMeshAgent Agent; // Bake�� NavMesh���� Ȱ���� ���ʹ�
    [SerializeField] private Animator Anim;
    [SerializeField] private const float ChaseRange = 8.0f;//�÷��̾� �߰� ���� ����
    [SerializeField] private const float DetectionRange = 7.0f;// �÷��̾� Ž�� �Ÿ�
    [SerializeField] private const float AttackRange = 3.0f;// ���� ���� ����
    private Define.EnemyState state;//���ʹ� ���� ����
    private float DistanceToPlayer;//�÷��̾���� �Ÿ��� ������ ����

//--------------------------���ʹ� ��� ��� ���� ����------------------------------------
    private List<Vector3> Path = new List<Vector3>();// A*�˰��������� ���� ��θ������� ����Ʈ
    private int CurrentPathIndex = 0;// ���ʹ̰� ���� �̵����� ��� ������ �ε���. ó������ Path[0]���� �̵�.

    //--------------------------���� �� ���ŷ� ���� ���� ����------------------------------
    public SanityManager sanityManager; // SanityManager�� ����
    private bool canAttack = true; // ���� ���� ���θ� üũ�� ����
    private float attackCooldown = 1.0f;//���� ��Ÿ��
    //--------------------------�Ҹ� ���� ����----------------------------------------

    [SerializeField] private AudioSource ado2;// ���� �� ���� �Ҹ�
    
    //------------------------------------------------------------------------------------
    private void Start()
    {
        Player = GameObject.Find("Player").transform;
        state = Define.EnemyState.IDLE;//�ʱ���� : IDLE
        Agent = GetComponent<NavMeshAgent>();
        Agent.isStopped = true;
        Anim = GetComponent<Animator>();
        sanityManager = GameObject.FindObjectOfType<SanityManager>(); // �߰�: SanityManager ã��
        BeginPatrol();//ó���� Ž�� ����
    }

    private void Update()
    {
        DistanceToPlayer = Vector3.Distance(transform.position, Player.position);//�÷��̾�� ���ʹ� ������ �Ÿ��� ���
        switch (state)
        { 
            case Define.EnemyState.IDLE:
            break;
            case Define.EnemyState.WALKING:
                Patrol();//��ο� ���� Ž���� ��� ����
                break;
            case Define.EnemyState.RUNNING:
                UpdateChase();// �÷��̾ �߰�
                break;
            case Define.EnemyState.ATTACK:
                UpdateAttack();//�÷��̾ ����
                break;
        }    
    }

    private void Patrol()// Ž������
    {
        if(Agent.isOnNavMesh && Path.Count>0)
        {
            Agent.isStopped = false;
            Agent.speed = 0.5f;

            if(DistanceToPlayer <=DetectionRange && state!=Define.EnemyState.ATTACK)//Ž�� ���� ���� �÷��̾ �����ϸ� && ���� ���°� �ƴ� �� �߰��� �����Ѵ�.
            {
                SetState(Define.EnemyState.RUNNING, "RUNNING");
                return;
            }

            if(!Agent.hasPath || Agent.remainingDistance < 1.0f)//���� ��ΰ� ���ų�, ��ǥ ������ �����ϸ�
            {
                if(CurrentPathIndex < Path.Count)//��� ���� ���� �������� �̵�
                {
                    Agent.SetDestination(Path[CurrentPathIndex]);
                    CurrentPathIndex++;//���� �̵����� ����� ���� ��η� �̵��� ��.
                }
                else// ��� ���� �����ϸ� �� ��� ���
                {
                    CalculateNewPath();
                }
            }
        }
    }

    private void BeginPatrol()
    {
        SetState(Define.EnemyState.WALKING, "WALKING"); // �ɾ�ٴϸ� Ž�� ����
        Agent.isStopped = false;
        CalculateNewPath();// ���ο� ��θ� ���
        Debug.Log($"���� �÷��̾���� �Ÿ� : {DistanceToPlayer}" );
        Debug.Log($"���� ���� : {state}");
    }

    private void UpdateAttack()// ���� �� -> �÷��̾���� �Ÿ��� ���� ���� ������ �Ѿ ���� && �÷��̾���� �Ÿ��� ���� Ž�� ������ ���Ե� �� �ٽ� �÷��̾ �����ؾ� ��.
    {
        if(Agent.isOnNavMesh)
        {
            Agent.isStopped = true;//���� �� �� �ڸ����� ����
            SetState(Define.EnemyState.ATTACK, "ATTACK");
            if(DistanceToPlayer > AttackRange)// ���ݹ����� ����ٸ�
            {
                UpdateChase();
                return;
            }
           
        
            if(canAttack)
            {
            Debug.Log($"���� �÷��̾���� �Ÿ� : {DistanceToPlayer}" );
            Debug.Log($"���� ���� : {state}");
            sanityManager.DecreaseSanity(); //���ŷ� ���� ȣ��
            StartCoroutine(AttackCooldown());
            }
        
        
        } 
    }

    private void UpdateChase()
    {
        if(Agent.isOnNavMesh)
        {
        Debug.Log($"���� �÷��̾���� �Ÿ� : {DistanceToPlayer}" );
        Debug.Log($"���� ���� : {state}");
            ado2.Play();
            SetState(Define.EnemyState.RUNNING, "RUNNING");
            Agent.isStopped = false;
            Agent.speed = 2.0f;
            Agent.destination = Player.position;// �������� �÷��̾� ���������� �����Ͽ� �߰�
            if(DistanceToPlayer > ChaseRange)//�÷��̾���� �Ÿ��� �߰� ���� ������ ����ٸ�
            {
                ado2.Stop();
                BeginPatrol();//Ž�� ���·� ��ȯ 
            }
            else if(DistanceToPlayer < AttackRange)//���� ���� �������� �ٰ����ٸ�
            {
                UpdateAttack();
                return;
            }
        }
    }

    private void CalculateNewPath()// ���ο� ��θ� ����ϴ� �޼���
    {
        Vector3 RandomDirection = Random.insideUnitSphere * 10.0f;// �ݰ� 1�� ���� �� ���� ���� ���� * 10���� ��� ����
        RandomDirection += transform.position;// ���ʹ� ������ ���� ���� ���� ���Ѵ�

        NavMeshHit hit;
        //SamplePosition((Vector3 sourcePosition, out NavmeshHit hit, float maxDistance, int areaMask)
        // ���������� �޼��� : areaMask�� �ش��ϴ� NavMesh �߿���, maxDistance �ݰ� ������ sourcePosition�� �ֱ����� ��ġ�� ã�� hit�� ��´�.
        if (NavMesh.SamplePosition(RandomDirection, out hit, 10.0f, NavMesh.AllAreas))
        {
            Path.Clear();
            Path.Add(hit.position);//A* �˰����� �ش��ϴ� ��� ����
            CurrentPathIndex = 0;// ������ġ�� ��� ���� �ʱ�ȭ
            Agent.SetDestination(Path[CurrentPathIndex]);
        }
    }

    private void SetState(Define.EnemyState NewState, string AnimationTrigger)// ���º��� �޼���
    {
        if (state != NewState) { state = NewState; Anim.SetTrigger(AnimationTrigger); }//���ʿ��� ���� ������ �ּ�ȭ. ������ ���¿� �°� �ִϸ������� Ʈ���Ÿ� �ٲپ��ش�
    }
    
    private IEnumerator AttackCooldown()//���� ��Ÿ���� �����ϴ� �ڷ�ƾ. attackCooldown��ŭ ��� �� ���� ���ɻ��·� ��ȯ
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}