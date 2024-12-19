using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //�÷��̾��� �Է��� �����ϰ� Ư�� Ű�� ������ �� �̺�Ʈ�� �߻���Ű�� �Ŵ���
    // �÷��̾� �Է��� update���� ������ ���ʿ��� �ݺ� ������ ����ǹǷ�, �̸� �����ϰ� ȿ������ �̺�Ʈ ��� ó���� ����

    public static event Action OnInteractKeyPressed;// Action ��������Ʈ Ÿ������ ����. E Ű�� ������ �� �߻��ϸ�, �ٸ� ��ũ��Ʈ���� �� �̺�Ʈ�� �����ϰ� Ư�� �ൿ ���� ����

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            
            OnInteractKeyPressed?.Invoke();//EŰ �Է��� �����Ͽ� �̺�Ʈ �߻�. �����ڰ� �ִ� ��쿡�� �̺�Ʈ ȣ��
        }
    }
}
