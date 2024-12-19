using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class JailDoorOpen : MonoBehaviour
{
    //���� 2�� ������ ���� ���� ��ũ��Ʈ. ��� ���� ���� y�� Rotation ���� 90 --> 180���� ������ ��.
    private GameObject JailDoor;
    private AudioSource JailDoorOpenSound;

    private float OpenDoorAngle = 180.0f;//���� ���� ������ ����
    private float OpenDoorSpeed = 5.0f;//���� ���� ������ �ð�
    private bool OpenDoorTag = false;

    void Start()
    {
        JailDoor = this.gameObject;
        JailDoorOpenSound = JailDoor.GetComponent<AudioSource>();

        if (JailDoor == null)
        {
            Debug.LogError("JailDoor is null");
        }

        if (JailDoorOpenSound == null)
        {
            Debug.LogError("AudioSource is null");
        }
    }

    void Update()
    {
        
    }

    public void StartOpenDoorBust()//���� ���� ���� �޼���
    {
        // float newDoorAngle = Mathf.LerpAngle(transform.eulerAngles.y, OpenDoorAngle, Time.deltaTime * OpenDoorSpeed);//���� ���������� ���� (���� y��, ���� ���� y��)�� OpenDoorSpeed�� �°� ����
        if (OpenDoorTag == false)//T/F���� ����� �ι� ���� ȣ��� ���� ���� �Ѵ�.
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, OpenDoorAngle, transform.eulerAngles.z);//�� ���� ����. 
            

            JailDoorOpenSound.Play();
            JailDoorOpenSound.loop = false;
            OpenDoorTag = true;
        }

       
    }
}
