/*
 * PersistentUI.cs
 * �� ��ũ��Ʈ�� UI ��Ұ� ���� �� ���� ���������� �����ǵ��� ��
 * �̱��� ������ ����Ͽ� UI ��Ұ� ���� ������ �ߺ����� �ʵ��� �ϸ�,
 * �� ��ȯ �ÿ��� UI ������Ʈ�� �ı����� �ʵ��� ����
 */
using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    // �̱��� �ν��Ͻ� ����
    private static PersistentUI instance;

    // ��ũ��Ʈ�� ó�� Ȱ��ȭ�� �� ȣ��Ǵ� �޼���
    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� UI ����
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� �ı�
        }
    }
}
