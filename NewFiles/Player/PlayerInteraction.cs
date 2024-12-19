using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam; // �÷��̾� ī�޶�
    public float interactDistance = 6f; // ��ȣ�ۿ� ������ �Ÿ�
    public string interactableTag = "Interactable"; // ��ȣ�ۿ� ������ ������Ʈ�� �±� ����
    public GameObject focusUI; // ��Ŀ�� UI
    public GameObject interactUI; // ��ȣ�ۿ� UI (E Ű ������)

    private void Start()
    {
        focusUI = GameObject.Find("RawImage_Focus");
        interactUI = GameObject.Find("RawImage_ButtonE");

        focusUI.SetActive(true);
        interactUI.SetActive(false);
    }

    //�����ؾ� �� ���� : focusUI, InteractUI �� �� �ϳ��� ������(��ȣ�ۿ� ������Ʈ�� ��ó�� ������) ���� �ϳ��� ������ ������ ���
    //������Ʈ���� �ۼ��Ȱ� �̺�Ʈ Ʈ���ŷ� ������, �ٸ� ������� �Űܾ� ��.


    //��������
    // 1. ���� �α� ���� --> ui �� ���ϳ��� null�� �� �����α� ��� x (�ݺ��Ǵ� �α������ ���ɿ� ����)
    // 2. �ǵ������� null���¸� ����Ͽ� setactive()�� ȣ������ �ʰ� ��
    // 3. nullüũ ����

    void Update()
    {
        RaycastHit hit;

        // ī�޶� �������� ����ĳ��Ʈ
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactDistance))
        {
            // ��ȣ�ۿ� ������ �±����� Ȯ��
            if (hit.collider.CompareTag(interactableTag))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                // ��ȣ�ۿ� ������ ������Ʈ�� �ִٸ�
                if (interactable != null)
                {
                    
                    // ��Ŀ�� UI ��Ȱ��ȭ, ��ȣ�ۿ� UI Ȱ��ȭ
                    if (focusUI != null && interactUI != null)
                    {
                        focusUI.SetActive(false);
                        interactUI.SetActive(true);
                    }

                    // E Ű �Է� �� ��ȣ�ۿ� ����
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactable.Interact();
                    }
                }
            }
            else
            {
                // ��ȣ�ۿ��� ������Ʈ�� ��ó�� ���� ��, UI �ʱ� ���·� ����
                if (focusUI != null && interactUI != null)
                {
                    focusUI.SetActive(true);
                    interactUI.SetActive(false);
                }
            }
        }
        else
        {
            // ����ĳ��Ʈ�� �ƹ��͵� ������ �ʾ��� �� UI �ʱ� ���·� ����
            if (focusUI != null && interactUI != null)
            {
                focusUI.SetActive(true);
                interactUI.SetActive(false);
            }
        }
    }
}