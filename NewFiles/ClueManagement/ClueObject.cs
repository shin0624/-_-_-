using UnityEngine;

public class ClueObject : MonoBehaviour
{
    public ClueData clueData;

    public void OnPickedUp()
    {
        Destroy(gameObject); // ������ ������Ʈ ����
    }
}
