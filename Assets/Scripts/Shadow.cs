using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow: MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] float maxH = 10f;
    float targetY;

    void Update(){
        // HandleShadow();
    }

    void HandleShadow(){
        targetY = target.transform.position.y;
        targetY = targetY/maxH;
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }
}