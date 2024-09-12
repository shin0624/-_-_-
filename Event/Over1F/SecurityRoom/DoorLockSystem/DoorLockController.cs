using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorLockController : MonoBehaviour
{
    [Header("Right Password")]
    public int RightPassword; // ������ �� �н����� ����

    [Header("Input Password")]
    public TMP_InputField InputField; // �ؽ�Ʈ�޽������� ��ǲ�ʵ� ��ü

    [Header("Buttons")]
    public List<Button> NumberButtons;//���� ��ư��
    public Button EnterButton; // ���� ��ư

    [Header("Color T/F")]
    public Color TrueColor; // ������ �� ���� �÷�
    public Color FalseColor; // ������ �ƴ� ���� �÷�
    public Color BasicColor; // �⺻ �÷�

    [Header("Sounds")]
    public AudioSource ButtonSound;//��ư Ŭ����
    public AudioSource CorrectSound;//������
    public AudioSource ErrorSound;//������

    private string InputPassword = "";//��й�ȣ �ʱ�ȭ ��
    public float characterSpacing; // ���ϴ� �ڰ� ��
    public bool OpenDoorFlag;//��й�ȣ�� ��Ȯ�� �Է��Ͽ� ���� ���ȴ��� ����

    public static event System.Action OnDoorLockClosed;//����� ui�� �����̺�Ʈ�� ����->ContactController���� �����Ͽ� ui�� Ȱ��/��Ȱ���� ó��
    //System.Action�� ��ȯ���� �Ű������� ���� �޼���

    void Start()
    {
        OpenDoorFlag = false;
       
        SetInputFieldColor(BasicColor);// ��ǲ�ʵ��� �ʱ� ���� ����
        InputField.textComponent.characterSpacing = characterSpacing; // �ʱ� �ڰ� ����

        foreach (var button in NumberButtons) // ��ư ��ü �Ʒ��� �ִ� TMP �ؽ�Ʈ�� �޾ƿ�, ���������� ���� ����ȯ.
        {
            int number = int.Parse(button.GetComponentInChildren<TextMeshProUGUI>().text);
            
            button.onClick.AddListener(() => AppendNumber(number)); // ��ư���� �����ʸ� ���. ���ٽ����� ������ �ۼ�
        }
        EnterButton.onClick.AddListener(EnterButtonClicked); // ���͹�ư ������
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))//ui�� Ȱ��ȭ �� ������ escŰ �Է��� üũ��.
        {
            OnDoorLockClosed?.Invoke();//escŰ�� ������ �̺�Ʈ ȣ��->�̺�Ʈ�� �����ڰ� ������ Invoke�޼��� ȣ��
            //NULL ���Ǻ� �����ڸ� ����Ͽ� �� �������� ����
        }
    }

    void AppendNumber(int number)
    {
        if (InputPassword.Length < 4)
        {
            ButtonSound.Play();
            InputPassword += number.ToString(); // ����Ÿ���� number�� string���� ����ȯ
            
            InputField.text = InputPassword; // ��ǲ�ʵ� ��ü�� �ۼ��Ǵ� text�� string��ȯ�� �н������ ��ü
            
        }
    }

    void EnterButtonClicked()
    {
        if (EnterButton != null)
        {
            int inputPasswordValue;
            bool isNumeric = int.TryParse(InputPassword, out inputPasswordValue); // string���� ����üũ �� ���������� ����
            if (isNumeric && inputPasswordValue == RightPassword)
            {
                OpenDoorFlag = true;
                Debug.Log($"OpenDoorFlag = {OpenDoorFlag}");
                CorrectSound.Play();
                SetInputFieldColor(TrueColor);//���� �÷�(���)���� ����
                InputField.textComponent.characterSpacing = 23.0f;//���� ����̹Ƿ� �ڰ� ����
                InputField.text = "OPEN DOOR";//���� �޼��� ���
                
                StartCoroutine(OpenDoorAfterDelay(0.7f));//0.5�� �� UI ����
            }
            else
            {
                OpenDoorFlag = false;
                ErrorSound.Play();
                SetInputFieldColor(FalseColor);//���� �÷�(������)���� ����
                InputField.textComponent.characterSpacing = 23.0f;//���� ����̹Ƿ� �ڰ� ����
                InputField.text = "ERROR";//���� �޼��� ���
                StartCoroutine(ResetColorAfterDelay(0.7f)); // ������ ������� �ǵ����� ���� �ڷ�ƾ ȣ��
            }
        }
    }

    void SetInputFieldColor(Color color)//���÷��� �÷� ������ ���� �޼���
    {
        var colors = InputField.colors;
        colors.normalColor = color;
        InputField.colors = colors;
    }

    IEnumerator ResetColorAfterDelay(float delay)// ��й�ȣ ���� �� ERROR ��� + ������ ���÷��� ��� �� �ٽ� �ʱ�ȭ
    {
        yield return new WaitForSeconds(delay);//������ �ð���ŭ ��� �� �÷� �ʱ�ȭ
        SetInputFieldColor(BasicColor); // �̹��� �÷� �ʱ�ȭ
        InputField.textComponent.characterSpacing = characterSpacing;
        InputPassword = ""; // ��й�ȣ �ʱ�ȭ
        InputField.text = "0000"; // �ؽ�Ʈ �ʱ�ȭ
    }

    IEnumerator OpenDoorAfterDelay(float delay)//��й�ȣ ���� �� OPEN DOOR ��� + ��� ���÷��� ��� �� UI ����
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        OnDoorLockClosed?.Invoke();
    }
}
