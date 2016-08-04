using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IslandPart : MonoBehaviour
{

  public Island island;

  public void OnPointerEnter() {
    island.OnPointerEnterPart(this);
  }

  public void OnPointerExit() {
    island.OnPointerExitPart(this);
  }

  public void OnClick() {
    island.OnClickPart(this);
  }
}