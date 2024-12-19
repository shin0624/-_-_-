using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FadeOutController : MonoBehaviour
{
    //2�� �⵵�� �ִϸ��̼ǿ� ���� ���̵�ƿ� ��ũ��Ʈ. image�� ���İ��� ���ҽ�Ű�� ���̵�ƿ� ȿ���� �ش�.
    private Image image;
    private void Awake()
    {
       image = GetComponent<Image>();
    }

    private void Update()
    {
        Color color = image.color;

        if(color.a < 1)//���İ��� 1 �����̸� ���İ� ~255���� ����. ���� ȭ������ ������ ���ϴ� ���̵�ƿ� ����
        {
            color.a+=Time.deltaTime;
        }
     
        image.color = color;
    }
}
