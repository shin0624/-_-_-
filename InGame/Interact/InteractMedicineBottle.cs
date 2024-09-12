/*
 * InteractMedicineBottle.cs
 * �� ��ũ��Ʈ�� �ິ ������Ʈ�� ��ȣ�ۿ��� �� �߻��ϴ� ������ ����
 * �÷��̾ �ິ�� ��ȣ�ۿ��� �� ���� �߰��ǰ�, ���尡 ����Ǹ�, �ິ ������Ʈ�� �ı�
 */
using UnityEngine;

public class InteractMedicineBottle : Interactable
{
    [Header("Scripts")]
    public AudioManager audioManager;
    public SanityManager sanityManager;

    [Header("Sounds")]
    public AudioClip pickupItemMedicine; // �� �Ⱦ� ����

    // ��ȣ�ۿ��� �߻����� �� ȣ��Ǵ� �޼���
    // �÷��̾ �ິ�� ��ȣ�ۿ��� �� ����
    public override void Interact()
    {
        if (sanityManager != null)
        {
            // SanityManager�� AddMedicine �޼��带 ȣ���Ͽ� ���� �߰�
            sanityManager.AddMedicine();
        }

        // AudioManager�� ����� �� �Ⱦ� ���� ���
        audioManager.PlaySound(pickupItemMedicine);

        // ��ȣ�ۿ� �� ������Ʈ �ı�
        Destroy(gameObject);
    }
}
