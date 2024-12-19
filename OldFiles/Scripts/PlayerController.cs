using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.UIElements;

// �÷��̾ �����ϴ� �÷��̾� ��Ʈ�ѷ� ��ũ��Ʈ. ��Ȱ�� ������ ���� �̱��� �������� ����
public class PlayerController : MonoBehaviour
{
    private static PlayerController PlayerInstance;// �̱��� �ν��Ͻ�

    Rigidbody rb;

    [Header("Move")]
    public float moveSpeed;

    //24.05.06 ���� ��� �߰�
    [Header("Jump")]
    public int JumpPower;//���� ���� ����
    private bool IsJumping;//���� ���� TF

    //24.05.06 ��ٸ� �����̵� �ڵ� �߰�
    [Header("Ladder")]
    public float climbSpeed;
    private bool isOnLadder;

    //24.06.18 �뽬 �߰�
    [Header("Dash")]
    public float dashSpeed;

    private void Awake()
    {
        if (PlayerInstance == null)
        {
            PlayerInstance = this;
            DontDestroyOnLoad(gameObject);//���� ��ȯ�Ǿ �÷��̾ �ı����� �ʵ��� ��ġ
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;// ���콺 Ŀ���� ȭ�� �ȿ��� ����
        Cursor.visible = false;                  // ���콺 Ŀ���� ������ �ʵ��� ����

        rb = GetComponent<Rigidbody>();          // Rigidbody ������Ʈ ��������
        rb.freezeRotation = true;                // Rigidbody�� ȸ���� �����Ͽ� ���� ���꿡 ������ ���� �ʵ��� ����

        IsJumping = false;//���� ���� �Ǵ�-->�� �ٴ��� �±׸� Ground�� �����Ͽ� Ground�� �ƴ� ���� ���� �Ұ��ϵ���(���� ���� ����)
    }

    void Update()
    {
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

        float currentSpeed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= dashSpeed;//���� ����Ʈ ������ �뽬
        }

        //Rigidbody�� �̿��Ͽ� �̵��ϰ� �Ͽ� �� ����� ����.
        rb.MovePosition(rb.position + moveVec.normalized * currentSpeed * Time.fixedDeltaTime);

    }

    void Jump()//���� �޼��� �߰�
    {
        if (Input.GetKeyDown(KeyCode.Space))//�����̽��� �Է� ��
        {
            //if(!IsJumping)//�����̽��� �Է� && Ground�±װ� ���� �����κ��� �������� ��
            // {
            IsJumping = true;
            rb.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);//������ٵ� ���� �߰����ִ� �޼��� ���-->ForceMode.Impulse : ������ٵ� ����(mass)�� ����Ͽ� �� ����� �߰�.
                                                                   // }
                                                                   // else//Ground�� ��� ������ �ȵǵ���(��������, �������� �Ұ�)
                                                                   // {
            ///   Debug.Log("Don`t Jump while IsJump = false");
            //  return;
            // }
        }
    }
    /* public void OnCollisionEnter(Collision collision)//Ground �±� ���� Ȯ��
     {
         if(collision.gameObject.CompareTag("Ground"))
         {
             IsJumping = false;
         }
     }*/

    void ClimbLadder()//��ٸ� �����̵� �޼���
    {
        if (isOnLadder)//��ٸ��� ������
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
        if (other.CompareTag("Ladder"))//��ٸ��� �÷��̾��� �ö��̴��� �浹�ϸ� ���� �̵� �߻�
        {
            isOnLadder = true;
            Debug.Log("touch!");
        }
    }
    private void OnTriggerExit(Collider other)//�ö��̴� �浹�� ����Ǹ� �����̵� ����. 
    {
        if (other.CompareTag("Ladder"))
        {
            rb.useGravity = true;
            isOnLadder = false;
            Debug.Log("Leaving Ladder!");
        }
    }

}
