using UnityEngine;
using TMPro;
using BlockCoding;

public class CommandButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI label;

    public void Initialize(CommandType type)
    {
        if (label != null)
            label.text = type.ToString();
    }
}