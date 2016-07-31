using System.Collections;
using System.Collections.Generic;

public class Team
{
    public readonly PurrateSkinManager.SkinColor skinColor;

    private bool isLocalPlayer;
    private List<Purrate> cats = new List<Purrate>();

    public Team(bool localPlayer, PurrateSkinManager.SkinColor color)
    {
        isLocalPlayer = localPlayer;
        skinColor = color;
    }

    public void AddCat(Purrate cat)
    {
        cats.Add(cat);
    }
}
