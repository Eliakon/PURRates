using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class SeaTile : MonoBehaviour
{
    public class Position {
        public Vector2 world;
        public Vector3 screen;

        public Position(SeaTile tile) {
            this.world = tile.worldPosition;
            this.screen = tile.rectTransform.localPosition;
        }
    }

    [System.Serializable]
    public class Vector3Event: UnityEvent<Vector3> {}

    public RectTransform rectTransform;
    public Game game;
    public Vector2 worldPosition;

    public Position position;

    void Awake() {
        position = new Position(this);
    }


    public void HandleTileClicked()
    {
        game.MoveCat(position);
    }
}
