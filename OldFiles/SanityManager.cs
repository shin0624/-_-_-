using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // ���ӿ��� �� ���� ������ϱ� ���� ���
using System;

public class SanityManager : MonoBehaviour
{
    // �� �Դ� ����
    public AudioClip eatingSound;

    public TextMeshProUGUI sanityText; // ���ŷ� ��ġ UI �ؽ�Ʈ
    public TextMeshProUGUI medicineText; // ȸ���� ��ġ UI �ؽ�Ʈ

    public int sanity; // ���ŷ� ��ġ --> ���Ǵ� ���ŷ� ��ġ�� �ҹ��ڷ� ����

    public int maxSanity; // �ִ� ���ŷ� (���̵���)
    public int medicine; // ȸ���� ����

    public string difficulty = "Normal";

    private bool gameOverTriggered = false; // ���ӿ��� ���¸� ����
    private bool isRestoringSanity = false; // ���ŷ� ȸ�� ������ ����
    private float countdownTime = 15f; // ���ŷ� 0�� �� ���ӿ��� �ð�
    private float lastMedicineTime = -Mathf.Infinity; // ���������� ���� ���� �ð�

    private IEnumerator countdownCoroutine; // ī��Ʈ�ٿ� �ڷ�ƾ ���� ����

    //���ŷ� ���� Ÿ�̸� �߰�. ���ŷ��� 1, 2�� �� 10�� ��� �� ���ŷ� �� �ܰ� �϶�.
    private float sanityTimer = 0f;
    [SerializeField]
    private float sanityDecreaseInterval = 10.0f;// ���ŷ� ���ӽð�.

    // sanity �̺�Ʈ �߰� --> ���ŷ� ui �̺�Ʈ�� ���� ���ͼ��͸� ������ ������ SSanity�� ����
    public event Action<int> OnSanityChanged;// sanity���� ���� ȭ�� ȿ���� �ٸ��� �ϱ� ���� �̺�Ʈ ����. SanityImageController���� �����Ͽ� ����� ��. 
    public int SSanity { get { return sanity; } set { if (sanity != value) { sanity = value; OnSanityChanged?.Invoke(sanity); sanityTimer = 0f; } } }//SSanity���� ����� �� �̺�Ʈ �߻�. ���ŷ� ���� �� Ÿ�̸� �ʱ�ȭ �߰�

    private CameraController cameraController;//���ŷ� ���ҽ� ī�޶� ��鸲 ȿ�� ������ �����ϱ� ���� ����
    
    void Start()
    {
        // ���̵��� ���� �ʱ� ���ŷ� �� ����
        SetDifficulty();
        ResetSanity();
        cameraController  = FindAnyObjectByType<CameraController>();
    }

    void Update()
    {
        // �׽�Ʈ�� ���� Q Ű�� ������ ���ŷ� ����
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DecreaseSanity();
            Debug.Log($"now sanity : {sanity} ");

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

        if(SSanity == 1 || SSanity == 2)
        {
            sanityTimer += Time.deltaTime;
            if(sanityTimer>=sanityDecreaseInterval)// 10�� �Ѿ��
            {
                DecreaseSanity();//���ŷ� ����
                sanityTimer = 0f;//Ÿ�̸� �ʱ�ȭ.
            }
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
            sanityText.text = sanity.ToString();

            if(cameraController!=null)
            {
                cameraController.StartShake();//ī�޶� ��鸲 ����
            }

            // ���ŷ��� 0 ���Ϸ� �������� ���ӿ����� ������Ű�� �ڷ�ƾ ȣ��
            if (sanity <= 0 && !gameOverTriggered)
            {
                sanity = 0;
                sanityText.color = new Color32(230, 25, 5, 255); // 0�� �� ���� ��
                gameOverTriggered = true; // ���ӿ����� Ʈ���ŵǾ����� ǥ��
                if (countdownCoroutine != null) StopCoroutine(countdownCoroutine); // ���� ī��Ʈ�ٿ� ����
                countdownCoroutine = GameOverCountdown(countdownTime);
                StartCoroutine(countdownCoroutine); // ī��Ʈ�ٿ� ����
            }

        }
    }

    public void RestoreSanity()
    {
        // ���� �ð��� ���������� ���� ���� �ð� �� �� ȸ�� ������ Ȯ��
        if (!isRestoringSanity && Time.time - lastMedicineTime >= 5f) // 5�ʰ� �������� Ȯ��
        {
            // ���ŷ��� �ִ�ġ ������ ���� ȸ��
            if (SSanity < maxSanity)
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
        yield return new WaitForSeconds(5f); // 5�� ���

        SSanity++;
        sanityText.text = SSanity.ToString(); // UI �ؽ�Ʈ ������Ʈ

        // ���ŷ� ȸ�� �� ���ӿ��� ���� �ʱ�ȭ
        if (gameOverTriggered && SSanity > 0)
        {
            if (countdownCoroutine != null) StopCoroutine(countdownCoroutine); // ī��Ʈ�ٿ� ����
            sanityText.color = new Color32(25, 25, 5, 255); // ȸ�� �� �⺻ ��������
            sanityText.text = sanity.ToString(); // ���ŷ� UI ������Ʈ
            gameOverTriggered = false; // ���ӿ��� Ʈ���� ���� �ʱ�ȭ
        }

        isRestoringSanity = false; // ȸ�� �Ϸ�
    }

    // ���ŷ��� �ʱ�ȭ�ϴ� �Լ�
    public void ResetSanity()
    {
        SSanity = maxSanity; // �ʱ�ȭ �� �ִ� ���ŷ����� ����
        sanityText.text = SSanity.ToString(); // UI �ؽ�Ʈ ������Ʈ
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
        // ���ӿ��� ���� �߰�
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
