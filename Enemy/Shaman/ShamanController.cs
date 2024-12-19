using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ShamanController : MonoBehaviour
{
    # region shaman������������
    //���� ����ĳ���� ��Ʈ�ѷ� : ������ IDLE, ATTACK, RUNNING, CHOKE, ROAR���°� ����
    //�÷��̾ ���� �Ÿ��� ������ ������ IDLE
    //�÷��̾ Ž���Ǹ� ROAR ��� -> RUNNING ���·� ���� ��ȯ
    //�÷��̾���� �Ÿ��� ���� ���� ���� ���� �� : ATTACK �Ǵ� CHOKE
    //CHOKE ���¿� ATTACK ���´� RUNNING���� ��ȯ�Ǹ�, ���� Ȯ���� CHOKE�� ���õ�
    //CHOKE ���·� ��ȯ�Ǹ� �÷��̾��� ���� ���� ���� ���ø�. �ѹ��� sanity�� 2�� ���̴� �ʻ��.
    // ����ĳ���ʹ� NAVMESH ������ �����δ�.
    //�÷��̾���� �Ÿ��� ���� �̻����� �־����� NAVMESH ���� �ٸ� ��ǥ�� �̵��Ͽ� �÷��̾� ������ IDLE���·� ��ٸ���.
    // ���� ĳ���ʹ� Behavior Tree �������� �����Ͽ� CHOKE�� ATTACK ���� ���Ǻ� ��ȯ�� �ܼ�ȭ �� ��, ���� ����ĳ�����̱⿡ ���� Ȯ�强�� ����� �� ���� 
    // �ൿ Ʈ�� ������ ������� Choke�� attack ���� ���θ� Ȯ���� ����, idle->combat selector -> running�� �ݺ�
    # endregion
    //---�ʼ� �Ҵ� ������Ʈ��--
     public NavMeshAgent agent;
     public Animator animator;
     public Transform player;
     [SerializeField] public AudioClip ado;
     [SerializeField] public CameraController cameraController;

    // --- �÷��̾� Ž�� ���� ������---
     [Header("Detection Settings")]
     public float detectionRange = 15.0f;
     public float attackRange = 2.5f;
     public float chokeRange = 0.7f;
     public float chokeProbility = 0.2f;//��ũ, ������ �������� ���õǾ�� �ϹǷ� Ȯ���� ����

     //--- �ൿƮ�� ���� ������ --
     private BehaviorNode rootNode;//BT�� ��Ʈ��� ���� ����
     public bool isChoking = false;// �������� ���� ���ΰ�?
     
     void Start()   
     {  
        agent  = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        cameraController = FindAnyObjectByType<CameraController>();
        player = GameObject.FindGameObjectWithTag("PLAYER").transform;
        SetUpBT();
     }

      private void OnDrawGizmos()
      {
         // Ž�� �Ÿ�
         Gizmos.color = Color.green;
         Gizmos.DrawWireSphere(this.transform.position, detectionRange);

         // ���� ���� ��Ÿ�
         Gizmos.color = Color.blue;
         Gizmos.DrawWireSphere(this.transform.position, attackRange);
      }

     private void Update() 
     {
      rootNode.Evaluate();//�� ������ �� ����
     }

     public void PlayRoarEffects()
     {
         
         AudioSource.PlayClipAtPoint(ado, transform.position);//Shaman�� �ִ� ��ҿ��� �Ҹ��� ��������� �ϱ� ������ PlayClipAtPoint�� ���.
         cameraController.StartShake();// ��ȿ�Ҹ��� �鸮�鼭 ī�޶� ��鸲 �߻�
     }

     private void SetUpBT()//�ൿƮ�� �¾�
     {
      Debug.Log("BT START");
        rootNode = new SelectorNode(); // ��Ʈ ���. ����->���������� �����ϸ� �켱 ������ ���� �ڽ� ������ ����. �ڽ� ���� �� ������ ��尡 �ִٸ� �� ��带 �����ϰ� ����.
        
        //idle ����Ʈ��
        var idleSequence = new SequenceNode();
        idleSequence.AddChild(new CheckPlayerDistanceNode(this, detectionRange, true));//�÷��̾���� �Ÿ� üũ
        idleSequence.AddChild(new IdleActionNode(this));

        //combat ����Ʈ��
        var combatSelector = new SelectorNode();

        //Choke������
        var chokeSequence = new SequenceNode();
        chokeSequence.AddChild(new CheckChokeConditionNode(this)); //��ũ�� ������ �Ǵ��� üũ
        chokeSequence.AddChild(new ChokeActionNode(this));

        //Attack������
        var attackSequence = new SequenceNode();
        attackSequence.AddChild(new CheckAttackRangeNode(this));//������ ������ �Ǵ��� üũ
        attackSequence.AddChild(new AttackActionNode(this));

        //Chase������
        var chaseSequence = new SequenceNode();
        chaseSequence.AddChild(new CheckPlayerDistanceNode(this, detectionRange,false)); // �߰��� ������ �Ǵ��� üũ
        chaseSequence.AddChild(new ChaseActionNode(this));

         //2��° ���(����or��ũor�߰�)�� �ڽ����� ������ �������� ����
        combatSelector.AddChild(chokeSequence);
        combatSelector.AddChild(attackSequence);
        combatSelector.AddChild(chaseSequence);

        //��Ʈ��忡 IDLE ����Ʈ��, CombatSelector ����Ʈ���� �ڽ����� ����
        rootNode.AddChild(idleSequence);
        rootNode.AddChild(combatSelector);
     }
}