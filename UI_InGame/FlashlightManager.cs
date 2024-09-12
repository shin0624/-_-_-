using UnityEngine;
using TMPro;

public class FlashlightManager : MonoBehaviour
{
    public TextMeshProUGUI batteryText;  // ���͸� ���¸� ǥ���� UI �ؽ�Ʈ
    public float batteryPercentage = 100f;  // �ʱ� ���͸� �뷮
    public float drainInterval = 0.01f;  // ���͸� �Ҹ� ���� (��)
    public float drainAmount = 0.00556f;  // ���͸� �Ҹ� // 0.00556 3��
    private bool isFlashlightOn = false;  // ������ ���� ����

    private float timeSinceLastDrain;

    void Update()
    {
        if (isFlashlightOn)
        {
            timeSinceLastDrain += Time.deltaTime;

            if (timeSinceLastDrain >= drainInterval)
            {
                timeSinceLastDrain -= drainInterval;  // �ð� ������ �ʱ�ȭ
                DrainBattery();
                UpdateBatteryText();
            }
        }
    }

    public void SetFlashlightState(bool isOn)
    {
        isFlashlightOn = isOn;
    }

    void DrainBattery()
    {
        if (batteryPercentage > 0)
        {
            batteryPercentage -= drainAmount;
            if (batteryPercentage < 0) batteryPercentage = 0;
        }
    }

    void UpdateBatteryText()
    {
        // ���͸� ���¸� �Ҽ��� ���� ǥ��
        batteryText.text = Mathf.FloorToInt(batteryPercentage).ToString() + "%";
    }
}
