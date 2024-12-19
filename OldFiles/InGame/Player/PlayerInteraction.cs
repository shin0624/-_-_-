/*
 * PlayerInteraction.cs
 * �� ��ũ��Ʈ�� �÷��̾ ��ȣ�ۿ� ������ ������Ʈ�� ��ȣ�ۿ��� ó��
 * ī�޶� ������ �������� ����ĳ��Ʈ�� ����Ͽ� ��ȣ�ۿ� ������ ������Ʈ�� Ž���ϰ�,
 * ��ȣ�ۿ� UI�� �����ϸ�, E Ű�� ������ �� ��ȣ�ۿ��� ����
 */
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Camera")]
    public Camera cam; // �÷��̾� ī�޶�
    
    [Header("UI Elements")]
    public GameObject focusUI;      // ��Ŀ�� UI
    public GameObject interactUI;   // ��ȣ�ۿ� UI

    [Header("Settings")]
    public float interactDistance = 3f;             // ��ȣ�ۿ� ������ �ִ� �Ÿ��� ����
    public string interactableTag = "Interactable"; // ��ȣ�ۿ� ������ ������Ʈ�� ������ �±�

    void Update()
    {
        RaycastHit hit;
        // �÷��̾� ī�޶��� ��ġ���� ���� �������� ����ĳ��Ʈ
        // ����ĳ��Ʈ�� ������ �Ÿ� (interactDistance) ������ ��ȣ�ۿ� ������ ������Ʈ�� Ž��
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactDistance))
        {
            // ����ĳ��Ʈ�� ��Ʈ�� ������Ʈ�� ��ȣ�ۿ� ������ �±׸� ������ �ִ��� Ȯ��
            if (hit.collider.CompareTag(interactableTag))
            {
                // ��Ʈ�� ������Ʈ���� Interactable ������Ʈ�� ã��
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    // ��ȣ�ۿ� ������ ������Ʈ�� ��Ŀ���� �������� ��� UI�� ������Ʈ
                    UpdateUI(true);

                    // �÷��̾ E Ű�� ������ �� ��ȣ�ۿ� �޼��带 ȣ��
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactable.Interact(); // Interactable ������Ʈ�� Interact �޼��带 ȣ��
                    }
                }
            }
            else
            {
                // ����ĳ��Ʈ�� ��ȣ�ۿ� ������ �±װ� �ƴ� ������Ʈ�� ��Ʈ�� ��� UI�� ������Ʈ
                UpdateUI(false);
            }
        }
        else
        {
            // ����ĳ��Ʈ�� �ƹ� �͵� ��Ʈ���� ���� ��� UI�� �⺻ ���·� ������Ʈ
            UpdateUI(false);
        }
    }

    // UI ��Ҹ� ������Ʈ�Ͽ� ��ȣ�ۿ� ���¿� ���� ��Ŀ�� UI�� ��ȣ�ۿ� UI�� ��ȯ
    private void UpdateUI(bool isInteracting)
    {
        // ��ȣ�ۿ� ���̸� ��ȣ�ۿ� UI, �ƴϸ� ��Ŀ�� UI�� ǥ��
        focusUI.SetActive(!isInteracting);
        interactUI.SetActive(isInteracting);
    }
}
