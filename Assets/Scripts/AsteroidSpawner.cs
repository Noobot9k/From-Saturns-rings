using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {

    public GravitationalObject Player;
    public Transform SpawnFocus;
    public GravitationalObject AsteroidPrefab;

    [Header("Asteroids on scene load")]
    public int AsteroidsOnSceneLoad = 3;
    [Tooltip("AsteroidSpawnDistanceSceneLoad")]
    public MinMax AsteroidSpawnDistanceSceneLoad = new MinMax(5, 10);
    [Tooltip("AsteroidSpawnMassMultiplierOfPlayerOnSceneLoad")]
    public MinMax AsteroidSpawnMassMultiplierOfPlayerOnSceneLoad = new MinMax(.4f, .8f);

    [Header("Spawning asteroids")]
    public MinMax AsteroidSpawnDistance = new MinMax(13, 20);
    [Tooltip("AsteroidSpawnMassMultiplierOfPlayer_Smaller")]
    public MinMax AsteroidSpawnMassMultiplierOfPlayer_Smaller = new MinMax(.05f, .79f);
    [Tooltip("AsteroidSpawnMassMultiplierOfPlayer_Bigger")]
    public MinMax AsteroidSpawnMassMultiplierOfPlayer_Bigger = new MinMax(.8f, 7);
    [Range(0, 100), Tooltip("ChanceOfAsteroidBeingBiggerThanPlayer 0-100 percent")]
    public float ChanceOfAsteroidBeingBiggerThanPlayer = 8;
    public float AsteroidPushTowardFocus = 0;

    public int MaxAsteroids = 50;
    [Tooltip("MaxDistFromPlayerMultiplierOfMass")]
    public float MaxDistFromPlayerMultiplierOfScale = 40;

    //public MinMax SpawnRate = new MinMax(1, 10);
    [Range(0, 1)] //[Tooltip("Percent chance for a asteroid to spawn near the player per unit of speed the player is traveling at"), Range(0, 100)]
    public float Spawnrate = .5f;
    private float _nextAsteroidSpawn = 0;
    private Vector3 _playerLastPos = Vector3.zero;

    void Start() {
        if(SpawnFocus == null) SpawnFocus = Player.transform;
        for(int i = 0; i < AsteroidsOnSceneLoad; i++) {
            SpawnAsteroid(AsteroidSpawnDistanceSceneLoad, AsteroidSpawnMassMultiplierOfPlayerOnSceneLoad );
        }
    }

    void Update() {
        if(!Player) return;

        // randomly spawn asteroids nearby
        float distanceTraveled = Vector3.Distance(_playerLastPos, Player.transform.position);
        if (Random.Range(0, 100) < distanceTraveled * Spawnrate) {
            if (Random.Range(0, 100) < ChanceOfAsteroidBeingBiggerThanPlayer) {
                SpawnAsteroid(AsteroidSpawnDistance, AsteroidSpawnMassMultiplierOfPlayer_Bigger);
            } else {
                SpawnAsteroid(AsteroidSpawnDistance, AsteroidSpawnMassMultiplierOfPlayer_Smaller);
            }
        }

        //if (Time.time > _nextAsteroidSpawn) {
        //    _nextAsteroidSpawn = Time.time + Random.Range(SpawnRate.Min, SpawnRate.Max);
        //    SpawnAsteroid(AsteroidSpawnDistance, AsteroidSpawnMassMultiplierOfPlayer);
        //}

        // destroy far away asteroids
        foreach(Transform asteroid in transform) { //GameObject.FindGameObjectsWithTag("GravitationalObject")
            float distance = Vector3.Distance(Player.transform.position, asteroid.transform.position);
            if (distance > Player.transform.localScale.magnitude * MaxDistFromPlayerMultiplierOfScale) {
                Destroy(asteroid.gameObject);
            }
        }

    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        if(SpawnFocus == null) return;
        Gizmos.DrawWireSphere(SpawnFocus.position, SpawnFocus.localScale.magnitude * MaxDistFromPlayerMultiplierOfScale);
    }

    public void SpawnAsteroid(MinMax distanceFromPlayer, MinMax MassMultiplierOfPlayer) {
        float PlayerScale = Player.transform.localScale.magnitude;
        float PlayerMass = Player.GetMass();
        SpawnAsteroid(
            Random.Range(PlayerScale * distanceFromPlayer.Min, PlayerScale * distanceFromPlayer.Max),
            Random.Range(PlayerMass * MassMultiplierOfPlayer.Min, PlayerMass * MassMultiplierOfPlayer.Max));
    }
    public void SpawnAsteroid(float distanceFromPlayer, float Mass) {
        if(SpawnFocus == null) return;
        if(transform.childCount >= MaxAsteroids) return;

        GravitationalObject newRock = Instantiate<GravitationalObject>(AsteroidPrefab, transform);
        Rigidbody newRockRB = newRock.GetComponent<Rigidbody>();
        newRockRB.AddForce((SpawnFocus.position - newRock.transform.position).normalized * AsteroidPushTowardFocus, ForceMode.VelocityChange);
        newRock.SetMass(Mass);
        Vector2 RandomOffset = Random.insideUnitCircle;
        newRock.transform.position = SpawnFocus.position
            + new Vector3(RandomOffset.x, 0, RandomOffset.y).normalized
            * distanceFromPlayer;

    }
}

[System.Serializable]
public struct MinMax {
    public MinMax(float min, float max) {
        Min = min;
        Max = max;
    }
    public float Min;
    public float Max;

    public override string ToString() => $"({Min}, {Max})";
}