using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour {

    new Rigidbody rigidbody;
    GravitationalObject GravObj;
    public GameObject Visual;

    public float MaxJoinRelativeMass = 0.8f;
    public float Health = 10000;
    //public float MaxHealth = 10000;

    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        GravObj = GetComponent<GravitationalObject>();
    }
    void Update() {

    }

    private IEnumerator OnCollisionStay(Collision collision) {
        Rigidbody otherRB = collision.gameObject.GetComponent<Rigidbody>();
        GravitationalObject otherGravObj = collision.gameObject.GetComponent<GravitationalObject>();
        Player isPlayer = collision.gameObject.GetComponent<Player>();
        if(collision.gameObject.CompareTag("GravitationalObject") && otherRB && otherGravObj) {
            if(rigidbody.mass * MaxJoinRelativeMass > otherRB.mass) {
                //consume planet
                //GravObj.IncrementMass(otherRB.mass);
                Health -= otherGravObj.GetMass();
                print(Health);
                if(Health <= 0) {
                    StartCoroutine(GameControl.Singleton.Die(true));
                    Visual.SetActive(false);
                    GravObj.Consume(false);
                } else otherGravObj.Consume();
            } else {
                // or die trying
                Visual.SetActive(false);
                GravObj.Consume(false);
                if(isPlayer == null)
                    StartCoroutine(GameControl.Singleton.Die(true));
                else {
                    Time.timeScale = 0;
                    yield return new WaitForSecondsRealtime(3);
                    string dialog = DialogHandler.Singleton.DialogTable["KilledEarth"];
                    yield return StartCoroutine(DialogHandler.Singleton.RunDialog(dialog, true));
                    StartCoroutine(GameControl.Singleton.Die(true, true));
                }
            }
        }
        yield return null;
    }
}
