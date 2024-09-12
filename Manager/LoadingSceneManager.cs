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
    public Image Progress;//�ε�ȭ�� �̹���

    void Start()
    {
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

        float timer = 0.0f;
        while(!op.isDone)//�ε��� �Ϸ�� �� ���� �ݺ��Ѵ�.
        {
            yield return null;//�� ������ ���
            timer+=Time.deltaTime;//Ÿ�̸� �ð� ����

            if(op.progress < 0.9f)//�񵿱ⰴü�� ���൵�� 0.9 ����(��, �ε� ������)�϶�
            {
                //�ε� �̹����� fillAmount�� ���� �ε� ���൵�� ���� �ε巴�� ������Ŵ.
                Progress.fillAmount = Mathf.Lerp(Progress.fillAmount, op.progress, timer);
                
                if(Progress.fillAmount >=op.progress)//�ε� �̹����� fillAmount�� �ε� ���൵���� ũ�ų� ��������(��, ĭ�� �� á�µ��� �ε����̸�)
                {
                    timer = 0f;//Ÿ�̸Ӵ� �ٽ� 0����.
                }
            }
            else//�ε� ���൵�� 0.9 �̻��� ��(���� �Ϸ�� ����)
            {
                Progress.fillAmount = Mathf.Lerp(Progress.fillAmount, 1f, timer);//�ε� �̹����� fillAmount�� 1(100%)�� �ε巴�� ������Ŵ
                
                if(Progress.fillAmount ==1.0f)//�ε� �̹����� fillAmount�� 1�� �Ǹ� ���� ������ ��ȯ
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    
    }

}
