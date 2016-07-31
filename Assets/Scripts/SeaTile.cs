using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class SeaTile : MonoBehaviour
{
    [System.Serializable]
    public class Vector3Event: UnityEvent<Vector3> {}

    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private Game game;

    public void HandleTileClicked()
    {
        game.MoveCat(rectTransform.localPosition);
    }
}
