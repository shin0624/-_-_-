using UnityEngine;
using UnityEngine.UIElements;

public class Temp_Pause : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    private Button backButton;
    private Button exitButton;
    private bool isPaused = false;

    void Start()
    {
        // UIDocument ���� ��������
        uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument component not found!");
            return;
        }

        // UXML ������ root VisualElement ��������
        root = uiDocument.rootVisualElement;

        // BackButton ã�� �� Ŭ�� �̺�Ʈ �ڵ鷯 ���
        backButton = root.Q<Button>("BackButton");
        if (backButton != null)
        {
            backButton.clickable.clicked += OnBackButtonClicked;
        }
        else
        {
            Debug.LogError("BackButton not found in the UXML.");
        }

        // ExitButton ã�� �� Ŭ�� �̺�Ʈ �ڵ鷯 ���
        exitButton = root.Q<Button>("ExitButton");
        if (exitButton != null)
        {
            exitButton.clickable.clicked += OnExitButtonClicked;
        }
        else
        {
            Debug.LogError("ExitButton not found in the UXML.");
        }

        // �ʱ⿡�� Pause UI ��Ȱ��ȭ
        root.style.display = DisplayStyle.None;
    }

    void Update()
    {
        // ESC Ű �Է� ó��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
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
        root.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f; // ���� �Ͻ� ����
        Debug.Log("Game paused.");
    }

    void ResumeGame()
    {
        isPaused = false;
        root.style.display = DisplayStyle.None;
        Time.timeScale = 1f; // ���� �簳
        Debug.Log("Game resumed.");
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
        // ���� ������Ʈ�� �ı��� �� �̺�Ʈ �ڵ鷯 ����
        if (backButton != null)
        {
            backButton.clickable.clicked -= OnBackButtonClicked;
        }
        if (exitButton != null)
        {
            exitButton.clickable.clicked -= OnExitButtonClicked;
        }
    }
}
