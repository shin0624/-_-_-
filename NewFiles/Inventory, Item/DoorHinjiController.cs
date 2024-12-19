using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHinjiController : MonoBehaviour
{
    // �� ������Ʈ�� �÷��̾ ������ E Ű�� ������ ���� ������ ��ũ��Ʈ
    //  ** �߿�** ���� ���������� �������� �� ������Ʈ�� �ǹ��� ��ø�� �ش��ϴ� ��ġ�� �ű��, ������ٵ� ������Ʈ(�߷»�� x, Ű�׸�ƽ����)�� �ڽ��ö��̴�(Ʈ���ŷ� ����, �� ũ��� ��ġ�� ���߱�), Interactable ��ũ��Ʈ, �±׸� Interactable�� �ٲٴ� �۾� �ʿ�



    [SerializeField]
    private float OpenAngle;// ���� ������ ����
    [SerializeField]
    private float CloseAngle; //���� ���� ������ ����
    private bool isOpening = false; //���� �����ִ��� ����
    private float DoorSpeed = 2.0f; //���� ������ �ӵ�

    private Rigidbody rb; //���� ������ٵ� --> ���� �Ѻ� ���� �� ���°� ���ϴ� ������ �����ϱ� ���� �ϴ� ������� ä�� ������Ŵ. �Ŀ� �ݴ� ��� �߰�

    [SerializeField]
    private AudioSource OpenDoorSound;//���� ������ �Ҹ�

    void Start()
    {
        CloseAngle = gameObject.transform.rotation.eulerAngles.y;

        // ������ٵ�� ������ҽ��� ���δ�.
        rb = GetComponent<Rigidbody>();
        if(rb==null)
        {
            Debug.Log("Door Rigidbody is NULL");
        }

        OpenDoorSound = GetComponent<AudioSource>();
        if(OpenDoorSound ==null)
        {
            Debug.Log("OpenDoorSound is NULL");
        }
    }

    private void OnTriggerEnter(Collider other)//�÷��̾ Ʈ���� ���� ���� �� + ���� ���� �����̸�
    {
        if(other.CompareTag("PLAYER") && !isOpening)
        {
            InputManager.OnInteractKeyPressed += OpenDoor;// EŰ �Է��� �̺�Ʈ�� ó��
        }
    }
    private void OnTriggerExit(Collider other)//�÷��̾ Ʈ���� ������ ����� �̺�Ʈ ���� ����
    {
        if (other.CompareTag("PLAYER"))
        {
            InputManager.OnInteractKeyPressed -= OpenDoor;
        }
    }

    private void OpenDoor()// ���� ������ ���ϸ� ������ �̺�Ʈ �޼���
    {
        isOpening = true; 
        OpenDoorSound.Play();
        OpenDoorSound.loop = false;

        // ������������ �ڿ������� �������� ǥ��
        float newYRotation = Mathf.LerpAngle(transform.eulerAngles.y, OpenAngle, Time.deltaTime * DoorSpeed);
        //���� ���ο� ���Ϸ����� ����
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, newYRotation, transform.eulerAngles.z);

        //������ �����ߴ� ���� ������ ���� ������ ��ġ�Ѵٸ� 0�� �� ���̹Ƿ�, ���� ���� ������ �����ϰ� Y������ ����
        if(Mathf.Abs(transform.eulerAngles.y - OpenAngle) < 0.1f)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationY;
        }

    }


}
