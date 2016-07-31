using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PurrateSkinManager : MonoBehaviour
{
    public enum SkinColor
    {
        blue = 0,
        green = 1,
        pink = 2,
        yellow = 3
    }

    [SerializeField]
    private SkinColor skinColor = SkinColor.yellow;
    [SerializeField]
    private Sprite[] hatSkins;
    [SerializeField]
    private Sprite[] flagSkins;
    [SerializeField]
    private Color[] colorSkins;
    [SerializeField]
    private Image hat;
    [SerializeField]
    private Image flag;
    [SerializeField]
    private Image selection;

    public SkinColor Color
    {
        get
        {
            return skinColor;
        }
    }

    private void OnValidate()
    {
        SetColor(skinColor);
    }

    public void SetColor(SkinColor color)
    {
        if (skinColor != color)
        {
            skinColor = color;
            hat.sprite = hatSkins[(int)skinColor];
            flag.sprite = flagSkins[(int)skinColor];
            selection.color = colorSkins[(int)skinColor];
        }
    }
}
