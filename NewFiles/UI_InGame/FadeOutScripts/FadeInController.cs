using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInController : MonoBehaviour
{
    //������ ����� �ִϸ��̼ǿ� ���� ���̵�ƿ� ��ũ��Ʈ. image�� ���İ��� ������Ű�� ���̵��� ȿ���� �ش�.
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        Color color = image.color;
        color.a = 1f;
        image.color = color;
    }

    private void Update()
    {
        Color color = image.color;

        if (color.a > 0)//���İ��� 0 �̻��̸� ���İ� 0���� ����. �� ȭ������ ������ ���ϴ� ���̵��� ����
        {
            color.a -= Time.deltaTime / 2f;//���̵� �� �ӵ� ����
            image.color = color;
        }

        
    }
}
