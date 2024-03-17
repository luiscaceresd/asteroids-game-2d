using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Asteroid asteroid;
    float spawnRate = 2f;
    float spawnDistance = 14f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("spawn", 0f, spawnRate);
    }

    void spawn()
    {
        //pick a random point to spawn astreroids from
        Vector2 spawnPoint = Random.insideUnitCircle.normalized * spawnDistance;

        //calculate a rotation arc
        float angle = Random.Range(-15f, 15f);
        //make a rotation transform
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //create a new asteroid
        Asteroid newAsteroid = Instantiate(asteroid, spawnPoint, rotation);
        //direction and size
        Vector2 direction = rotation * -spawnPoint.normalized;
        float mass = Random.Range(0.8f, 1.4f);
        newAsteroid.kick(mass, direction);
    }
}
