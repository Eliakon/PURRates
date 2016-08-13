using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Island : MonoBehaviour
{
    public List<IslandPart> parts;
    public Material enlightedMaterial;
    public Material normalMaterial;

    private bool isEnlighted = false;
    private int shouldResetEnlight = 0;


    public void OnPointerEnterPart(IslandPart sourcePart) {
        if (isEnlighted) return;

        isEnlighted = true;
        shouldResetEnlight = 0;

        foreach (IslandPart part in parts) {
            part.ChangeMaterial(enlightedMaterial);
        }
    }

    public void OnPointerExitPart(IslandPart sourcePart) {
        if (isEnlighted) shouldResetEnlight = 5;
    }

    public void OnClickPart(IslandPart sourcePart) {

    }

    void Update() {
        if (isEnlighted && shouldResetEnlight > 0) {
            shouldResetEnlight--;

            if (shouldResetEnlight == 0) {
                foreach (IslandPart part in parts) {
                    part.ChangeMaterial(normalMaterial);
                }

                isEnlighted = false;
            }
        }
    }
}