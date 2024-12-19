using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SanityManager : MonoBehaviour
{
    [Header("Scripts")]
    public AudioManager audioManager;

    [Header("UI Elements")]
    public Image filledMedicine;            // ȸ���� UI �̹��� (fillAmount�� ǥ��)
    public Image filledSanity;              // ���ŷ� UI �̹��� (fillAmount�� ǥ��)
    public Image redScreenPanel;            // ������ ȭ�� �г� UI
    public TextMeshProUGUI sanityText;      // ���ŷ� ��ġ�� ǥ���ϴ� �ؽ�Ʈ
    public TextMeshProUGUI medicineText;    // ȸ���� ������ ǥ���ϴ� �ؽ�Ʈ

    [Header("Sounds")]
    public AudioClip eatingSound;   // ȸ���� ��� �� ����� ����
    public AudioClip heartbeat;     // ���ŷ� 0�� �� ����� ���� �Ҹ�

    [Header("Settings")]
    public int sanity;                      // ���� ���ŷ� ��ġ
    public int maxSanity;                   // �ִ� ���ŷ� ��ġ
    public int medicine;                    // ���� ������ ȸ���� ����
    public string difficulty = "Normal";    // ���� ���̵� ����

    private bool gameOverTriggered = false; // ���� ���� ���¸� ����
    private bool isRestoringSanity = false; // ���ŷ� ȸ�� ������ ���θ� ����
    private float countdownTime = 10f;      // ���ŷ��� 0�� �� ���� �������� ��� �ð� 
    private float lastMedicineTime = -Mathf.Infinity;   // ������ ȸ���� ��� �ð�

    private IEnumerator countdownCoroutine; // ���� ���� ī��Ʈ�ٿ� �ڷ�ƾ ����
    private CameraController cameraController;//���ŷ� ���ҽ� ī�޶� ��鸲 ȿ�� ������ �����ϱ� ���� ����
    void Start()
    {
        // ���� ���̵��� ���� �ʱ� ���ŷ� ���� �����ϰ� ���ŷ��� �ʱ�ȭ
        audioManager = GetComponent<AudioManager>();
        cameraController  = FindAnyObjectByType<CameraController>();
        SetDifficulty();
        ResetSanity();
    }

    void Update()
    {
        // Q Ű�� ������ �׽�Ʈ������ ���ŷ� ����
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DecreaseSanity();
        }

        // R Ű�� ������ �׽�Ʈ������ ���ŷ� ȸ��
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestoreSanity();
        }

        // M Ű�� ������ �׽�Ʈ������ ȸ���� ȹ��
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddMedicine();
        }

        // ���� �ÿ��� �׽�Ʈ�� Ű�� ����
    }

    // ���̵��� ���� �ִ� ���ŷ� ���� ����
    private void SetDifficulty()
    {
        switch (difficulty)
        {
            case "Easy":
                maxSanity = 5; // Easy ���̵����� �ִ� ���ŷ� ����
                break;
            case "Normal":
                maxSanity = 3; // Normal ���̵����� �ִ� ���ŷ� ����
                break;
            default:
                maxSanity = 3; // �⺻������ Normal ���̵� ����
                break;
        }
    }

    // ���ŷ��� ���ҽ�Ű�� �޼���
    public void DecreaseSanity()
    {
        // ���ŷ��� 0���� Ŭ ���� ����
        if (sanity > 0)
        {
            sanity--;
            sanityText.text = sanity.ToString(); // UI ������Ʈ
             if(cameraController!=null)
            {
                cameraController.StartShake();//ī�޶� ��鸲 ����
            }
            
            // ���ŷ��� 0 ���Ϸ� �������� ���� ���� ���·� ��ȯ
            if (sanity <= 0 && !gameOverTriggered)
            {
                sanity = 0;
                sanityText.color = new Color32(230, 25, 5, 255);    // ���ŷ��� 0�� �� ���ŷ� �ؽ�Ʈ ���� ����
                gameOverTriggered = true;                           // ���� ���� ���� ����
                audioManager.PlaySound(heartbeat);              // ���� �Ҹ� �ݺ� ���
                if (countdownCoroutine != null) StopCoroutine(countdownCoroutine); // ���� ī��Ʈ�ٿ� ����
                countdownCoroutine = GameOverCountdown(countdownTime);
                StartCoroutine(countdownCoroutine); // ī��Ʈ�ٿ� ����

                // �г��� ���İ��� �ε巴�� ����
                if (redScreenPanel != null)
                {
                    StartCoroutine(FadeRedScreenPanel(redScreenPanel.color.a, 20f / 255f, 1f));
                }
            }

            // ���ŷ� ���ҿ� ���� UI ������Ʈ
            UpdateSanityBar();
        }
    }

    // ���ŷ��� ȸ����Ű�� �޼���
    public void RestoreSanity()
    {
        // ������ ȸ���� ��� �� 5�ʰ� �������� Ȯ��
        if (!isRestoringSanity && Time.time - lastMedicineTime >= 5f)
        {
            // ���ŷ��� �ִ�ġ �̸��� ���� ȸ��
            if (sanity < maxSanity)
            {
                if (medicine > 0)
                {
                    isRestoringSanity = true;                   // ȸ�� ����
                    StartCoroutine(DelayedRestoreSanity());     // 5�� �� ���ŷ� ȸ��
                    medicine--;
                    medicineText.text = medicine.ToString();    // ȸ���� ���� UI ������Ʈ

                    audioManager.PlaySound(eatingSound);        // ȸ���� ��� ���� ���

                    lastMedicineTime = Time.time;               // ������ ���� ���� �ð� ���
                }
            }
        }
        else
        {
            // Debug.Log("���� �ٽ� �Ա� ���ؼ��� 5�ʸ� ��ٷ��� �մϴ�.");
        }
    }

    // ���� ���� �� 5�� �ڿ� ���ŷ��� ȸ���ϴ� �ڷ�ƾ
    private IEnumerator DelayedRestoreSanity()
    {
        float duration = 5f;    // ȸ���� �ɸ��� �ð�
        float elapsedTime = 0f; // ��� �ð� ����

        filledMedicine.fillAmount = 1f; // ȸ���� UI�� fillAmount�� �ʱ�ȭ

        // ȸ�� ������ ����Ǵ� ���� �ݺ�
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            filledMedicine.fillAmount = Mathf.Lerp(1f, 0f, elapsedTime / duration); // UI�� fillAmount �ִϸ��̼�
            yield return null; // ���� �����ӱ��� ���
        }

        sanity++;
        sanityText.text = sanity.ToString(); // ���ŷ� ��ġ UI ������Ʈ

        // ���ŷ� ȸ�� �� ���� ���� ���� �ʱ�ȭ
        if (gameOverTriggered && sanity > 0)
        {
            if (countdownCoroutine != null) StopCoroutine(countdownCoroutine); // ī��Ʈ�ٿ� ����
            sanityText.color = new Color32(25, 25, 5, 255); // ���ŷ� ȸ�� �� ���ŷ� �ؽ�Ʈ �⺻ �������� ����
            sanityText.text = sanity.ToString();    // ���ŷ� ��ġ UI ������Ʈ
            gameOverTriggered = false;              // ���� ���� ���� �ʱ�ȭ
            audioManager.StopSound(heartbeat);           // ���� �Ҹ� ����

            // �г��� ���İ��� �ε巴�� ����
            if (redScreenPanel != null)
            {
                StartCoroutine(FadeRedScreenPanel(redScreenPanel.color.a, 0f, 1f)); // 1�� ���� ���İ��� 0���� ����
            }
        }

        isRestoringSanity = false; // ȸ�� �Ϸ�

        // ���ŷ� ȸ�� �� UI ������Ʈ
        UpdateSanityBar();
    }

    // ���ŷ� �� UI�� ������Ʈ�ϴ� �޼���
    private void UpdateSanityBar()
    {
        if (maxSanity > 0)
        {
            float targetFillAmount = 1 - ((float)sanity / maxSanity); // ���ŷ� ���� ���
            StartCoroutine(SmoothUpdateSanityBar(filledSanity.fillAmount, targetFillAmount)); // �ε巴�� ������Ʈ
        }
    }

    // ���ŷ� ���� fillAmount ���� �ε巴�� ������Ʈ�ϴ� �ڷ�ƾ
    private IEnumerator SmoothUpdateSanityBar(float startFillAmount, float targetFillAmount)
    {
        float duration = 1f;    // �ε巴�� ������Ʈ�� �ð�
        float elapsedTime = 0f; // ��� �ð� ����

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            filledSanity.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / duration); // UI �ִϸ��̼�
            yield return null; // ���� �����ӱ��� ���
        }

        filledSanity.fillAmount = targetFillAmount; // ���� �� ����
    }

    // ������ ȭ�� �г��� ���İ��� �ε巴�� ��ȭ��Ű�� �ڷ�ƾ
    private IEnumerator FadeRedScreenPanel(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color startColor = redScreenPanel.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, endAlpha);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            redScreenPanel.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        redScreenPanel.color = endColor; // ���� ���� ����
    }

    // ���ŷ��� �ʱ�ȭ�ϴ� �޼���
    public void ResetSanity()
    {
        sanity = maxSanity;                         // �ʱ�ȭ �� �ִ� ���ŷ����� ����
        sanityText.text = sanity.ToString();        // ���ŷ� ��ġ UI ������Ʈ
        medicineText.text = medicine.ToString();    // ȸ���� ���� UI ������Ʈ
        gameOverTriggered = false;                  // ���� ���� ���� �ʱ�ȭ
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);  // ī��Ʈ�ٿ� ����
            countdownCoroutine = null;          // �ڷ�ƾ ���� �ʱ�ȭ
        }
    }

    // ȸ���� ������ ������Ű�� �޼���
    public void AddMedicine()
    {
        medicine++;
        medicineText.text = medicine.ToString(); // UI ������Ʈ
    }

    // ���� ���������� ī��Ʈ�ٿ��� ó���ϴ� �ڷ�ƾ
    private IEnumerator GameOverCountdown(float delay)
    {
        float elapsedTime = 0f; // ��� �ð� ����
        while (elapsedTime < delay)
        {
            float remainingTime = delay - elapsedTime;
            // sanityText.text = $"���ӿ������� {Mathf.Ceil(remainingTime)}��"; // ���� �ð� UI ������Ʈ
            yield return new WaitForSeconds(1f); // 1�� ���
            elapsedTime += 1f; // ��� �ð� ������Ʈ
        }
        GameOver(); // ���� ���� ó��
    }

    // ���� ������ ó���ϴ� �޼���
    private void GameOver()
    {
        // sanityText.text = "Game Over"; // ���� ���� �޽��� ǥ��
        audioManager.StopSound(heartbeat); // ���� �Ҹ� ����
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ���� ��� �ٽ� �ε�
        /*
         * ���� ���� ���� �߰�
         */
    }
}