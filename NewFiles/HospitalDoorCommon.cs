using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalDoorCommon : MonoBehaviour
{
    //�������� �ְ��� �ϴ� ��ü, ������� ������ �������� ����
    [SerializeField] private GameObject obj;
    [SerializeField] public float x, y, z;//���Ͱ� ���� ������ ��
    private Vector3 OpenPosition;//���� �� ��ġ
    private Vector3 ClosePosition;//���� ��ġ
    private float Speed = 0.1f;
    private bool Open = false;

    void Start()
    {
        obj = gameObject.GetComponent<GameObject>();
        ClosePosition = obj.transform.position;
        OpenPosition = new Vector3(OpenPosition.x +x, OpenPosition.y+y, OpenPosition.z + z);// ���� �� ��ġ�� ���� ��ġ �� xyz�� - �����ڰ� ������ xyz����ŭ ����. ������Ʈ ��ǥ�࿡ ���� -��, +���� �Ҵ�

    }

    private void OnTriggerEnter(Collider other)
    {
        if (obj != null && other.CompareTag("Player"))
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                Open = true;
            }
        }
    }

    void CalcPosition()
    {
        if (Open)
        {
            obj.transform.position = Vector3.Lerp(ClosePosition, OpenPosition, Time.deltaTime * Speed);
            if(Vector3.Distance(ClosePosition, OpenPosition) < 0.01) { ClosePosition = OpenPosition; Open = false; }
        }
    }

}
