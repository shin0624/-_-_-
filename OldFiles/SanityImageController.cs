using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanityImageController : MonoBehaviour
{
    // ���ŷ¿� ���� �÷��̾� ȭ�鿡 ���� �ٲ�� �д� ���°� �Ǵ� ������ ���� ��ũ��Ʈ. Sanity Manager�� Sanity�� �����ͼ�, ���� ���� �г� �̹����� Color, alpha���� �����Ͽ� ��Ÿ����.

    [SerializeField]
    private GameObject Panel;//ui �г�
    [SerializeField]
    private Image PanelImage;//ui�гο� ������ �̹��� ������Ʈ

    [SerializeField]
    private Color SanityColor03 = new Color(0f, 0f, 0f, 0f);//�⺻ ui �÷� (���İ� 0)
    [SerializeField]
    private Color SanityColor02 = new Color(0f, 123 / 255f, 0f, 27 / 255f);// ���ŷ� 2�� �� : ���ȭ��
    [SerializeField]
    private Color SanityColor01 = new Color(123 / 255f, 0f, 0f, 110 / 255f);//���ŷ� 1�� �� : ������ ȭ��
    [SerializeField]
    private Color SanityColor00 = new Color(0f, 0f, 0f, 230 / 255f);//���ŷ� 0�� �� : ������ ȭ��

    [SerializeField]
    private AudioClip SanitySound00;
    [SerializeField]
    private AudioClip SanitySound01;
    [SerializeField]
    private AudioClip SanitySound02;

    public SanityManager sanityManager;// sanityManager Ŭ������ sanity������ �����ϱ� ����

    private Coroutine alphaCoroutine; //���İ� �ִϸ��̼� �ڷ�ƾ.  sanity�� ���� �� ���İ��� ������ �־ ���� ������ ��ġ�� ȿ���� �� ����


    void Start()
    {
        
        PanelImage = Panel.GetComponent<Image>();
        SanityColor03 = PanelImage.color; //�ʱ� sanity ���� ���� ���� ����. ���� ���� �� ���İ� 0�� �⺻ ȭ��

        if (PanelImage==null)
        {
            Debug.Log("PanelImage is null!");
        }
        else
        {
            Debug.Log(PanelImage);
        }
        if(SanitySound00==null || SanitySound01==null || SanitySound02==null)
        {
            Debug.Log($"SanitySound is NULL");
        }

        sanityManager.OnSanityChanged += UpdatePanelColor;// SanityManager�� �̺�Ʈ�� ����

       // UpdatePanelColor(sanityManager.SSanity);

    }

    // sanity ���� ���� �г��� ������ ������Ʈ�ϴ� �̺�Ʈ �ڵ鷯
    private void UpdatePanelColor(int sanity)
    {
       // Debug.Log("Sanity Changed : " + sanity);
       if (alphaCoroutine!=null)
        {
            StopAllCoroutines();//���� �ڷ�ƾ�� �ִٸ� �ߴ�
        }

        switch (sanity)
        {   
            case 3:
                PanelImage.color = SanityColor03;//���İ� 0, �⺻ ȭ��
                AudioManager.Instance.StopSound(SanitySound02);
                break;
            case 2:
                PanelImage.color = SanityColor02;// ���ŷ� 2ĭ, ���İ� 0.3, ���
                //alphaCoroutine = StartCoroutine(AnimateAlpha(SanityColor02.a, 0.7f));
                alphaCoroutine = StartCoroutine(PulseAlpha(0.3f, 0.6f, 0.5f)); // ���İ��� 0.3���� 0.6 ���̷� ����
                AudioManager.Instance.StopSound(SanitySound01);
                AudioManager.Instance.LoopSound(SanitySound02);
                break;
            case 1:
                PanelImage.color = SanityColor01; // ���ŷ� 1ĭ, ���İ� 0.7, ������
                //alphaCoroutine = StartCoroutine(AnimateAlpha(SanityColor01.a, 0.7f));
                alphaCoroutine = StartCoroutine(PulseAlpha(0.5f, 0.8f, 0.5f)); // ���İ��� 0.5���� 0.8 ���̷� ����
                AudioManager.Instance.StopSound(SanitySound00);
                AudioManager.Instance.StopSound(SanitySound02);
                AudioManager.Instance.LoopSound(SanitySound01);
                break;
            case 0:
                PanelImage.color= SanityColor00;//���ŷ� 0ĭ, ���İ� 1, ������
                //alphaCoroutine = StartCoroutine(AnimateAlpha(SanityColor00.a, 1.0f));
                alphaCoroutine = StartCoroutine(PulseAlpha(0.7f, 1.0f, 0.5f)); // ���İ��� 0.7���� 1.0 ���̷� ����
                AudioManager.Instance.StopSound(SanitySound01);
                AudioManager.Instance.PlaySound(SanitySound00);
                break;
            default:
                Debug.Log("Unexpected Sanity value " + sanity);
                break;
        }
    }

    private IEnumerator AnimateAlpha(float targetAlpha, float duration)//���İ��� ��ȭ��Ű�� �ڷ�ƾ �߰�
    {
        Color currentColor = PanelImage.color; //���� �г��� �÷�
        float startAlpha = currentColor.a;//���� ���İ�
        float elapsedTime = 0f;

        // ���İ��� targetAlpha�� ���� ������ �ð��� �������� ��ȭ
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime/duration);//���İ� ��������
            PanelImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);//���İ� ����
            yield return null;// �������� ���
        }
        PanelImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);//��������
    }

    private IEnumerator PulseAlpha(float minAlpha, float maxAlpha, float duration)//���İ� ���� �ڷ�ƾ �߰�
    {
        // ��ǥ ���İ��� �ݺ������� �����ϴ� �ڷ�ƾ. Ư�� sanity���� �� ���İ��� �Դٰ����ϴ� ����ȿ�� ����
        while(true)
        {
            //���İ��� min���� max�� ����
            yield return StartCoroutine(AnimateAlpha(maxAlpha, duration));

            //���İ��� max���� min���� ����
            yield return StartCoroutine(AnimateAlpha(minAlpha, duration));
        }
    }




    private void OnDestroy()// �޸� ���� ������ ���� ������Ʈ�� �ı��� �� �̺�Ʈ ���� ����
    {
        sanityManager.OnSanityChanged -= UpdatePanelColor;
    }




}
