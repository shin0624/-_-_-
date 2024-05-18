using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightContoller : MonoBehaviour
{
    bool GetLight;
    Light LightComponent;
  
    void Start()
    {
        GetLight = false;
        LightComponent = this.GetComponent<Light>();//������(flashlight) ������Ʈ�� ���� Light������Ʈ�� �ҷ��´�.
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            GetLight = GetLight ? false : true;
            //FŰ�� �������� �¿����Ѵ�. ó�� ���� �� �������� false�����̹Ƿ� True�� ��ȯ�ϸ� �������� ���� ��. false�� �� true�� ��ȯ�ؾ� FŰ �ϳ��� on/off������ �����ϴ�.
            //(true�� �� false�� ��ȯ�ؾ� �������� ���� ���̹Ƿ�) 
        }
        if(GetLight==false)//������ ����
        {
            LightComponent.intensity = 0;
        }
        if(GetLight ==true)//������ ����
        {
            LightComponent.intensity = 3;
        }
    }
}
