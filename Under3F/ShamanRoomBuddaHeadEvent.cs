using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanRoomBuddaHeadEvent : MonoBehaviour
{
    //���� ������ �̺�Ʈ : �÷��̾ ����� �������� ��ó �Ӹ��� ������
    [SerializeField] private GameObject BuddaHead; // ��ó �Ӹ� ������Ʈ
    [SerializeField] private AudioSource RollingSound; //�������� �Ҹ�
    private Rigidbody BuddaHeaRB;//��ó �Ӹ��� ������ٵ�
    private float Rbforce = 1.50f;// �� ���ϴ� ����
    private bool HasRolled = false;//�̺�Ʈ�� �ѹ��� �߻��ϵ��� �����ϴ� Ʈ����
    
    void Start()
    {
        BuddaHeaRB = BuddaHead.GetComponent<Rigidbody>();
        RollingSound = BuddaHead.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HasRolled) return;//�̹� �̺�Ʈ�� �߻��ߴٸ� �ߺ�������� ����

        if(other.CompareTag("Player"))
        {
            BuddaHeaRB.AddForce(Vector3.right * Rbforce, ForceMode.Impulse);//x�������� Impulse��� ���� ���Ѵ�. ĸ�� �ö��̴��� ������ ��ó�Ӹ� ������Ʈ�� �������� ������ ��ó�� ���� ��.
            RollingSound.Play();
            RollingSound.loop = false;
            HasRolled= true;
        }
    }
}
