/*
 * FlashlightController.cs
 * �� ��ũ��Ʈ�� �������� ���¸� �����ϸ�,
 * �÷��̾ �������� �Ѱ� �� �� ���带 ���
 * F Ű�� ���� �������� ���¸� ���
 */
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [Header("Scripts")]
    public AudioManager audioManager;

    [Header("Flashlight")]
    public Light flashlight;    // �������� Light ������Ʈ

    private bool isOn = true;   // �������� �ʱ� ���� (����/����)

    // ������ ����
    [Header("Sounds")]
    public AudioClip flashlightOn;  // ������ ���� ���� Ŭ��
    public AudioClip flashlightOff; // ������ ���� ���� Ŭ��

    void Start()
    {
        // �������� �ʱ� ���¸� ����
        flashlight.enabled = isOn;
    }

    void Update()
    {
        // F Ű�� ������ �� ������ ���¸� ����
        if (Input.GetKeyDown(KeyCode.F))
        {
            // ������ ���¿� ���� ������ ���带 ���
            if (!isOn)
            {
                audioManager.PlaySound(flashlightOn);   // ������ ���� ���� ���
            }
            else
            {
                audioManager.PlaySound(flashlightOff);  // ������ ���� ���� ���
            }

            // ������ ���¸� ���
            isOn = !isOn;
            flashlight.enabled = isOn;
        }
    }
}
