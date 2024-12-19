using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class FadeInOutController : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Color startColor = Color.white;
    [SerializeField] private Color endColor = Color.black;
    [SerializeField] private float fadeDuration = 3f;
    [SerializeField] private float delay = 2f;

    private void Start()
    {
        image = GetComponent<Image>();
        image.color = startColor;

        StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeOutCoroutine() //���̵�ƿ� �ڷ�ƾ
    {
        
        yield return new WaitForSeconds(delay); // ���� ���� �� ��� �����̸� �ش�.

        float timer = 0f;

        while (timer < fadeDuration) // ������ ���̵�ƿ� �ð����� ����
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;
            image.color = Color.Lerp(startColor, endColor, progress);//������ ���̵�ƿ� �ð����� white ~ black���� ������ �����
            yield return null; 
        }
        image.color = endColor;
    }
}
