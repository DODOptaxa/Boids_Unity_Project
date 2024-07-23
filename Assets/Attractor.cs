using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class Attractor : MonoBehaviour
{   static public Vector3 POS = Vector3.zero;
    public int num = 0;
    [Header("Set in Inspector")]
    public float radius = 10;
    public float xPhase = 0.5f;
    public float yPhase = 0.4f;
    public float zPhase = 0.1f;

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Rotate(0, 1f, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Rotate(0, -1f, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            GameObject child = this.transform.GetChild(0).gameObject;
            Vector3 pos = child.transform.localPosition;
            pos.z++;
            child.transform.localPosition = pos;
        }
        if (Input.GetKey(KeyCode.S))
        {
            GameObject child = this.transform.GetChild(0).gameObject;
            Vector3 pos = child.transform.localPosition;
            pos.z--;
            child.transform.localPosition = pos;
        }
    }
    void FixedUpdate()
    {
        num++;
        Vector3 tPos = GOpos;
        Vector3 scale = this.transform.localScale;
        tPos.x = Mathf.Sin(xPhase * Time.time) * radius * scale.x;
        tPos.y = Mathf.Sin(yPhase * Time.time) * radius * scale.y;
        tPos.z = Mathf.Sin(zPhase * Time.time) * radius * scale.z;
        GOpos = tPos;
        POS = tPos;
    }
    public Vector3 GOpos {
        get { return this.transform.position; }
        set { this.transform.position = value; }
    }    
        
    
}

