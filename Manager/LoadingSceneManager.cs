using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene; // ���� �� �̸�

    // UI Elements
    [SerializeField] private Image loadingBar; // �ε� �� �̹���
    [SerializeField] private List<Sprite> backgroundImages = new List<Sprite>(); // ��� �̹��� ����Ʈ
    [SerializeField] private Image backgroundImage; // ��� �̹��� ������Ʈ
    [SerializeField] private Material bloodMaterial; // �� ��Ƽ����
    [SerializeField] private float waveSpeed = 0.1f; // �ǰ� �����̴� �ӵ�
    [SerializeField] private TextMeshProUGUI tipText; // �� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI mapNameText; // �� �̸� �ؽ�Ʈ

    // �� ����Ʈ
    public string[] tips = {
        "Tip: W A S D �� �̿��Ͽ� ������ �� �ֽ��ϴ�.",
        "Tip: F�� ���� �÷��ø� ���� �� �� �ֽ��ϴ�.",
        "Tip: ���� �����̾� ���������� 17�� ���� 46���� �Դϴ�.",
        "Tip: �� �׽�Ʈ1.",
        "Tip: �� �׽�Ʈ2.",
        "Tip: �� �׽�Ʈ3.",
        "Tip: �� �׽�Ʈ4.",
        "Tip: �� �׽�Ʈ5."
    };

    private Queue<string> recentTips = new Queue<string>(); // �ֱ� ���� ������ ť
    private int maxRecentTips = 3; // �ߺ� ������ ���� �ִ� ť ������

    void Start()
    {
        // ���� ��� �̹��� ����
        SetRandomLoadingImage();

        // �� �̸� "�췹�ý�" ����
        mapNameText.text = "�췹�ý�";

        // �񵿱� �ε� ����
        StartCoroutine(LoadAsyncScene());

        // �� ���� �ڷ�ƾ ����
        StartCoroutine(ChangeTips());
    }

    // �ٸ� ��ũ��Ʈ���� ���� ȣ���� �� �ֵ���
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene"); // �ε� �� ȣ��
    }

    // ���� ��� �̹��� ����
    private void SetRandomLoadingImage()
    {
        if (backgroundImages.Count > 0)
        {
            int randomIndex = Random.Range(0, backgroundImages.Count);
            backgroundImage.sprite = backgroundImages[randomIndex];
        }
    }

    // �񵿱� �� �ε�
    IEnumerator LoadAsyncScene()
    {
        yield return null; // ������ ������ ���

        // �� �ε� ����
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);
        operation.allowSceneActivation = false; // �ڵ����� �� ��ȯ���� �ʵ��� ����

        float loadingDuration = 2.0f; // �ּ� �ε� �ð� (��)
        float startTime = Time.time; // �ε� ���� �ð�

        while (!operation.isDone)
        {
            // �ε� ���൵ �ݿ� (0~1)
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.fillAmount = progress;

            // �� �ؽ�ó�� UV ��ǥ�� �������� �����̴� ȿ�� �ֱ�
            bloodMaterial.mainTextureOffset = new Vector2(0, Time.time * waveSpeed);
            //bloodMaterial.mainTextureOffset = new Vector2(Mathf.Sin(Time.time * waveSpeed), Mathf.Cos(Time.time * waveSpeed));

            // �ε� �Ϸ� ��
            if (operation.progress >= 0.9f)
            {
                // �ּ� �ε� �ð��� ����
                float elapsedTime = Time.time - startTime;
                if (elapsedTime < loadingDuration)
                {
                    // ���� �ð���ŭ ���
                    yield return new WaitForSeconds(loadingDuration - elapsedTime);
                }

                loadingBar.fillAmount = 1.0f; // �ε� �Ϸ�
                operation.allowSceneActivation = true; // �� ��ȯ
            }

            yield return null;
        }
    }

    // ���� 3�ʸ��� �������� ����, �ߺ� ����
    IEnumerator ChangeTips()
    {
        while (true)
        {
            string newTip;
            do
            {
                newTip = tips[Random.Range(0, tips.Length)];
            } while (recentTips.Contains(newTip));

            tipText.text = newTip;

            // �ֱ� �� ���� �� �ߺ� ����
            recentTips.Enqueue(newTip);
            if (recentTips.Count > maxRecentTips)
            {
                recentTips.Dequeue(); // ������ �� ����
            }

            // 3�� ���
            yield return new WaitForSeconds(3f);
        }
    }
}
