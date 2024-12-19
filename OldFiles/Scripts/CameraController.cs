using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;  // �÷��̾��� Transform
    public Camera cam;        // ī�޶�
    public float mouseSpeed = 100f; // ���콺 �̵� �ӵ�
    public float rotationSmoothTime = 0.1f; // ȸ�� �ε巯�� ���� �ð�

    private float xRotation = 0f;  // ī�޶��� ���� ȸ�� ����
    private float yRotation = 0f;  // ī�޶��� ���� ȸ�� ����
    private float currentXRotation = 0f; // ���� ī�޶��� ���� ȸ�� ��
    private float currentYRotation = 0f; // ���� ī�޶��� ���� ȸ�� ��
    private float xRotationVelocity = 0f; // ���� ȸ�� �� ��ȭ �ӵ�
    private float yRotationVelocity = 0f; // ���� ȸ�� �� ��ȭ �ӵ�

    void Start()
    {
        // ī�޶� �÷��̾��� �ڽ����� ����
        cam.transform.SetParent(player);
        cam.transform.localPosition = new Vector3(0, 0, -0.1f); // ī�޶� �÷��̾� �ڷ� �̵�
        cam.transform.localRotation = Quaternion.identity; // ī�޶��� ȸ���� �ʱ�ȭ
    }

    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSpeed * Time.deltaTime;

        // ��ǥ ȸ�� �� ���
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // ���� ȸ�� ���� -90������ 90�� ���̷� ����

        // ���� ȸ�� ���� ��ǥ ȸ�� ������ �ε巴�� �̵�
        currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationVelocity, rotationSmoothTime);
        currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationVelocity, rotationSmoothTime);

        // ī�޶��� ȸ�� ����
        cam.transform.localRotation = Quaternion.Euler(currentXRotation, 0, 0);
        player.transform.rotation = Quaternion.Euler(0, currentYRotation, 0); // �÷��̾� ȸ�� ����
    }
}
