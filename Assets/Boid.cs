using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Rigidbody rigid;

    private Neighborhood neighborhood;
    void Awake()
    {   
        neighborhood = this.GetComponent<Neighborhood>();
        rigid = this.GetComponent<Rigidbody>();
        pos = Random.insideUnitSphere * Spawner.S.spawnRadius;
        Vector3 vel = Random.onUnitSphere * Spawner.S.veloncity;
        print(Random.onUnitSphere);
        rigid.velocity = vel;

        LookAhead();

        Color randColor = Color.black;
        while ( randColor.r + randColor.g + randColor.b < 1.0f)
        {
            randColor = new Color(Random.value, Random.value, Random.value);
        }
        Renderer[] rends = this.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
        {
            r.material.color = randColor;
        }
        TrailRenderer tRend = this.GetComponent<TrailRenderer>();
        tRend.material.SetColor("_TintColor", randColor);
    }
    void LookAhead()
    {
        transform.LookAt(pos + rigid.velocity);
    }
    public Vector3 pos
    {
        get { return this.transform.position; }
        set { transform.position = value; }
    }
    void FixedUpdate()
    {
        Vector3 vel = rigid.velocity;
        Spawner spn = Spawner.S;

        //Предотвращение столкновений!
        Vector3 velAvoid = Vector3.zero;
        Vector3 tooClosePos = neighborhood.avgClosePos;
        if (tooClosePos != Vector3.zero)
        {
            velAvoid = pos - tooClosePos;
            velAvoid.Normalize();
            velAvoid *= spn.veloncity;
        }

        //Согласование скорости!
        Vector3 velAlign = neighborhood.avgVel;
        if (velAlign != Vector3.zero)
        {
            velAlign.Normalize();
            velAlign *= spn.veloncity;
        }

        //Движение в сторону центра группы
        Vector3 velCenter = neighborhood.avgPos;
        if (velCenter != Vector3.zero)
        {
            velCenter -= this.transform.position;
            velCenter.Normalize();
            velCenter *= spn.veloncity;
        }
        //Организовать движение в сторону attractor
        Vector3 delta = Attractor.POS - pos;
        //Определить, куда двигатся - к нему или от него
        bool attracted = (delta.magnitude > spn.attractPushDist);
        Vector3 velAttract = delta.normalized * spn.veloncity;
        float fdt = Time.fixedDeltaTime;

        if (velAvoid != Vector3.zero)
        {
            vel = Vector3.Lerp(vel, velAvoid, spn.collAvoid * fdt);
        }
        if (velAlign != Vector3.zero)
        {
            vel = Vector3.Lerp(vel, velAlign, spn.velMatching * fdt);
        }
        if (velCenter != Vector3.zero)
        {
            vel = Vector3.Lerp(vel, velAlign, spn.flockCentering * fdt);
        }
        if (attracted)
        {
            vel = Vector3.Lerp(vel, velAttract, spn.attractPull * fdt);
        } else
        {
            vel = Vector3.Lerp(vel, -velAttract, spn.attractPush * fdt);
        }

        vel = vel.normalized* spn.veloncity;

        rigid.velocity = vel;

        LookAhead();
    }
}
