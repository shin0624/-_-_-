using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Compilation;
using UnityEngine;

//���Ƚǿ��� ������ �ø��� 2������ ���� ����� ���� ���Ͱ� �ö󰣴�. �̸� �����ϱ� ���� ��ũ��Ʈ.

public class AutoShutterScript : MonoBehaviour
{ 
    public bool RaiseShutter;//���Ͱ� �ö󰬴��� ����
    private OpenShieldController _sh;//���Ƚ� ������ ��ũ��Ʈ���� ������ �ö󰬴��� ������ Ȯ���Ͽ� ����
    private Vector3 RaisPosition;//���� �ö󰡴� ��ġ����
    public float RaiseLerpTime = 0.1f;//�����ð�
    public AudioSource Ado;//���� ȿ����


    void Start()
    {
        RaiseShutter = false;
        RaisPosition = transform.position + new Vector3(0, 2.9f, 0);//y������ 2.96 +�Ǿ�� �� ����

        _sh = GameObject.FindFirstObjectByType<OpenShieldController>();

        if(_sh==null)
        {
            Debug.Log("OpenShieldController object not found");
        }

        Ado = GetComponent<AudioSource>();//������ҽ� ������Ʈ�� ã�´�. �� ��ũ��Ʈ�� ������ ������Ʈ���� ã���Ƿ� GameObject�� ���� x
        if (Ado == null)
        {
            Debug.LogError("AudioSource component not assigned!");
            
        }
    }

    
    void Update()
    {
        if (_sh.leverRaised == true)//������ ������ �ö󰡸�
        {
            Raise();
        }

        if(RaiseShutter && Vector3.Distance(transform.position, RaisPosition) <0.01f)
        {
            _sh.ResetLever(); //���Ͱ� ��ǥ ��ġ���� �ö�Դٸ� leverRaised���� false�� ����. �̷��� �ؾ� ���� �ݺ������ �ȵȴ�.
        }

    }

    void Raise()
    {
            Debug.Log(" 1F Shutter Raised! ");
            transform.position = Vector3.MoveTowards(transform.position, RaisPosition, Time.deltaTime * RaiseLerpTime);//������ �������� �����. �����Լ��� �����Ͽ� �ڿ������� ���
            RaiseShutter = true;
             Debug.Log($" Position : {transform.position}");
                if (Ado != null && !Ado.isPlaying)
                {
                    Ado.Play();
                    Ado.loop = false;
                }

    }
}
