using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorController : MonoBehaviour
{
    [SerializeField] private GameObject Doctor;
    [SerializeField] private GameObject Player;
    [SerializeField] private float Speed;

    [SerializeField] private Animator Anim;
    [SerializeField] private const float ChaseRange = 12.0f;//�÷��̾� �߰� ���� ����
    [SerializeField] private const float DetectionRange = 8.0f;// �÷��̾� Ž�� �Ÿ�
    [SerializeField] private const float AttackRange = 1.0f;// ���� ���� ����
    public JailDoorEventController Flag;

    private Define.DoctorState state;//���� ���º���
    private float DistanceToPlayer;//�÷��̾���� �Ÿ��� ������ ����

    void Start()
    {
        Doctor = GameObject.FindWithTag("Doctor");
        Player = GameObject.FindWithTag("Player");
        Anim = gameObject.GetComponent<Animator>();
        state = Define.DoctorState.IDLE;
        
    }


    void Update()
    {
        
    }

    void AwakeDoctor()
    {
        if(Flag.isEnter)
        {
            SetState(Define.DoctorState.WALKING, "WALKING");//�÷��̾ ���� 2���� ������ �����̱� �����Ѵ�.
        }
    }

    private void SetState(Define.DoctorState NewState, string AnimationTrigger)// ���º��� �޼���
    {
        if (state != NewState) { state = NewState; Anim.SetTrigger(AnimationTrigger); }//���ʿ��� ���� ������ �ּ�ȭ. ������ ���¿� �°� �ִϸ������� Ʈ���Ÿ� �ٲپ��ش�
    }

}
