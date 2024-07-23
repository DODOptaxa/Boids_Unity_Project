using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Boid : MonoBehaviour
{
    private int emissionColorId;
    [Header("Set Dynamically")]
    public Rigidbody rigid;

    private Neighborhood neighborhood;
    void Awake()
    {   
        neighborhood = this.GetComponent<Neighborhood>();
        rigid = this.GetComponent<Rigidbody>();
        pos = Random.insideUnitSphere * Spawner.S.jsonFile.SpawnRadius;
        Vector3 vel = Random.onUnitSphere * Spawner.S.jsonFile.Velocity;
        print(Random.onUnitSphere);
        rigid.velocity = vel;
        int emissionColorId = Shader.PropertyToID("_EmissionColor");
        Color randColor = Color.black;
        while (randColor.r + randColor.g + randColor.b < 1.1f)
        {
            randColor = new Color(Random.value, Random.value, Random.value);
        }
        Renderer[] rends = this.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
        {
            PostProcessVolume postProcessVolume = r.GetComponent<PostProcessVolume>();
            if (postProcessVolume != null)
            {
                ColorParameter col = new ColorParameter();
                col.value = randColor;
                Bloom bloom = postProcessVolume.profile.GetSetting<Bloom>();
                bloom.color.Override(col);
                print(randColor);
            }
            r.material.color = randColor;
            r.material.SetColor(emissionColorId, randColor);
        }
        LookAhead();
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
        JsonFileParser spn = Spawner.S.jsonFile;

        //Предотвращение столкновений
        Vector3 velAvoid = Vector3.zero;
        Vector3 tooClosePos = neighborhood.avgClosePos;
        if (tooClosePos != Vector3.zero)
        {
            velAvoid = pos - tooClosePos;
            velAvoid.Normalize();
            velAvoid *= spn.Velocity;
        }

        //Согласование скорости
        Vector3 velAlign = neighborhood.avgVel;
        if (velAlign != Vector3.zero)
        {
            velAlign.Normalize();
            velAlign *= spn.Velocity;
        }

        //Движение в сторону центра группы
        Vector3 velCenter = neighborhood.avgPos;
        if (velCenter != Vector3.zero)
        {
            velCenter -= this.transform.position;
            velCenter.Normalize();
            velCenter *= spn.Velocity;
        }
        //Организовать движение в сторону attractor
        Vector3 delta = Attractor.POS - pos;

        //Определить, куда двигатся - к нему или от него
        bool attracted = (delta.magnitude > spn.AttractPushDist);
        Vector3 velAttract = delta.normalized * spn.Velocity;
        float fdt = Time.fixedDeltaTime;

        if (velAvoid != Vector3.zero)
        {
            vel = Vector3.Lerp(vel, velAvoid, spn.CollAvoid * fdt);
        }
        if (velAlign != Vector3.zero)
        {
            vel = Vector3.Lerp(vel, velAlign, spn.VelMatching * fdt);
        }
        if (velCenter != Vector3.zero)
        {
            vel = Vector3.Lerp(vel, velAlign, spn.FlockCentering * fdt);
        }
        if (attracted)
        {
            vel = Vector3.Lerp(vel, velAttract, spn.AttractPull * fdt);
        } else
        {
            vel = Vector3.Lerp(vel, -velAttract, spn.AttractPush * fdt);
        }

        vel = vel.normalized* spn.Velocity;

        rigid.velocity = vel;

        LookAhead();
    }
}
