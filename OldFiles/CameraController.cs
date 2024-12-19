using System.Collections;
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

    //--------���ݹ����� ī�޶� ��鸮�� ��� ���� �ڵ�---------------
    private float shakeDuration = 0.5f;//��鸲 ���ӽð�
    private float shakeAmount = 0.7f;//��鸲 ����
    private Vector3 originalPos;
    private float noiseOffsetX;//�޸�������� ���� �߰�-->�������� ������ �����ϴ� �޸������ ����Ͽ� �ڷ�ƾ �������� Random.Range ��뺸�� �ڿ������� ��鸲 ���� ����
    private float noiseOffsetY;
    void Start()
    {
        cam.transform.localPosition = new Vector3(0, 0.5f, -0.1f); // ī�޶� �÷��̾� �ڷ� �̵�
        cam.transform.localRotation = Quaternion.identity; // ī�޶��� ȸ���� �ʱ�ȭ
        originalPos = cam.transform.localPosition;//ī�޶� ���� ��ġ
        //�޸������� ������ ���� ������ �������� ����
        noiseOffsetX = Random.Range(0f, 1000f);
        noiseOffsetY = Random.Range(0f,1000f);
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

    public void StartShake()// ������ ������ ī�޶� ��鸲�� ����. DecreaseSanity()�� ȣ��� �� ȣ��.
    {
        StopAllCoroutines();//���� ��鸲�� �ִٸ� ������.
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0.0f;
        while(elapsed < shakeDuration)
        {
            elapsed+=Time.deltaTime;
            float percentComplete = elapsed / shakeDuration;//��鸲�� �Ϸ�Ǵ� �ð�
            float damper = 1.0f - Mathf.Clamp(percentComplete, 0.0f, 1.0f);//�ð��� ���� ���� ��鸲�� �ڿ������� �����ϵ��� �Ѵ�.
            
            //�޸� ����� ����� ��鸲 ����. elapsed*10�� ����Ͽ� �������� �ӵ��� �����Ѵ�. �� ���� ������ �� ���� ��鸲�� ������ �� ������.
            float offsetX = Mathf.PerlinNoise(noiseOffsetX + elapsed*10f, 0f );
            float offsetY = Mathf.PerlinNoise(0f, noiseOffsetY + elapsed * 10f);

            //1-���� 1 ������ ������ ��ȯ
            offsetX = (offsetX * 2.0f - 1.0f) * shakeAmount * damper;
            offsetY = (offsetY * 2.0f - 1.0f) * shakeAmount * damper;
            cam.transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);//��鸰 ��ġ�� ī�޶� ���������ǿ� ����
            
            yield return null;
        }
        cam.transform.localPosition = originalPos;//ī�޶� ���� ��ġ�� �̵�.
    }
}
