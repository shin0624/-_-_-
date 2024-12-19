using UnityEngine;

// �� ��Ȳ�� �´� ���̾�α׸� ����� �� �ֵ��� �߻� Ŭ������ ����. �̷��� �ϸ� �ٸ� ��ũ��Ʈ���� ���ϴ� ������ �޼��带 �������̵��Ͽ� ���̾�α� ��� ����
public abstract class DialogController : MonoBehaviour
{
    protected DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        OnStart();
    }

    void Update()
    {
            // StartDialogue();
            OnUpdate();
    }

    protected abstract void OnStart();
    protected abstract void OnUpdate();

    protected void StartDialogue(string dialogueID)// �������̵��� Ŭ�������� Ư�� ��Ȳ�� dialogueID�� �Է��ϸ� �׿� �´� ��� ���.
    {
        dialogueManager.DisplayDialogue(dialogueID);
    }
}
