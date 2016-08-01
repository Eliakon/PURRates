using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    [SerializeField]
    private CatsManager catsManager;

    private Team localTeam;
    private Purrate selectedCat;

    private void Start()
    {
        Reset();

        localTeam = new Team(true, PurrateSkinManager.SkinColor.blue);
        catsManager.CreateCat(localTeam, 0, 0);
        catsManager.CreateCat(localTeam, 260, 130);

        var otherTeam = new Team(false, PurrateSkinManager.SkinColor.pink);
        catsManager.CreateCat(otherTeam, -260 / 2, -130 / 2);
        catsManager.CreateCat(otherTeam, -260 - 260/2, -130 + 130/2);
    }

    private void Reset()
    {
        catsManager.Reset();
    }

    public void SelectCat(Purrate cat)
    {
        if (cat.skinManager.Color != localTeam.skinColor)
        {
            if (selectedCat != null)
            {
                selectedCat.LaunchAttack(cat);
            }
            return;
        }

        if (selectedCat != null)
        {
            selectedCat.Select(false);
        }

        if (selectedCat != cat)
        {
            selectedCat = cat;
            cat.Select(true);
            return;
        }

        selectedCat = null;
    }

    public void MoveCat(SeaTile.Position target)
    {
        if (selectedCat != null)
        {
            selectedCat.Move(target.screen);
        }
    }
}
