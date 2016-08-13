using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class IslandPart : MonoBehaviour
{

  public Island island;
  public Image image;

  public void ChangeMaterial(Material material) {
    image.material = material;
  }

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