using UnityEngine;

public class InteractMedicineBottle : Interactable
{
    // SanityManager�� ����
    public SanityManager sanityManager;
    
    // �� �Ⱦ� ����
    public AudioClip pickupItemMedicine;


    public override void Interact()
    {
        if (sanityManager != null)
        {
            sanityManager.AddMedicine(); // AddMedicine ȣ��
        }

        AudioManager.Instance.PlaySound(pickupItemMedicine); // AudioManager�� ���� ���� ���

        Destroy(gameObject); // ������Ʈ �ı�
    }
}
