using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecurityDoorController : MonoBehaviour
{
    private Rigidbody rb;
    public DoorLockController doorLockController;

    private float OpenAngle = -4.4f;//��й�ȣ ���� �� ���� ���� ������ �����Ǵ� ����
    private float CloseAngle = 88.156f;//���� ���� ������ ����
    private bool isOpening = false;//���� ������ ������ ����
    private float doorSpeed = 2.0f;//���� ������ �ӵ�


    public AudioSource LockedDoorSound;//���� ��� ���¿��� E��ư�� ������ �� 
    public AudioSource OpenDoorSound;//��й�ȣ�� �ԷµǾ� ���� ������ ��

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationY; // �ʱ� ����: ���� ���� ����
        }
        if(LockedDoorSound==null || OpenDoorSound == null)
        {
            Debug.Log("AudioSource is NULL (Security Door). ");
        }

    }

    void Update()
    {
        if(doorLockController ==null)//����� ui�� Ȱ��ȭ���� ���� ����.
        {
            doorLockController = FindObjectOfType<DoorLockController>();//DoorLockController��ũ��Ʈ�� �ִ� ������Ʈ�� Ž��
        }
        if(doorLockController!=null)
        {
            if (doorLockController.OpenDoorFlag == false && Input.GetKeyDown(KeyCode.E))// ��� ���� E Ű�� ������ ��� �Ҹ��� ���
            {
                LockedDoorSound.Play();
                LockedDoorSound.loop = false;
            }
            if (rb != null && doorLockController.OpenDoorFlag)//��й�ȣ�� �¾� ���� ������
            {
                rb.constraints &= ~RigidbodyConstraints.FreezeRotationY; // ��й�ȣ�� ������ Y�� ȸ�� ���� ����
                //��Ʈ ���� -> y�� ȸ�� ���� ������Ƽ�� ��Ʈ�� 0���� ����
                Debug.Log("Door rotation unlocked.");
                OpenDoorSound.Play();
                OpenDoorSound.loop = false;
                isOpening = true;//�� ���� ����
            }
        }
        if (isOpening)//���� ������ ���� ��
        {
            float newYRotation = Mathf.LerpAngle(transform.eulerAngles.y, OpenAngle, Time.deltaTime * doorSpeed);
            //���� ��������->���� y�ఢ���� ���� ������ ���� ������ �� ������ �ӵ��� ���߾� ����
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, newYRotation, transform.eulerAngles.z);//�� �����̼� �� ����

            if(Mathf.Abs(transform.eulerAngles.y - OpenAngle) < 0.1f)//���� �� ���� - ��ȹ�� ������ ������ ���� 0�� �� ���̹Ƿ�.
            {
                isOpening = false;
                rb.constraints = RigidbodyConstraints.FreezeRotationY; // ���� ���� �� ȸ�� ����
            }
            
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
}