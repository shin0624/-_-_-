using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam; // �÷��̾� ī�޶�
    public float interactDistance = 3f; // ��ȣ�ۿ� ������ �Ÿ�
    public string interactableTag = "Interactable"; // ��ȣ�ۿ� ������ ������Ʈ�� �±� ����
    public GameObject focusUI; // ��Ŀ�� UI
    public GameObject interactUI; // ��ȣ�ۿ� UI (E Ű ������)

    void Update()
    {
        RaycastHit hit;
        // ī�޶� �������� ����ĳ��Ʈ
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactDistance))
        {
            if (hit.collider.CompareTag(interactableTag))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    // ��ȣ�ۿ� ����
                    UpdateUI(true);

                    // E Ű�� ������ �� ��ȣ�ۿ� ����
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactable.Interact();
                    }
                }
            }
            else
            {
                // ��ȣ�ۿ� �Ұ�
                UpdateUI(false);
            }
        }
        else
        {
            // �⺻ ����
            UpdateUI(false);
        }
    }

    private void UpdateUI(bool isInteracting)
    {
        focusUI.SetActive(!isInteracting);
        interactUI.SetActive(isInteracting);
    }
}
