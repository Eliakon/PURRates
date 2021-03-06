﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CatsManager : MonoBehaviour
{
    [SerializeField]
    private Purrate purratePrefab;
    [SerializeField]
    private Transform canvas;

    private List<Purrate> allCats = new List<Purrate>();
    private bool sort;

    public void CreateCat(Team team, float coordX, float coordY)
    {
        var cat = Instantiate(purratePrefab);
        cat.rectTransform.parent = canvas;
        cat.rectTransform.localPosition = new Vector3(coordX, coordY, 0);
        cat.rectTransform.localScale = Vector3.one;
        cat.skinManager.SetColor(team.skinColor);
        team.AddCat(cat);
        allCats.Add(cat);
        sort = true;
    }

    public bool Sort
    {
        get
        {
            return sort;
        }
        set
        {
            sort = value;
        }
    }

    private void SortCats()
    {
        allCats.Sort(delegate(Purrate cat1, Purrate cat2) {
            return (int) (cat2.rectTransform.anchoredPosition.y - cat1.rectTransform.anchoredPosition.y);
        });

        for (var i = 0; i < allCats.Count; i++)
        {
            allCats[i].rectTransform.SetSiblingIndex(i);
        }
    }

    public void DestroyCat(Purrate cat)
    {

        allCats.Remove(cat);
        Destroy(cat.gameObject);
    }

    public void Reset()
    {
        for (var i = 0; i < allCats.Count; i++)
        {
            Destroy(allCats[i].gameObject);
        }
        allCats.Clear();
    }

    private void Update()
    {
        if (sort)
        {
            SortCats();
            sort = false;
        }
    }
}
