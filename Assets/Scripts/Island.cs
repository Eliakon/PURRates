using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Island : MonoBehaviour
{
    public List<IslandPart> parts;

    public void OnPointerEnterPart(IslandPart part) {}
    public void OnPointerExitPart(IslandPart part) {}
    public void OnClickPart(IslandPart part) {}
}