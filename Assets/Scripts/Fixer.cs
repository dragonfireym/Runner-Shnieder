using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixer : MonoBehaviour
{

    [SerializeField] bool x= false , y = false, z= false;

    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StopBody();
    }

    void StopBody(){
        pos = transform.position;
        if(x){
            pos.x = 0;
        }

        if(y){
            pos.y = -0.4479f;
        }

        if(z){
            pos.z = 0;
        }

        transform.position = pos;
    }
}
