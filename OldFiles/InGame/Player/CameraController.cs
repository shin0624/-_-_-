/*
 * CameraController.cs
 * �� ��ũ��Ʈ�� �÷��̾�� ī�޶��� ȸ���� ����
 * ���콺 �Է¿� ���� ī�޶��� ���� �� ���� ȸ���� �����ϸ�,
 * �ε巯�� ȸ���� ���� `Mathf.SmoothDamp`�� ���
 */
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Player")]
    public Transform player;  // �÷��̾��� Transform, ī�޶� ���� ��� 

    [Header("Camera")]
    public Camera cam;        // �÷��̾� ī�޶�

    [Header("Settings")]
    public float mouseSpeed = 100f;         // ���콺 �̵� �ӵ�, ī�޶� ȸ�� �ӵ� ����
    public float rotationSmoothTime = 0.1f; // ȸ�� ���� �ε巯�� ������ ���� �ð�

    // ī�޶��� ȸ�� ���� �����ϴ� ������
    private float xRotation = 0f;           // ī�޶��� ���� ȸ�� ����
    private float yRotation = 0f;           // ī�޶��� ���� ȸ�� ����
    private float currentXRotation = 0f;    // ���� ī�޶��� ���� ȸ�� ��
    private float currentYRotation = 0f;    // ���� ī�޶��� ���� ȸ�� ��
    private float xRotationVelocity = 0f;   // ���� ȸ�� �� ��ȭ �ӵ�
    private float yRotationVelocity = 0f;   // ���� ȸ�� �� ��ȭ �ӵ�

    void Start()
    {
        // ī�޶��� �ʱ� ��ġ�� ȸ���� ����
        // cam.transform.SetParent(player); // ī�޶� �÷��̾��� �ڽ����� ����
        cam.transform.localPosition = new Vector3(0, 0.5f, -0.1f);  // ī�޶� �÷��̾� �ڷ� �̵���Ŵ
        cam.transform.localRotation = Quaternion.identity;          // ī�޶��� ȸ���� �ʱ�ȭ�Ͽ� �⺻ ���·� ����
    }

    void Update()
    {
        // �� �����Ӹ��� ī�޶� ȸ�� ó��
        Rotate();
    }

    // ī�޶�� �÷��̾��� ȸ���� ������Ʈ�ϴ� �޼���
    void Rotate()
    {
        // ���콺 �Է��� �޾Ƽ� ī�޶� ȸ�� �� ���
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSpeed * Time.deltaTime;

        // ��ǥ ȸ�� ���� ������Ʈ
        yRotation += mouseX;
        xRotation -= mouseY;

        // ���� ȸ�� ���� -90������ 90�� ���̷� �����Ͽ� ī�޶� �Ѿ�� �ʵ��� ��
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // ���� ȸ�� ���� ��ǥ ȸ�� ������ �ε巴�� �̵�
        currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationVelocity, rotationSmoothTime);
        currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationVelocity, rotationSmoothTime);

        // ī�޶��� ȸ���� ����
        cam.transform.localRotation = Quaternion.Euler(currentXRotation, 0, 0);

        // �÷��̾��� ȸ���� ����
        player.transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
    }
}
