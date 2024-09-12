/*
 * PlayerController.cs
 * �� ��ũ��Ʈ�� �÷��̾��� ������, ����, �뽬, ��ٸ� Ÿ��� ���� �ൿ�� ����
 * Rigidbody�� ����Ͽ� ���� ����� �̵��� �����ϸ�, �߼Ҹ� ����� ����
 * �̱��� ������ �����Ͽ� �� ��ȯ �ÿ��� �÷��̾� ������Ʈ�� ����
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾ �����ϴ� PlayerController ��ũ��Ʈ
public class PlayerController : MonoBehaviour
{
    // �̱��� �ν��Ͻ� ����
    private static PlayerController instance;

    Rigidbody rb;

    [Header("Scripts")]
    public AudioManager audioManager;

    [Header("Sounds")]
    public List<AudioClip> walkFootstepClips;   // ���� �� �߼Ҹ� Ŭ�� ����Ʈ
    public List<AudioClip> runFootstepClips;    // �� �� �߼Ҹ� Ŭ�� ����Ʈ

    [Header("Settings")]
    public float moveSpeed;     // �̵� �ӵ�
    public float dashSpeed;     // �뽬 �� �̵� �ӵ� ���
    public float JumpPower;     // ���� �Ŀ�
    public float climbSpeed;    // ��ٸ� ������ �ӵ�

    private bool isJumping;     // ���� ������ ����
    private bool isOnLadder;    // ��ٸ��� �ִ��� ����
    private bool isMoving;      // �ȴ� ������ ����
    private bool isRunning;     // �޸��� ������ ����
    private Coroutine footstepCoroutine; // �߼Ҹ� ����� ���� �ڷ�ƾ

    // ��ũ��Ʈ�� ó�� Ȱ��ȭ�� �� ȣ��Ǵ� �޼���
    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� �÷��̾� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� �ı�
        }
    }

    void Start()
    {
        // ���콺 Ŀ���� ȭ�鿡 �����ϰ� ������ �ʵ��� ����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ��������
        rb.freezeRotation = true;       // Rigidbody�� ȸ���� �����Ͽ� ���� ���꿡 ������ ���� �ʵ��� ����

        isMoving = false;   // �ȱ� ���� �ʱ�ȭ
        isRunning = false;  // �޸��� ���� �ʱ�ȭ
        isJumping = false;  // ���� ���� �ʱ�ȭ
    }

    void Update()
    {
        HandleJump(); // ���� ó��
    }

    // ���� ������ ���� �޼���
    void FixedUpdate()
    {
        HandleMovement(); // �̵� ó��
        HandleClimbing(); // ��ٸ� ������ ó��
    }

    // �÷��̾� �̵��� ó���ϴ� �޼���
    void HandleMovement()
    {
        // ����, ���� �Է� ���� ������
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // �Է¿� ���� �̵� ���� ���� ���
        Vector3 moveVec = transform.forward * v + transform.right * h;

        // �⺻ �̵� �ӵ� ����
        float currentSpeed = moveSpeed;

        // ���� Shift Ű�� ������ �뽬
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= dashSpeed;
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        // �÷��̾ ������ �� �߼Ҹ� ��� ����
        if (moveVec.magnitude > 0 && !isJumping)
        {
            if (!isMoving)
            {
                isMoving = true;
                // �Ȱų� �޸� �� �߼Ҹ� ����� ���� �ڷ�ƾ ����
                footstepCoroutine = StartCoroutine(PlayFootstepSounds());
            }
        }
        else
        {
            isMoving = false;
            // �÷��̾ �������� ���� �� �߼Ҹ� �ڷ�ƾ ����
            if (footstepCoroutine != null)
            {
                StopCoroutine(footstepCoroutine);
                footstepCoroutine = null;
            }
        }

        // Rigidbody�� �̿��Ͽ� �̵� ó��
        rb.MovePosition(rb.position + moveVec.normalized * currentSpeed * Time.fixedDeltaTime);
    }

    // �÷��̾� ������ ó���ϴ� �޼���
    void HandleJump()
    {
        // �����̽��� �Է� �� ���� ó��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(!isJumping) // �÷��̾ ���� ���� �ƴ� ���� ���� ����
            {
                isJumping = true;
                rb.AddForce(Vector3.up * JumpPower, ForceMode.Impulse); // �������� ���� ���� ����
            }
        }
    }

    // �浹�� �߻��� �� ȣ��Ǵ� �޼��� (Ground �±� Ȯ��)
    public void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� �±װ� "Ground"�� ���
        if (collision.gameObject.CompareTag("Ground"))
         {
            isJumping = false; // ���� ���� �ʱ�ȭ
        }
    }

    // ��ٸ� �����⸦ ó���ϴ� �޼���
    void HandleClimbing()
    {
        // ��ٸ��� ���� ���� ����
        if (isOnLadder)
        {
            rb.useGravity = false; // �߷��� ��
            float climbInput = Input.GetAxisRaw("Vertical"); // ���� �Է� ���� ������

            // W Ű�� ������ �� ���� �̵�
            if (climbInput > 0)
            {
                rb.MovePosition(rb.position + Vector3.up * climbSpeed * Time.fixedDeltaTime);
            }
            // S Ű�� ������ �� �Ʒ��� �̵�
            else if (climbInput < 0)
            {
                rb.MovePosition(rb.position - Vector3.up * climbSpeed * Time.fixedDeltaTime);
            }
        }
    }

    // Ʈ���ſ� ������ �� ȣ��Ǵ� �޼��� (��ٸ����� �浹 ����)
    private void OnTriggerEnter(Collider other)
    {
        // ��ٸ����� �浹�� ����
        if (other.CompareTag("Ladder"))
        {
            isOnLadder = true;
        }
    }

    // Ʈ���ſ��� ���� �� ȣ��Ǵ� �޼��� (��ٸ����� ���)
    private void OnTriggerExit(Collider other)
    {
        // ��ٸ����� ����� ����
        if (other.CompareTag("Ladder"))
        {
            rb.useGravity = true; // �߷��� �ٽ� Ȱ��ȭ
            isOnLadder = false;
        }
    }

    // ���� ���� �� �� �߼Ҹ��� ���� �������� ����ϴ� �ڷ�ƾ
    private IEnumerator PlayFootstepSounds()
    {
        while (isMoving)
        {
            if (isJumping)
            {
                // ���� �߿��� �߼Ҹ��� ������� ����
                yield return null;
                continue;
            }

            AudioClip footstepClip = null;

            // �ٴ� ������ �� �߼Ҹ� ���
            if (isRunning && runFootstepClips.Count > 0)
            {
                // �޸��� �߼Ҹ� ����Ʈ���� �������� Ŭ�� ���� �� ���
                footstepClip = runFootstepClips[Random.Range(0, runFootstepClips.Count)];
                audioManager.PlaySound(footstepClip);
                yield return new WaitForSeconds(0.3f); // �߼Ҹ� ����
            }
            // �ȴ� ������ �� �߼Ҹ� ���
            else if (!isRunning && walkFootstepClips.Count > 0)
            {
                // �ȱ� �߼Ҹ� ����Ʈ���� �������� Ŭ�� ���� �� ���
                footstepClip = walkFootstepClips[Random.Range(0, walkFootstepClips.Count)];
                audioManager.PlaySound(footstepClip);
                yield return new WaitForSeconds(0.6f); // �߼Ҹ� ����
            }
        }
    }
}
