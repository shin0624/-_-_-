using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;

    [Header("Rotate")]
    public float mouseSpeed;//���� �ܰ� Inspectorâ������ ���� ������ ���� public���� �����ϳ�, �ڵ� ����ȭ �� ĸ��ȭ�� ���� Serialized Field�� ������ ��.
    float yRotation;
    float xRotation;
    Camera cam;//CameraController�� ���� �ۼ����� �ʰ�, GameObject�� ��ӹ޴� CameraŸ���� ������ ����.

    float smoothXRotation;
    float smoothYRotation;//ī�޶� ȸ�� �� ���� ������ ���� �Է� ������ ����.
    public float rotationSmoothTIme = 0.1f;//�Է� �������� ���� �ð�. Inspector���� ��������.

    [Header("Move")]
    public float moveSpeed;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // ���콺 Ŀ���� ȭ�� �ȿ��� ����
        Cursor.visible = false;                     // ���콺 Ŀ���� ������ �ʵ��� ����

        rb = GetComponent<Rigidbody>();             // Rigidbody ������Ʈ ��������
        rb.freezeRotation = true;                   // Rigidbody�� ȸ���� �����Ͽ� ���� ���꿡 ������ ���� �ʵ��� ����

        cam = Camera.main;                          // ���� ī�޶� �Ҵ�
        
    }

    void Update()
    {
        Rotate();//�� �����Ӹ��� ȸ�� ������Ʈ
    }

    void FixedUpdate()//Move()�� ���� ó���ϱ� ���� FIxedUpdate()
    {
        Move();//�� �����Ӹ��� �̵� ������Ʈ
    }


    void Move()
    {
        //h,v�� ���������� �����Ͽ� moveȣ�� �ÿ��� �޸𸮸� �Ҵ��ϹǷ� �޸� ��� ����ȭ ����
       float h = Input.GetAxisRaw("Horizontal"); // ���� �̵� �Է� ��
       float v = Input.GetAxisRaw("Vertical");   // ���� �̵� �Է� ��

        // �Է¿� ���� �̵� ���� ���� ���
        Vector3 moveVec = transform.forward * v + transform.right * h;

        //Rigidbody�� �̿��Ͽ� �̵��ϰ� �Ͽ� �� ����� ����.
        rb.MovePosition(rb.position + moveVec.normalized * moveSpeed * Time.fixedDeltaTime);
    }


    void Rotate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSpeed * Time.deltaTime;
#if ī�޶󶳸�����_������
        yRotation += mouseX;    // ���콺 X�� �Է¿� ���� ���� ȸ�� ���� ����
        xRotation -= mouseY;    // ���콺 Y�� �Է¿� ���� ���� ȸ�� ���� ����
#endif
        //�Է½������� �̿��Ͽ� ī�޶� ������ ����. 0.1�� ���� ����.
        smoothXRotation = Mathf.SmoothDamp(smoothXRotation, mouseX, ref smoothXRotation, rotationSmoothTIme);
        smoothYRotation = Mathf.SmoothDamp(smoothYRotation, mouseY, ref smoothYRotation, rotationSmoothTIme);

        yRotation += smoothXRotation;
        xRotation -= smoothYRotation;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // ���� ȸ�� ���� -90������ 90�� ���̷� ����

        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0); // ī�޶��� ȸ���� ����
        transform.rotation = Quaternion.Euler(0, yRotation, 0);             // �÷��̾� ĳ������ ȸ���� ����
    }
}
