using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // ���ӿ��� �� ���� ������ϱ� ���� ���

public class SanityManager : MonoBehaviour
{
    public TextMeshProUGUI sanityText; // ���ŷ� ��ġ UI �ؽ�Ʈ
    public TextMeshProUGUI medicineText; // ȸ���� ��ġ UI �ؽ�Ʈ

    public int sanity; // ���ŷ� ��ġ
    public int maxSanity; // �ִ� ���ŷ� (���̵���)
    public int medicine; // ȸ���� ����

    public string difficulty = "Normal";

    private bool gameOverTriggered = false; // ���ӿ��� ���¸� ����
    private float countdownTime = 20f; // ���ŷ� 0�� �� ���ӿ��� �ð�

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
            sanityText.text = "���ŷ�: " + sanity.ToString();

            // ���ŷ��� 0 ���Ϸ� �������� ���ӿ����� ������Ű�� �ڷ�ƾ ȣ��
            if (sanity <= 0 && !gameOverTriggered)
            {
                sanity = 0;
                gameOverTriggered = true; // ���ӿ����� Ʈ���ŵǾ����� ǥ��
                if (countdownCoroutine != null) StopCoroutine(countdownCoroutine); // ���� ī��Ʈ�ٿ� ����
                countdownCoroutine = GameOverCountdown(countdownTime);
                StartCoroutine(countdownCoroutine); // ī��Ʈ�ٿ� ����
            }
        }
    }

    public void RestoreSanity()
    {
        // ���ŷ��� �ִ�ġ ������ ���� ȸ��
        if (sanity < maxSanity)
        {
            if (medicine > 0)
            {
                sanity++;
                medicine--;

                // UI �ؽ�Ʈ ������Ʈ
                sanityText.text = "���ŷ�: " + sanity.ToString();
                medicineText.text = "ȸ����: " + medicine.ToString();

                // ���ŷ� ȸ�� �� ���ӿ��� ���� �ʱ�ȭ
                if (gameOverTriggered && sanity > 0)
                {
                    if (countdownCoroutine != null) StopCoroutine(countdownCoroutine); // ī��Ʈ�ٿ� ����
                    sanityText.text = "���ŷ�: " + sanity.ToString(); // ���ŷ� UI ������Ʈ
                    gameOverTriggered = false; // ���ӿ��� Ʈ���� ���� �ʱ�ȭ
                }
            }
        }
    }

    // ���ŷ��� �ʱ�ȭ�ϴ� �Լ�
    public void ResetSanity()
    {
        sanity = maxSanity; // �ʱ�ȭ �� �ִ� ���ŷ����� ����
        sanityText.text = "���ŷ�: " + sanity.ToString(); // UI �ؽ�Ʈ ������Ʈ
        medicineText.text = "ȸ����: " + medicine.ToString();
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

        medicineText.text = "ȸ����: " + medicine.ToString();
    }

    private IEnumerator GameOverCountdown(float delay)
    {
        float elapsedTime = 0f; // ��� �ð�
        while (elapsedTime < delay)
        {
            float remainingTime = delay - elapsedTime;
            sanityText.text = $"���ӿ������� {Mathf.Ceil(remainingTime)}��"; // ���� �ð� UI ������Ʈ
            yield return new WaitForSeconds(1f); // 1�� ���
            elapsedTime += 1f; // ��� �ð� ������Ʈ
        }
        GameOver(); // ���� ���� ó��
    }

    // ���� ������ ó���ϴ� �Լ�
    private void GameOver()
    {
        sanityText.text = "Game Over"; // test
        // ���ӿ��� ���� �߰�
    }
}
