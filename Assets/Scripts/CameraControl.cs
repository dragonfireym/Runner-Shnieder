using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] Transform target;
    Vector3 difference;
    Vector3 position ;
    float oldPosition , newPosition , constPos ;
    // Start is called before the first frame update
    void Awake()
    {
        difference = transform.position - target.position;
        oldPosition = transform.position.y;
        constPos = transform.position.y;
        newPosition = target.position.y;
        Debug.Log("old position is :  "+oldPosition + " new position is : "+ newPosition );
    }

    // Update is called once per frame
    void LateUpdate()
    {

        position = transform.position;

        if(oldPosition != newPosition){
            position.y = newPosition + constPos;
            oldPosition = newPosition;
        }
        

        transform.position = new Vector3(target.position.x, position.y, difference.z + target.position.z);

        newPosition = target.position.y;


    }
}
