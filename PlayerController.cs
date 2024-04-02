using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _speed = 10.0f;

    void Update()
    {
        OnKeyboard();
    }

    void OnKeyboard()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            moveDirection += Vector3.forward;
        if (Input.GetKey(KeyCode.S))
            moveDirection += Vector3.back;
        if (Input.GetKey(KeyCode.A))
            moveDirection += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            moveDirection += Vector3.right;

        // �÷��̾ �̵���Ű�� ȸ������ ī�޶��� ȸ�������� ����
        if (moveDirection != Vector3.zero)
        {
            transform.position += moveDirection * Time.deltaTime * _speed;
            transform.rotation = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f);
        }
    }
}