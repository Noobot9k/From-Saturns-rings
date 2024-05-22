using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform Target;
    public float ViewDistance = 12;
    public float LerpSpeed = 10;
    public float VelocityLeadMultiplier = .5f;

    void Start() {
    }
    void LateUpdate() {
        if(!Target) return;
        Vector3 targetPosition = Target.position + -transform.forward * Target.localScale.magnitude * ViewDistance;
        transform.position = Vector3.Lerp(transform.position, targetPosition, LerpSpeed * Time.unscaledDeltaTime);
    }
}
