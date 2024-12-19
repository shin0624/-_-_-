using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class JailNeonLampsSound : MonoBehaviour
{
    //���� 2�� �̺�Ʈ �� �׿·��� �̺�Ʈ ����

    private GameObject NeonLamp;
    private AudioSource zizizi;
    private Light PointLight;
    private Coroutine flickerCoroutine;//�ݺ��ϸ� ���������ϴ� ȿ���� ���� �ڷ�ƾ

    void Start()
    {
        NeonLamp = this.gameObject;//�� ��ũ��Ʈ�� �پ��ִ� ������Ʈ�� NeonLamp�� ����
        PointLight = NeonLamp.GetComponentInChildren<Light>();//NeonLamp�� �ڽ��� pointLight�� ����
        zizizi = NeonLamp.GetComponent<AudioSource>();//NeonLamp�� ���� ������ҽ��� ã��

        // ����Ʈ ����Ʈ�� ����� �ҽ��� ����� �����Ǿ����� Ȯ��
        if (PointLight == null)
        {
            Debug.LogError("Point Light is null");
        }
        else
        {
            Debug.Log("Point Light SET");
        }

        if (zizizi == null)
        {
            Debug.LogError("AudioSource is null");
        }
        else
        {
            Debug.Log("AudioSource SET");
        }
    }

    public void SetLightIntensity(float intensity)//����Ʈ����Ʈ�� ������ 0 ~ 1�� �����Ͽ� �����Ÿ��� ȿ���� ���� �޼���
    {
        if(PointLight!=null)
        {
            PointLight.intensity = Mathf.Clamp(intensity, 0.0f, 1.5f);

        }
    }

    public void PlayziziziSound()//������ �Ҹ� ��� �޼���
    {
        if (zizizi != null)
        {
            zizizi.Play();
        }
    }
    public void StopziziziSound()//������ �Ҹ� ���� �޼���
    {
        if (zizizi != null && zizizi.isPlaying)
        {
            zizizi.Stop();
        }
    }

    private IEnumerator FlickerLight(float interval)//����Ʈ����Ʈ�� ������ 0�� 1�� �ݺ��ϸ� �����̴� �ڷ�ƾ
    {
        while(true)
        {
            SetLightIntensity(0.0f);
            PlayziziziSound();
            //Debug.Log($"{gameObject} Light is Flickered");
            yield return new WaitForSeconds(interval);
            SetLightIntensity(1.5f);
            StopziziziSound();
            yield return new WaitForSeconds(interval);
        }
    }

    public void StartFlickering(float interval)//�����̱� ���� 
    {
        if(flickerCoroutine==null)
        {
            flickerCoroutine = StartCoroutine(FlickerLight(interval));
        }
    }

    public void StopFlickering()//�����̱� ����
    {
        if(flickerCoroutine!=null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
            SetLightIntensity(0);
            StopziziziSound();

        }
    }



}
