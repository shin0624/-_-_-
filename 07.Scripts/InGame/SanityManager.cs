using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SanityManager : MonoBehaviour
{
    // �� �Դ� ����
    public AudioClip eatingSound;

    // ����Ҹ� ����
    public AudioClip heartbeat;

    // filledImage ����
    public Image filledMedicine;
    public Image filledSanity;

    // TMP UI �ؽ�Ʈ ����
    public TextMeshProUGUI sanityText; // ���ŷ� ��ġ
    public TextMeshProUGUI medicineText; // ȸ���� ��ġ

    public int sanity; // ���ŷ� ��ġ
    public int maxSanity; // �ִ� ���ŷ�
    public int medicine; // ȸ���� ����

    public string difficulty = "Normal"; // ���̵�

    private bool gameOverTriggered = false; // ���ӿ��� ���¸� ����
    private bool isRestoringSanity = false; // ���ŷ� ȸ�� ������ ����
    private float countdownTime = 10f; // ���ŷ� 0�� �� ���ӿ��� �ð�
    private float lastMedicineTime = -Mathf.Infinity; // ���������� ���� ���� �ð�

    private IEnumerator countdownCoroutine; // ī��Ʈ�ٿ� �ڷ�ƾ ���� ����

    void Start()
    {
        // ���̵��� ���� �ʱ� ���ŷ� �� ����
        SetDifficulty();
        ResetSanity();
    }

    void Update()
    {
        // �׽�Ʈ�� ���� Q Ű�� ������ ���ŷ� ����
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DecreaseSanity();
        }

        // �׽�Ʈ�� ���� R Ű�� ������ ���ŷ� ȸ��
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestoreSanity();
        }

        // �׽�Ʈ�� ���� M Ű�� ������ ȸ���� ȹ��
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddMedicine();
        }

        //���� �� ȸ�� Ű ���� ��� ����
    }

    // ���̵��� ���� �ִ� ���ŷ� ����
    private void SetDifficulty()
    {
        switch (difficulty)
        {
            case "Easy":
                maxSanity = 5;
                break;
            case "Normal":
                maxSanity = 3;
                break;
            default:
                maxSanity = 3; // �⺻���� Normal
                break;
        }
    }

    public void DecreaseSanity()
    {
        // ���ŷ��� 1 �̻��� ���� ����
        if (sanity > 0)
        {
            sanity--;

            // UI �ؽ�Ʈ ������Ʈ
            sanityText.text = sanity.ToString();

            // ���ŷ��� 0 ���Ϸ� �������� ���ӿ����� ������Ű�� �ڷ�ƾ ȣ��
            if (sanity <= 0 && !gameOverTriggered)
            {
                sanity = 0;
                sanityText.color = new Color32(230, 25, 5, 255); // 0�� �� ���� ��
                gameOverTriggered = true; // ���ӿ����� Ʈ���ŵǾ����� ǥ��
                AudioManager.Instance.PlaySoundLoop(heartbeat);
                if (countdownCoroutine != null) StopCoroutine(countdownCoroutine); // ���� ī��Ʈ�ٿ� ����
                countdownCoroutine = GameOverCountdown(countdownTime);
                StartCoroutine(countdownCoroutine); // ī��Ʈ�ٿ� ����
            }

            // ���ŷ� ���� �� ü�¹� ������Ʈ
            UpdateSanityBar();
        }
    }

    public void RestoreSanity()
    {
        // ���� �ð��� ���������� ���� ���� �ð� �� �� ȸ�� ������ Ȯ��
        if (!isRestoringSanity && Time.time - lastMedicineTime >= 5f) // 5�ʰ� �������� Ȯ��
        {
            // ���ŷ��� �ִ�ġ ������ ���� ȸ��
            if (sanity < maxSanity)
            {
                if (medicine > 0)
                {
                    isRestoringSanity = true; // ȸ�� ����
                    StartCoroutine(DelayedRestoreSanity()); // 5�� �� ���ŷ� ȸ��
                    medicine--;
                    medicineText.text = medicine.ToString();

                    AudioManager.Instance.PlaySound(eatingSound); // AudioManager�� ���� ���� ���

                    lastMedicineTime = Time.time; // ���� �ð��� ������ �� ���� �ð����� ���
                }
            }
        }
        else
        {
            //Debug.Log("���� �ٽ� �Ա� ���ؼ��� 5�ʸ� ��ٷ��� �մϴ�.");
        }
    }

    // ���� ���� �� 5�� �ڿ� ���ŷ��� ȸ���ϴ� �ڷ�ƾ
    private IEnumerator DelayedRestoreSanity()
    {
        // ȸ���� �ɸ��� �ð� (5��) // �� �Դ� �Ҹ� ���� ����
        float duration = 5f;
        // ��� �ð� ���� ����
        float elapsedTime = 0f;

        filledMedicine.fillAmount = 1f;

        // ȸ�� ������ ����Ǵ� ���� �ݺ��� ����
        while (elapsedTime < duration)
        {
            // ��� �ð��� ������Ʈ (Time.deltaTime�� ������ �� ��� �ð�)
            elapsedTime += Time.deltaTime;
            // ����� �ð� ������ ���� fillAmount�� 1���� 0���� ���ҽ�Ű�� �ð��� ȿ���� ����
            filledMedicine.fillAmount = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            // ���� �����ӱ��� ���
            yield return null;
        }

        sanity++;
        sanityText.text = sanity.ToString(); // UI �ؽ�Ʈ ������Ʈ

        // ���ŷ� ȸ�� �� ���ӿ��� ���� �ʱ�ȭ
        if (gameOverTriggered && sanity > 0)
        {
            if (countdownCoroutine != null) StopCoroutine(countdownCoroutine); // ī��Ʈ�ٿ� ����
            sanityText.color = new Color32(25, 25, 5, 255); // ȸ�� �� �⺻ ��������
            sanityText.text = sanity.ToString(); // ���ŷ� UI ������Ʈ
            gameOverTriggered = false; // ���ӿ��� Ʈ���� ���� �ʱ�ȭ
            AudioManager.Instance.StopSoundLoop();
        }

        isRestoringSanity = false; // ȸ�� �Ϸ�

        // ���ŷ� ȸ�� �� ü�¹� ������Ʈ
        UpdateSanityBar();
    }

    private void UpdateSanityBar()
    {
        // �ִ� ���ŷ��� 0���� ū ��쿡�� ������Ʈ ����
        if (maxSanity > 0)
        {
            // ���� ���ŷ� ������ ��� (0���� 1 ����)
            float targetFillAmount = 1 - ((float)sanity / maxSanity);
            // fillAmount ������Ʈ
            StartCoroutine(SmoothUpdateSanityBar(filledSanity.fillAmount, targetFillAmount));
        }
    }

    private IEnumerator SmoothUpdateSanityBar(float startFillAmount, float targetFillAmount)
    {
        // UI �ε巴�� ������Ʈ �ð�
        float duration = 1f;
        // ��� �ð� ���� ����
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            // fillamount�� �ε巴�� ����
            filledSanity.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / duration);
            // ���� �����ӱ��� ���
            yield return null;
        }

        // ��ȯ�� �Ϸ�� �� ���� �� ����
        filledSanity.fillAmount = targetFillAmount;
    }

    // ���ŷ��� �ʱ�ȭ�ϴ� �Լ�
    public void ResetSanity()
    {
        sanity = maxSanity; // �ʱ�ȭ �� �ִ� ���ŷ����� ����
        sanityText.text = sanity.ToString(); // UI �ؽ�Ʈ ������Ʈ
        medicineText.text = medicine.ToString();
        gameOverTriggered = false; // ���ӿ��� ���� �ʱ�ȭ
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine); // ���� ī��Ʈ�ٿ� ����
            countdownCoroutine = null; // ī��Ʈ�ٿ� �ڷ�ƾ ������ null�� ����
        }
    }

    public void AddMedicine()
    {
        medicine++;
        medicineText.text = medicine.ToString();
    }

    private IEnumerator GameOverCountdown(float delay)
    {
        float elapsedTime = 0f; // ��� �ð�
        while (elapsedTime < delay)
        {
            float remainingTime = delay - elapsedTime;
            //sanityText.text = $"���ӿ������� {Mathf.Ceil(remainingTime)}��"; // ���� �ð� UI ������Ʈ
            yield return new WaitForSeconds(1f); // 1�� ���
            elapsedTime += 1f; // ��� �ð� ������Ʈ
        }
        GameOver(); // ���� ���� ó��
    }

    // ���� ������ ó���ϴ� �Լ�
    private void GameOver()
    {
        sanityText.text = "Game Over"; // test
        AudioManager.Instance.StopSoundLoop();
        // ���ӿ��� ���� �߰�
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
