using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutterRaiseController : MonoBehaviour
{
    //1�� ���Ƚ� ������ �ø��� ����� ���� �ִ� ���Ͱ� �ö󰡵��� �ϴ� ��ũ��Ʈ
    // ���� ���� : �÷��̾ ���� �տ��� EŰ�� ������ ������ �ö󰡴� �ִϸ��̼� �۵�( + ������ �ö󰡴� �Ҹ�) ->  ���Ͱ� �ö󰡴� �ִϸ��̼� �۵�(+ ���Ͱ� �ö󰡴� �Ҹ�)
    //(������ ���� ��� ���� ���·� ���ƿ��� �ִϸ��̼��� ���� �ʾ���.)

    [SerializeField]
    private GameObject Shutter;
    [SerializeField]
    private GameObject Knob;
    [SerializeField]
    private AudioSource ShutterRaiseSound;
    [SerializeField]
    private AudioSource KnobSound;
    [SerializeField]
    private GameObject SecondFloorTrigger;//������ �ö󰡸� ���� ��ǥ�� ��µǴ� Ʈ������ Ȱ��ȭ

    private Animator ShutterAnimator;
    private Animator KnobAnimator;

    private bool isActivated = false;//������ �̹� �ö� �ִ��� Ȯ���ϴ� �÷���


    void Start()//�� ������Ʈ�� �ִϸ����� ������Ʈ ��������
    {
        ShutterAnimator = Shutter.GetComponent<Animator>();
        KnobAnimator = Knob.GetComponent<Animator>();
        SecondFloorTrigger.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)//�÷��̾ Ʈ���� ������ ���� ��
    {
        if(other.CompareTag("PLAYER") && !isActivated)//�÷��̾ ������ ��ȣ�ۿ��� �� �ִ� ������ ���� �� + ������ �ö��� ���� ������ ��
        {
            InputManager.OnInteractKeyPressed += ActivateKnob;//EŰ �Է��� �̺�Ʈ�� ó��
            
        }
    }

    private void OnTriggerExit(Collider other)//�÷��̾ Ʈ���� ������ ����� ��
    {
        if(other.CompareTag("PLAYER"))
        {
            InputManager.OnInteractKeyPressed-= ActivateKnob;
        }
    }

    private void ActivateKnob()
    {
        KnobAnimator.SetTrigger("Raise");
        KnobSound.Play();

        ShutterAnimator.SetTrigger("Raise");
        ShutterRaiseSound.Play();

        isActivated= true;//���� Ȱ��ȭ ǥ��

        SecondFloorTrigger.SetActive(true);


        InputManager.OnInteractKeyPressed -= ActivateKnob;//���� �Է� �̺�Ʈ ����

    }
}
