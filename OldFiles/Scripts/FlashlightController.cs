using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public GameObject flashlight;  // ������ ������Ʈ
    private bool isFlashlightOn = false;  // �������� ���� �ִ��� ����

    private Transform playerTransform; // �÷��̾��� Transform�� ������ ����

    void Start()
    {
        if (flashlight != null)
        {
            flashlight.SetActive(isFlashlightOn);  // �ʱ� ���·� ������ ��Ȱ��ȭ
            playerTransform = transform.parent;    // �������� �θ��� Transform�� ����
        }
    }

    void Update()
    {
        if (flashlight != null && playerTransform != null)
        {
            // �÷��̾� ��ġ�� ȸ���� ���� ������ ��ġ �� ȸ�� ������Ʈ
            flashlight.transform.position = playerTransform.position;
            flashlight.transform.rotation = playerTransform.rotation;
        }

        ToggleFlashlight();
    }

    void ToggleFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F))  // FŰ�� ������ ������ �ѱ�/����
        {
            isFlashlightOn = !isFlashlightOn;
            flashlight.SetActive(isFlashlightOn);  // ������ Ȱ��ȭ/��Ȱ��ȭ
        }
    }
}
