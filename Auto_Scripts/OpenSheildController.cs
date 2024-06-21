using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShieldController : MonoBehaviour
{
    public GameObject cup; // ���� ������Ʈ
    private bool isOpen = false;//������ ���ȴ��� ����
    private GameObject player;

    public bool leverRaised = false;//�����̰� �ö󰬴��� ����
    public GameObject Knob;//������ ������Ʈ

    [SerializeField]
    public float leverRaiseDigit;

    private void Start()
    {
        player = GameObject.FindWithTag("PLAYER"); // �±׸� ����Ͽ� �÷��̾� ��ü�� ã�´�.
        if (player == null)
        {
            Debug.LogError("Player object not found. Make sure the player object has the 'Player' tag.");
        }
    }

    private void Update()
    {
        if (player != null && !isOpen)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance < 3.0f && Input.GetKeyDown(KeyCode.E)) // �÷��̾ ������ ������ �ְ� E Ű�� �����ٸ�
            {
                OpenCup();
                
            }
        }
        else if(isOpen && !leverRaised)//�������� �����ְ� eŰ�� �����ٸ�
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                LeverUp();
                
            }
            
        }
    }

    private void OpenCup()
    {
        if (!isOpen)
        {
            Vector3 newRotation = cup.transform.eulerAngles;
            newRotation.x = cup.transform.eulerAngles.x;
            newRotation.y =  newRotation.y + 150.0f;
            newRotation.z = cup.transform.eulerAngles.z;

            cup.transform.eulerAngles = newRotation;
            isOpen = true;
            Debug.Log("Shield cup opened!");
        }
    }

    private void LeverUp()
    {             
        if(isOpen)
        {
            leverRaised = true;//������ �ö� ���·� ����
            Vector3 newRotation_K = Knob.transform.eulerAngles;
            newRotation_K.x = newRotation_K.x + 111.0f;
            newRotation_K.y = Knob.transform.eulerAngles.y;
            newRotation_K.z = Knob.transform.eulerAngles.z;

            Knob.transform.eulerAngles = newRotation_K;
            
            Debug.Log("lever up!");
        }
        
    }

    public void ResetLever()
    {
        leverRaised = false;
        
    }
}
