
using Core.Scripts.Runtime.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanRoomSlidingDoor : MonoBehaviour
{
    //���� ������ �����̵� ���� ��ũ��Ʈ --> ����, ������ ���� �ѹ��� �����ϱ� ���� �� ������ �θ� ������Ʈ���� Ʈ���� �̺�Ʈ�� ����

    [SerializeField] private GameObject LeftDoor;//���� ��
    [SerializeField] private GameObject RightDoor;//������ ��
    [SerializeField] private AudioClip DoorSound01;//�� ������ �Ҹ�
    [SerializeField] private AudioClip DoorSound02;

    private Vector3 ClosedLeftPosition;//�����ִ� ���� �� ��ǥ
    private Vector3 ClosedRightPosition;//�����ִ� ������ �� ��ǥ
    private Vector3 OpenLeftPosition;//���� �� ���� �� ��ǥ
    private Vector3 OpenRightPosition;//������ �� ���� �� ��ǥ
    [SerializeField] private float OpenValue;

    private float SlidingSpeed = 0.1f;// �� ������ �ӵ�
    private bool isOpening = false;//���� ���ȴ��� �˷��ִ� �÷���
    private bool PlayerInTrigger = false;// �÷��̾ Ʈ���� ������ �ִ��� �˷��ִ� �÷���

    void Start()
    {
        LeftDoor = transform.Find("ShamanRoomDoor01").gameObject;// �� ������Ʈ�� �̸����� ���� Ž��
        RightDoor = transform.Find("ShamanRoomDoor02").gameObject;

        ClosedLeftPosition = LeftDoor.transform.position;//���� �� ��ġ ������ ����
        ClosedRightPosition = RightDoor.transform.position;

        OpenLeftPosition = new Vector3(ClosedLeftPosition.x, ClosedLeftPosition.y, ClosedLeftPosition.z - OpenValue);//z������ �����̵� �� ���� �� ��ġ���� ����
        OpenRightPosition = new Vector3(ClosedRightPosition.x, ClosedRightPosition.y, ClosedRightPosition.z + OpenValue);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))//�÷��̾ Ʈ���� ������ ���� ��
        {
            PlayerInTrigger = true;//Ʈ���� ������ �����ߴٴ� �÷��׸� true�� ����
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))//�÷��̾ Ʈ���� �������� ����� ��
        {
            PlayerInTrigger = false;//Ʈ���� ������ �����ߴٴ� �÷��׸� false�� ����
        }
    }

    private void Update()// ������Ʈ���� �÷��̾� Ʈ���� ���� ���ο� EŰ �Է��� üũ. OnTriggerEnter���� EŰ �Է��� ��� Ȯ���Ϸ� ������, GetKeyDown�� ª�� �ð����ȸ� �����Ͽ� �Է� ó���� ������ �߻��߱� ������ ������Ʈ���� ó��
    {
        if (PlayerInTrigger && !isOpening && Input.GetKeyDown(KeyCode.E))
        {
            isOpening = true;// �÷��̾ Ʈ���� ������ ���� + ���� ���� ���� + eŰ �Է� �� �� �������� ���� ����
            AudioManager.Instance.PlaySound(DoorSound02);
            AudioManager.Instance.PlaySound(DoorSound01);

        }
        if (isOpening)//�� ���� �����̸�
        {

            LeftDoor.transform.position = Vector3.Lerp(LeftDoor.transform.position, OpenLeftPosition, Time.deltaTime * SlidingSpeed);//�¿��� ���� ��ġ�� �̸� �����س��� ��ġ�� �ڿ������� �ű��.
            RightDoor.transform.position = Vector3.Lerp(RightDoor.transform.position, OpenRightPosition, Time.deltaTime * SlidingSpeed);

            if (Vector3.Distance(LeftDoor.transform.position, OpenRightPosition) < 0.01f && Vector3.Distance(RightDoor.transform.position, OpenLeftPosition) < 0.01f)// ��ǥ ��ġ���� �����ߴٸ�
            {
                LeftDoor.transform.position = OpenLeftPosition;//�¿��� ���� �ڿ������� ��ǥ ��ġ�� �����Ͽ� ���� �� �ֵ��� ó���Ѵ�.
                RightDoor.transform.position = OpenRightPosition;
                isOpening = false;//�� �������·� ����
                
            }
        }
    }

}
