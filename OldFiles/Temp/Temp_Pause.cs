using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Temp_Pause : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu; // Inspector���� ���� �����ϵ��� ����
    private bool isPaused = false;

    void Start()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
            Debug.Log("Pause menu initialized and set to inactive.");
        }
        else
        {
            Debug.LogError("Pause menu not assigned! Please assign the Temp_PauseUI GameObject in the Inspector.");
        }
    }

    void Update()
    {
        // Esc Ű �Է� ó��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed.");
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ExitGame();
            }
        }

        // Space Ű �Է� ó��
        if (isPaused && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key pressed.");
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        if (pauseMenu != null)
        {
            isPaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f; // ������ �Ͻ� �����մϴ�.
            Debug.Log("Game paused.");
        }
    }

    private void ResumeGame()
    {
        if (pauseMenu != null)
        {
            isPaused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f; // ������ �簳�մϴ�.
            Debug.Log("Game resumed.");
        }
    }

    private void ExitGame()
    {
        Debug.Log("Exiting game.");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
