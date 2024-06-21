using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditSceneController : MonoBehaviour
{
    //ũ���� �� ��Ʈ�ѷ�. ��������� ������, esc�� ������ ��ŸƮ ������ ���ư���.
    public AudioSource ado;

    void Start()
    {
        ado = GetComponent<AudioSource>();
        if (ado != null && !ado.isPlaying)
        {
            ado.loop = true;
            ado.volume = 1.0f;
            ado.Play();

        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("StartScene");
        }
    }
}
