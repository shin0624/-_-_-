using UnityEditor;
using UnityEngine;

public class Managers : MonoBehaviour
{
    //���� �Ŵ��� Ŭ���� ����. ���� ����Ǿ ��� �����Ǿ�� �� ������ �� �ֻ������� ����Ǿ�� �� �����͸� ����

    static Managers instance;// �̱��� ������ ����ϱ� ���� ���� �ν��Ͻ� ����
    public static Managers GetInstance() { if (instance == null) { Init(); } return instance; }//�ٸ� Ŭ�������� Managers�� ������ �� ����ϴ� ������Ƽ

    private ResourceManager resourceManager = new ResourceManager();//���ҽ��Ŵ��� �ν��Ͻ� ����
    public static ResourceManager Resource { get { return instance.resourceManager; } }

    public SceneChangeManager sceneChangeManager = new SceneChangeManager();//��ü���� �Ŵ��� �ν��Ͻ� ����
    public static SceneChangeManager SceneChange { get { return instance.sceneChangeManager; } }


    [SerializeField]
    private int PlayerMentalPower;//�÷��̾� ���ŷ�
    [SerializeField]
    private float PlayerFlashIntensity;//������ ���

    private void Awake()// Awake�� Unity���� ��ü�� ������ �� ���� ���� ȣ��ǹǷ�, Mangers��ü�� �ߺ� �������� �ʰ� �ϱ� ����
    {
        //���� awake�� ȣ��Ǿ��� �� �ٸ� Managers�ν��Ͻ��� �����ϸ� ���� ������ ��ü�� ����
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Init();//�ٸ� Managers �ν��Ͻ��� �������� ������ �ʱ�ȭ ����
    }

    static void Init()//�̱��� �ʱ�ȭ�� Ŭ���� �������� ����Ǳ� ������, Ŭ���� ������ �ٸ� �޼��峪 �ܺο��� ȣ���ϱ� ���ϵ��� static���� ����
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("Managers");
            if (go == null)
            {
                go = new GameObject { name = "Managers" };
                instance = go.AddComponent<Managers>();//go ���� �� �ٷ� Managers������Ʈ �߰�
            }
            else
            {
                instance = go.GetComponent<Managers>();
                if (instance == null)
                {
                    instance = go.AddComponent<Managers>();
                }
            }
            DontDestroyOnLoad(go);

        }
    }
    //�ٸ� Ŭ�������� Managers�� ����� �� : Managers mg = Managers.GetInstance();���·� ���
#if WhatIsManagers
    //���� ������ �����ϱ� ������ Static���� �����Ͽ� ���� ������ ������ �ν��Ͻ��� ����(���ϼ� ����)
    //�Ŵ��� ������Ʈ�� ���Ϲ��� �� �ִ� �Լ� GetInstance�� �ʿ�.
#endif
#if CaseOfInit
    //Init()�� ȣ��Ǵ� ���
    // 1. Managers�� �����Ǿ��� ��
    // 2. �ٸ� ��ũ��Ʈ���� GetInstance() ȣ�� �� ��üũ
#endif


}
