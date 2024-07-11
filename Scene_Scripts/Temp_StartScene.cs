using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class Temp_StartScene : MonoBehaviour
{
    private Button StartButton;//��ŸƮ ��ư
    private Button ExitButton;// �ͽ�Ʈ ��ư
    private Button CreditButton;//ũ���� ��ư

    public VideoPlayer vdo;//���� �÷��̾�
    public AudioSource ado;//������ҽ�
    public AudioSource ado2;// �⺻ �����

    private VisualElement root;

    private void Start()
    {
        ado2.Play();
    }

    void Awake()
    {
        // UIDocument �� AudioSource �ʱ�ȭ Ȯ��
        if (GetComponent<UIDocument>() == null)
        {
            Debug.LogError("UIDocument component not found!");
            return;
        }

        if (vdo == null)
        {
            Debug.LogError("VideoPlayer component not assigned!");
            return;
        }

        if (ado == null)
        {
            Debug.LogError("AudioSource component not assigned!");
            return;
        }
        if (ado2 == null)
        {
            Debug.LogError("AudioSource component not assigned!");
            return;
        }
    }


    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;//��ư��
        

        StartButton = root.Q<Button>("button-start");
        ExitButton = root.Q<Button>("button-exit");
        CreditButton = root.Q<Button>("button-credits");

        StartButton.clicked += StartButtonClicked;
        ExitButton.clicked += ExitButtonClicked;
        CreditButton.clicked += CreditButtonClicked;

        vdo.loopPointReached += OnVideoFinished;// �����÷��̾ ����Ǿ����� �˱� ���� looppointreached�̺�Ʈ�� �ڵ鷯 �߰�
        
    }

     void OnDisable()
    {
        StartButton.clicked -= StartButtonClicked;
        ExitButton.clicked -= ExitButtonClicked;
        CreditButton.clicked -= CreditButtonClicked;

        vdo.loopPointReached -= OnVideoFinished;//�ڵ鷯 ����
    }

    private void StartButtonClicked()
    {
        root.style.display = DisplayStyle.None; // ��ŸƮ ��ư Ŭ�� �� ui ��Ȱ��ȭ.
        vdo.Prepare();//���� �غ� ����
        vdo.prepareCompleted += OnVideoPrepared;//���� �غ� �Ϸ� �̺�Ʈ�� �ڵ鷯 �߰�
    }
    
    private void OnVideoPrepared(VideoPlayer vdo)//������ �غ�Ǹ� ����, ����� ���
    {
        ado2.Stop();
        vdo.prepareCompleted -= OnVideoPrepared;
        vdo.Play();
        ado.Play();
    }

    // ��ŸƮ ��ư Ŭ�� �� �ƽ� ���� ��� �� ���� ������ ����.
    private void OnVideoFinished(VideoPlayer vdo)
    {
        ado.Stop();//����� ����
        SceneManager.LoadScene("1_2FloorScene");
    }

    private void ExitButtonClicked()
    {
        Application.Quit();
    }
    private void CreditButtonClicked()
    {
        SceneManager.LoadScene("CreditScene");
    }


}
