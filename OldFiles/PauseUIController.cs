using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIController : MonoBehaviour
{
    public Canvas pauseCanvas;
    public Button backButton;
    public Button exitButton;

    private bool isPaused = false;

    void Start()
    {
        if(pauseCanvas !=null)
        {
            pauseCanvas.gameObject.SetActive(false);//��Ȱ��ȭ ���·� �ʱ�ȭ
        }
        else
        {
            Debug.LogError("Pause Canvas not assigned!");
        }

        if(backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }
        else
        {
            Debug.LogError("BackButton not assigned!");
        }

        if(exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
        else
        {
            Debug.LogError("ExitButton not assigned!");
        }
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
    void PauseGame()
    {
        isPaused = true;
        if(pauseCanvas!=null)
        {
            pauseCanvas.gameObject.SetActive(true);
        }
        Time.timeScale = 0f;
        Debug.Log("game paused");
    }

    void ResumeGame()
    {
        isPaused = false;
        if(pauseCanvas!=null)
        {
            pauseCanvas.gameObject.SetActive(false);
        }
        Time.timeScale = 1f;
        Debug.Log("game resumed");
    }

    void OnBackButtonClicked()
    {
        Debug.Log("BackButton clicked.");
        ResumeGame(); // �Ͻ� ���� ����
    }


    void OnExitButtonClicked()
    {
        Debug.Log("ExitButton clicked.");
        Application.Quit(); // ���� ����
    }

    void OnDestroy()
    {
        // �̺�Ʈ �ڵ鷯 ����
        if (backButton != null)
        {
            backButton.onClick.RemoveListener(OnBackButtonClicked);
        }
        if (exitButton != null)
        {
            exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }
    }
}
