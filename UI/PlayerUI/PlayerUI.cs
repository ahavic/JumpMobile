using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    static PlayerUI instance = null;
    [SerializeField] TextMeshProUGUI boosterAmountText = null;
    [SerializeField] TextMeshProUGUI forceText = null;
    [SerializeField] TextMeshProUGUI angleText = null;

    private void Awake()
    {
        instance = this;
    }

    public static void UpdateBoosterAmount(int amount)
    {
        instance.boosterAmountText.SetText(amount.ToString());
    }

    public static void UpdateCurrentJumpData(JumpData data)
    {
        instance.forceText.SetText("Force: " + data.forceData.ToString("0.0"));
        instance.angleText.SetText("Angle: " + data.angleData.ToString("0.0"));
    }
}
