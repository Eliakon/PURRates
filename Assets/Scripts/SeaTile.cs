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
    Vector3Event TileClicked;

    public void HandleTileClicked()
    {
        TileClicked.Invoke(rectTransform.localPosition);
    }
}
