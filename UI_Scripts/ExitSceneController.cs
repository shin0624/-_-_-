using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSceneController : MonoBehaviour
{
    //������ �� ��Ʈ�� ��ũ��Ʈ. ���� �� ������ �帣�� ui�� ǥ�õǸ� esc�� ������ ������ ����ȴ�. 
    public AudioSource ado;

    void Start()
    {
        ado = GetComponent<AudioSource>();
        if(ado!=null && !ado.isPlaying)
        {
            ado.loop = true;
            ado.volume = 1.0f;
            ado.Play();
          
        }
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
