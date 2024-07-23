using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonFileParser
{
    public float Radius = 10f;
    public float xPhase = 0.5f;
    public float yPhase = 0.4f;
    public float zPhase = 0.1f;

    public float Numboids = 100;
    public float SpawnRadius = 100;
    public float SpawnDelay = 0.1f;

    public float Velocity = 50;
    public float NeighborDist = 25;
    public float CollDist = 50;
    public float VelMatching = 0.25f;
    public float FlockCentering = 0.2f;
    public float CollAvoid = 0.2f;
    public float AttractPull = 2;
    public float AttractPush = 2;
    public float AttractPushDist = 5;
}
