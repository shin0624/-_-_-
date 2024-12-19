using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseUICONTROLLER_UITOOLKIT : MonoBehaviour
{
    //�Ͻ����� ui ���� ��ũ��Ʈ. 
    private Button BackButton;//�� ��ư
    private Button SettingsButton;// ���� ��ư
    private Button ExitButton;//�ͽ�Ʈ ��ư

    private VisualElement root;
    private bool isPaused = false;

    private void Awake()
    {
        if (GetComponent<UIDocument>() == null)
        {
            Debug.LogError("UIDocument component not found!");
            return;
        }
    }

    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;//��ư��


        BackButton = root.Q<Button>("button-Back");
        SettingsButton = root.Q<Button>("button-settings");
        ExitButton = root.Q<Button>("button-Exit");

        BackButton.clicked += BackButtonClicked;
        SettingsButton.clicked += SettingsButtonClicked;
        ExitButton.clicked += ExitButtonClicked;

        root.style.display = DisplayStyle.None;//ó������ ui�� �����.
    }
    void OnDisable()
    {
        BackButton.clicked -= BackButtonClicked;
        SettingsButton.clicked -= SettingsButtonClicked;
        ExitButton.clicked -= ExitButtonClicked;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))//esc��ư Ŭ������ ui Ȱ��ȭ
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;//���� �ð� �Ͻ� ����
        root.style.display = DisplayStyle.Flex;//ui�� ǥ���Ѵ�. ��  esc��ư�� Ŭ����.
        UnityEngine.Cursor.lockState = CursorLockMode.None;//���콺 Ŀ���� ui���� ���� ����
        UnityEngine.Cursor.visible = true;//���콺 Ŀ���� ���̵��� ����
    
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;//�����簳
        root.style.display = DisplayStyle.None;//ui�� �����.
       UnityEngine.Cursor.lockState = CursorLockMode.Locked;//���콺 Ŀ���� ui���� ����
        UnityEngine.Cursor.visible = false;//���콺 Ŀ���� ����

    }

    private void BackButtonClicked()
    {
        Debug.Log("Back button clicked");
        ResumeGame();
    }

    private void SettingsButtonClicked()
    {
        Debug.Log("Settings button clicked");
    }

    private void ExitButtonClicked() 
    {
        Debug.Log("Exit button clicked");
        Application.Quit();
    }


}
