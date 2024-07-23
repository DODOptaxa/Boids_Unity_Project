using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    static public Spawner S;
    static public List<Boid> boids;


    private string path;
    public JsonFileParser jsonFile;

    [Header("Spawning")]
    public GameObject boidPrefab;
    public Transform boidAnchor;

    void Awake()
    {
        S = this;

        path = Application.streamingAssetsPath + "/config.json";
        jsonFile = JsonUtility.FromJson<JsonFileParser>(File.ReadAllText(path));

        boids = new List<Boid>();
        InstantiateBoid();
    }
    public void InstantiateBoid()
    {
        GameObject go = Instantiate(boidPrefab);
        Boid b = go.GetComponent<Boid>();
        b.transform.SetParent(boidAnchor);
        boids.Add(b);
        b.transform.rotation = Quaternion.Euler(180, 0, 0);
        if (boids.Count < jsonFile.Numboids)
        {
            Invoke("InstantiateBoid", jsonFile.SpawnDelay);
        }
    }
}
