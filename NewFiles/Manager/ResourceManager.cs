using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    //Resource�� Load, Instantiate, Destroy�� �����ϴ� ���ҽ� �Ŵ���

    //path�� �ִ� ������ �ε�. �ε� ������ Object�� ��
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    //����(Instantiate)
    //parent : �������� �����ؼ� ���� ��
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if(prefab == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }
        return Object.Instantiate(prefab, parent);
    }

    public void Destroy(GameObject obj)
    {
        if (obj == null) return;
        Object.Destroy(obj);
    }

    //--> �̱��� Managers�� ResourceManager �ν��Ͻ��� �����ϰ�, �ٸ� Ŭ�������� ������ �� �ֵ��� ������Ƽ ���·� �ļ�
    //--> �ٸ� Ŭ�������� �����Ϸ��� Managers.Resource.Instantiate("��θ�"); ���·� ���
}
