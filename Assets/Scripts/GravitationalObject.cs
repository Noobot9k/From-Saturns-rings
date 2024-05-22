using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalObject : MonoBehaviour {

    new Rigidbody rigidbody;
    new Collider collider;
    ParticleSystemForceField particleGravity;
    ParticleSystem consumedParticles;

    [Range(0, 1)]
    public static float GravityMultiplier = .001f;
    public static float ParticleGravityRangeDivider = 15;
    public static float SizeToMassDivider = 20;
    public static float HorizonMultiplierOfScale = .9f;

    [SerializeField] float Mass = 100;

    void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        particleGravity = GetComponentInChildren<ParticleSystemForceField>();
        consumedParticles = GetComponentInChildren<ParticleSystem>();

        rigidbody.centerOfMass = Vector3.zero;

        UpdateMass();
    }
    void Update() {
        if (particleGravity) {
            //    particleGravity.endRange = rigidbody.mass / ParticleGravityRangeDivider;
            //particleGravity.gravity = new ParticleSystem.MinMaxCurve(rigidbody.mass / 100);
        }

        foreach(GameObject other in GameObject.FindGameObjectsWithTag("GravitationalObject")) {
            if(other == gameObject) continue;
            Rigidbody otherRB = other.GetComponent<Rigidbody>();
            Vector3 toOtherVector = (other.transform.position - transform.position);
            //float distanceToOther = Mathf.Max(toOtherVector.magnitude - (other.transform.localScale.x * HorizonMultiplierOfScale) - (transform.localScale.x * HorizonMultiplierOfScale), 1);
            float distanceToOther = toOtherVector.magnitude;
            if(otherRB && otherRB.isKinematic == false) {
                float gravitationalStrength = (otherRB.mass * rigidbody.mass * GravityMultiplier) / (distanceToOther * 2);
                rigidbody.AddForce(toOtherVector.normalized * gravitationalStrength, ForceMode.Force);
            }
        }
    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, transform.localScale.x * HorizonMultiplierOfScale);
    }

    public void IncrementMass(float deltaMass) {
        Mass += deltaMass;
        UpdateMass();
    }
    public void SetMass(float mass) {
        Mass = mass;
        UpdateMass();
    }
    public float GetMass() {
        return Mass;
    }
    void UpdateMass() {
        transform.localScale = Vector3.one * Mathf.Sqrt(Mass) / SizeToMassDivider;
        rigidbody.mass = Mass;
    }
    public void Consume(bool destroy = true) {
        rigidbody.isKinematic = true;
        collider.enabled = false;
        if(consumedParticles) {
            consumedParticles.transform.SetParent(null);
            consumedParticles.Emit(250);
            Destroy(consumedParticles.gameObject, 15);
        }
        if(particleGravity) particleGravity.gameObject.SetActive(false);
        if (destroy) Destroy(gameObject);
    }
}
