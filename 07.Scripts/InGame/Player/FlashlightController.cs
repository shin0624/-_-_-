using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    // ������
    public Light flashlight;

    // �������� �ʱ� ���� (����/����)
    private bool isOn = true;

    // ������ ����
    [Header("Sounds")]
    public AudioClip flashlightOn;
    public AudioClip flashlightOff;

    void Start()
    {
        // �ʱ� ���� ����
        flashlight.enabled = isOn;
    }

    void Update()
    {
        // F Ű�� ������ �������� ���¸� �����մϴ�.
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isOn)
            {
                AudioManager.Instance.PlaySound(flashlightOn); // AudioManager�� ���� ���� ���
            }
            else
            {
                AudioManager.Instance.PlaySound(flashlightOff); // AudioManager�� ���� ���� ���
            }
            isOn = !isOn;  // ������ ���� ���
            flashlight.enabled = isOn;
        }
    }
}
