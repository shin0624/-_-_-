using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class ExitSceneController : MonoBehaviour
{
    //������ �� ��Ʈ�� ��ũ��Ʈ. ���� �� ������ �帣�� ui�� ǥ�õǸ� esc�� ������ ������ ����ȴ�. 
    public AudioSource ado;
    public VideoPlayer vdo;

    private VisualElement root;


    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        ado = GetComponent<AudioSource>();
        if(ado!=null && !ado.isPlaying)
        {
            ado.loop = true;
            ado.volume = 1.0f;
            ado.Play();
          
        }

        root.style.display = DisplayStyle.None;
        vdo = GetComponent<VideoPlayer>();
        if (vdo != null)
        {
            vdo.Play();
            vdo.loopPointReached += OnVideoFinished;
        }


    }

    private void Awake()
    {
        if(GetComponent<UIDocument>() == null)
        {
            Debug.LogError("UIDocument component(in ENDscene) not found!");
            return;
        }
    }

    private void OnVideoFinished(VideoPlayer vdo)
    {
        root.style.display = DisplayStyle.Flex;
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
