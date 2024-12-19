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

    public VideoPlayer vdo;//���� �÷��̾�
    public AudioSource ado;//������ҽ�
    private VisualElement root;

    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;//��ư��
        

        StartButton = root.Q<Button>("button-start");
        ExitButton = root.Q<Button>("button-exit");

        StartButton.clicked += StartButtonClicked;
        ExitButton.clicked += ExitButtonClicked;

        vdo.loopPointReached += OnVideoFinished;// �����÷��̾ ����Ǿ����� �˱� ���� looppointreached�̺�Ʈ�� �ڵ鷯 �߰�
        
    }

     void OnDisable()
    {
        StartButton.clicked -= StartButtonClicked;
        ExitButton.clicked -= ExitButtonClicked;
        vdo.loopPointReached -= OnVideoFinished;//�ڵ鷯 ����
    }

    private void StartButtonClicked()
    {
        root.style.display = DisplayStyle.None; // ��ŸƮ ��ư Ŭ�� �� ui ��Ȱ��ȭ.
        vdo.Prepare();//���� �غ� ����
        vdo.prepareCompleted += OnVideoPrepared;//���� �غ� �Ϸ� �̺�Ʈ�� �ڵ鷯 �߰�
        
     
    }
    
    private void OnVideoPrepared(VideoPlayer soucre )//������ �غ�Ǹ� ����, ����� ���
    {
        vdo.prepareCompleted -= OnVideoPrepared;
        vdo.Play();
        ado.Play();
    }

    // ��ŸƮ ��ư Ŭ�� �� �ƽ� ���� ��� �� ���� ������ ����.
    private void OnVideoFinished(VideoPlayer vdo)
    {
        ado.Stop();//����� ����
        SceneManager.LoadScene("UndergroundScene");
    }

    private void ExitButtonClicked()
    {
        Application.Quit();
    }


}
