using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    new Rigidbody rigidbody;
    GravitationalObject gravitationalObject;

    public float Acceleration = 5;
    public float MaxJoinRelativeMass = 0.8f;

    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        gravitationalObject = GetComponent<GravitationalObject>();
    }
    void Update() {
        float InputX = Input.GetAxis("Horizontal");
        float InputY = Input.GetAxis("Vertical");
        Vector3 InputVector = Vector3.ClampMagnitude(new Vector3(InputX, 0, InputY), 1);
        rigidbody.AddForce(InputVector * Acceleration, ForceMode.Acceleration);

    }

    private void OnCollisionStay(Collision collision) {
        Rigidbody otherRB = collision.gameObject.GetComponent<Rigidbody>();
        GravitationalObject otherGravObj = collision.gameObject.GetComponent<GravitationalObject>();
        Earth isEarth = collision.gameObject.GetComponent<Earth>();
        if(collision.gameObject.CompareTag("GravitationalObject") && otherRB && otherGravObj) {
            if (rigidbody.mass * MaxJoinRelativeMass > otherRB.mass) {
                if(isEarth != null) return;
                //consume planet
                gravitationalObject.IncrementMass(otherRB.mass);
                otherGravObj.Consume();
            } else {
                // or die trying
                StartCoroutine(GameControl.Singleton.Die());
                gravitationalObject.Consume();
            }
        }
    }
}
