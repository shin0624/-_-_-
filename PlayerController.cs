using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;

    [Header("Rotate")]
    public float mouseSpeed;//���� �ܰ� Inspectorâ������ ���� ������ ���� public���� �����ϳ�, �ڵ� ����ȭ �� ĸ��ȭ�� ���� Serialized Field�� ������ ��.
    float yRotation;
    float xRotation;
    Camera cam;//CameraController�� ���� �ۼ����� �ʰ�, GameObject�� ��ӹ޴� CameraŸ���� ������ ����.

    [Header("Move")]
    public float moveSpeed;

    float smoothXRotation;
    float smoothYRotation;//ī�޶� ȸ�� �� ���� ������ ���� �Է� ������ ����.
    public float rotationSmoothTIme = 0.01f;//�Է� �������� ���� �ð�. Inspector���� ��������.

    //24.05.06 ���� ��� �߰�
    public int JumpPower;//���� ���� ����
    private bool IsJumping;//���� ���� TF

    //24.05.06 ��ٸ� �����̵� �ڵ� �߰�
    [Header("Ladder")]
    public float climbSpeed;
    private bool isOnLadder;

    //24.05.18 ������ ������Ʈ �߰�-->ī�޶� ���
    [Header("Flash Light")]
    public GameObject FlashLight;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;// ���콺 Ŀ���� ȭ�� �ȿ��� ����
        Cursor.visible = false;                  // ���콺 Ŀ���� ������ �ʵ��� ����

        rb = GetComponent<Rigidbody>();          // Rigidbody ������Ʈ ��������
        rb.freezeRotation = true;                // Rigidbody�� ȸ���� �����Ͽ�
                                                 // ���� ���꿡 ������ ���� �ʵ��� ����

        cam = Camera.main;                       // ���� ī�޶� �Ҵ�

        IsJumping = false;//���� ���� �Ǵ�-->�� �ٴ��� �±׸� Ground�� �����Ͽ� Ground�� �ƴ� ���� ���� �Ұ��ϵ���(���� ���� ����)
   
        if(FlashLight!=null)//hierarchy�� �������� �ִٸ� ī�޶��� �ڽ����� ����
        {
            FlashLight.transform.SetParent(cam.transform);
            FlashLight.transform.localPosition = Vector3.zero;//������ ��ġ�� ī�޶� �����-->�÷��̾ ���Ϸ� �ü��� �Űܵ� �������� ����� �� �ְ�
            FlashLight.transform.localRotation = Quaternion.identity;//�������� ȸ���� �ʱ�ȭ
        }
    
    
    
    }

    void Update()
    {

        Rotate();//�� �����Ӹ��� ȸ�� ������Ʈ

        Jump();
    }

    void FixedUpdate()//Move()�� ���� ó���ϱ� ���� FIxedUpdate()
    {
        Move();//�� �����Ӹ��� �̵� ������Ʈ
        ClimbLadder();

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

        //�Է½������� �̿��Ͽ� ī�޶� ������ ����. 0.1�� ���� ����.
        smoothXRotation = Mathf.SmoothDamp(smoothXRotation, mouseX, ref smoothXRotation, rotationSmoothTIme);
        smoothYRotation = Mathf.SmoothDamp(smoothYRotation, mouseY, ref smoothYRotation, rotationSmoothTIme);

        yRotation += smoothXRotation;
        xRotation -= smoothYRotation;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // ���� ȸ�� ���� -90������ 90�� ���̷� ����


        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0); // ī�޶��� ȸ���� ����
        transform.rotation = Quaternion.Euler(0, yRotation, 0);             // �÷��̾� ĳ������ ȸ���� ����

        transform.rotation = Quaternion.Euler(0, yRotation, 0);             // �÷��̾� ĳ������ ȸ���� ����
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    void Jump()//���� �޼��� �߰�
    {
        if(Input.GetKeyDown(KeyCode.Space))//�����̽��� �Է� ��
        {
            if(!IsJumping)//�����̽��� �Է� && Ground�±װ� ���� �����κ��� �������� ��
            {
                IsJumping = true;
                rb.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);//������ٵ� ���� �߰����ִ� �޼��� ���-->ForceMode.Impulse : ������ٵ� ����(mass)�� ����Ͽ� �� ����� �߰�.
            }
            else//Ground�� ��� ������ �ȵǵ���(��������, �������� �Ұ�)
            {
                Debug.Log("Don`t Jump while IsJump = false");
                return;
            }
        }
    }
    public void OnCollisionEnter(Collision collision)//Ground �±� ���� Ȯ��
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            IsJumping = false;
        }
    }

    void ClimbLadder()//��ٸ� �����̵� �޼���
    {
        if(isOnLadder)//��ٸ��� ������
        {
            rb.useGravity = false;//�߷��� false�� ����
            float climbInput = Input.GetAxisRaw("Vertical");//���� �̵� ���� ������ �ִ´�.

            if (climbInput > 0) // W Ű�� ������ ��
            {
                rb.MovePosition(rb.position + Vector3.up * climbSpeed * Time.fixedDeltaTime);
            }
            else if (climbInput < 0) // S Ű�� ������ ��
            {
                rb.MovePosition(rb.position - Vector3.up * climbSpeed * Time.fixedDeltaTime);
            }
            // float climbInput = Input.GetAxisRaw("Vertical");
            //Vector3 climbvec = transform.up * climbInput;
            //rb.MovePosition(rb.position + climbvec.normalized * climbSpeed * Time.fixedDeltaTime);  
        }
    }
   
    private void OnTriggerEnter(Collider other)//���� �̵� ó���� ���� �ö��̴����� �浹�� �����Ѵ�. ����, jump�� ���������� ��ٸ��� tag�� Ladder�� ����.
    {
        if(other.CompareTag("Ladder"))//��ٸ��� �÷��̾��� �ö��̴��� �浹�ϸ� ���� �̵� �߻�
        {
            isOnLadder = true;
            Debug.Log("touch!");
        }
        
    }
    private void OnTriggerExit(Collider other)//�ö��̴� �浹�� ����Ǹ� �����̵� ����. 
    {
        if(other.CompareTag("Ladder"))
        {
            rb.useGravity = true;
            isOnLadder = false;
            Debug.Log("Leaving Ladder!");
        }
    }
    
}
