using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pon : MonoBehaviour
{
    public List<int> n1 ;
    public List<int> n2 ;
    public List<int> n3 ;
    public List<int> n4 ;
    // Start is called before the first frame update
    void Start()
    {
        n1 = new List<int>();
        n2 = new List<int>();
        n3 = new List<int>();
        n4 = new List<int>();

        for(int i = 0; i<150; i++)
        {
            int t = Random.Range(0, 1);
            print("this");
            print(t);
        }
    }


}
