using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRotation : MonoBehaviour {

    public Vector3 RotationSpeed;

    void Start() {

    }
    void Update() {
        transform.rotation *= Quaternion.Euler(RotationSpeed * Time.unscaledDeltaTime);
    }
}
