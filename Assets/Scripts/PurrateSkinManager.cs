using UnityEngine;
using System.Collections;

public class PurrateSkinManager : MonoBehaviour
{
    [System.Serializable]
    public enum SkinColor
    {
        blue,
        green,
        pink,
        yellow
    }

    [SerializeField]
    private SkinColor skinColor = SkinColor.yellow;
    /*{
        get
        {
            return color;
        }
        set
        {
            color = value;
            UpdateColor();
        }
    }*/

    private SkinColor color;

    private void UpdateColor()
    {
        Debug.Log("UPDATE COLOR");
    }
}
