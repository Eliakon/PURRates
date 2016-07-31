using UnityEngine;
using System.Collections;

public class Cameraman : MonoBehaviour {

    public Transform cameraTransform;
    

    float moveVelocity = 0.1f / 0.02f;
    Vector2 movingVector = new Vector2(0, 0);

    public void StartMoveLeft() {
        movingVector.x = -1;
    }

    public void StopMoveLeft() {
        movingVector.x = 0;
    }

    public void StartMoveRight() {
        movingVector.x = 1;
    }

    public void StopMoveRight() {
        movingVector.x = 0;
    }

    public void StartMoveTop() {
        movingVector.y = 1;
    }

    public void StopMoveTop() {
        movingVector.y = 0;
    }

    public void StartMoveBottom() {
        movingVector.y = -1;
    }

    public void StopMoveBottom() {
        movingVector.y = 0;
    }

    void Update() {
        float gap = Time.deltaTime * moveVelocity;

        cameraTransform.position = new Vector3(
            cameraTransform.position.x + movingVector.x * gap,
            cameraTransform.position.y + movingVector.y * gap,
            cameraTransform.position.z
        );
    }


}
