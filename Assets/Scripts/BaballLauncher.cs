using UnityEngine;
using System.Collections;

public class BaballLauncher : MonoBehaviour {

    public Transform transform;
    public Transform otherTransform;
    public GameObject baballPrefab;

    void Update()
    {
        if ( Input.GetMouseButtonDown(0))
        {
            Instantiate(baballPrefab, transform.position, Quaternion.identity);
        }

    }

}
