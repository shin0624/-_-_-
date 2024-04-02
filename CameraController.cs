using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    GameObject _player = null;

    // ī�޶�� �÷��̾� ������ �ʱ� ������ (���� �ʿ�)
    Vector3 offset = new Vector3(0.0f, 1.5f, -3.0f);

    void LateUpdate()
    {
        if (_player != null)
        {
            // �÷��̾��� ��ġ�� �������� ���� ī�޶��� ��ġ�� ����
            transform.position = _player.transform.position + offset;

            // ī�޶� �÷��̾ �ٶ󺸵��� ȸ��
            transform.LookAt(_player.transform);
        }
    }
}