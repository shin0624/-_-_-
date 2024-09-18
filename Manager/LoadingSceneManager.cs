using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;//���� �� �̸�

    [SerializeField]
    private Image Progress;//�ε� �� �̹���
    [SerializeField]
    private List<Sprite> ProgressImages = new List<Sprite>();//��������Ʈ �̹����� �������� �����ֱ� ����, ��������Ʈ ��ü�� ���� ����Ʈ�� ����. ����Ʈ�� ����ũ���Ҵ��� ������ �ڷ����̶� ���� ���� �̹����� ���� ũ�������� �����൵ ��. ���� ���� �̹����� �ִ´ٸ� �������ָ� ����.
    [SerializeField]
    private Image LoadingPanelImage;//�гο� ������ �̹��� ����

    void Start()
    {
        SetRandomLoadingImage();
        StartCoroutine(LoadSceneCoroutione());
    }

    public static void LoadScene(string SceneName)//LoadScene�� �������� ȣ���Ͽ� �ٸ� ��ũ��Ʈ���� ���� ȣ�� ����
    {
        nextScene = SceneName;
        SceneManager.LoadScene("LoadingScene");//�ε��� ȣ��
    }
    
    IEnumerator LoadSceneCoroutione()//���� ���� �񵿱� ������� �ε��ϴ� �ڷ�ƾ
    {
        yield return null;//�������� ���� �� ���� ���

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);//���� ���� �񵿱� ������� �ε� ����

        op.allowSceneActivation = false;//���� �ε��� ������ �ڵ����� �ҷ��� ������ �̵��� ���ΰ��� ���� �ɼ�. 
                                        //false�� �����Ͽ� �ε� �Ϸ� �� ���� ������ ��ȯ���� �ʰ� ��� -> true�� �� �� ������ �ε� �� �� ��ȯ
        while(!op.isDone)
        {
            yield return null;// �� ������ ���
            //�ε� ���൵�� ���缭 fillAmount�� ����.
            float ProgressValue = Mathf.Clamp01(op.progress / 0.9f);//Clamp01�� ����ؼ� �ε� ���൵�� 0.0 ~ 1.0���� �����. Clamp01�� �ۼ�Ʈ���� �ٷ� �� ����.
            Progress.fillAmount = ProgressValue;

            if(op.progress >= 0.9f)//�ε� �Ϸ� ��
            {
                Progress.fillAmount = 1.0f;//�ε� �Ϸ� �� 100%�� �����.
                op.allowSceneActivation = true;//�� ��ȯ
                yield break;
            }       
        }
    }

    private void SetRandomLoadingImage()//���� �̹����� �����ϴ� �޼���
    {
        if(ProgressImages.Count >0)//����Ʈ�� �̹����� ������
        {
            int RandomIndex = Random.Range(0, ProgressImages.Count);//���� �ε��� ����
            LoadingPanelImage.sprite = ProgressImages[RandomIndex];//�г� �̹��� ����
        }
    }

}
