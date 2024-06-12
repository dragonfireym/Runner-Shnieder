using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] Transform target;
    Vector3 difference;    
    // Start is called before the first frame update
    void Awake()
    {
        difference = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, difference.z + target.position.z);
    }




}
