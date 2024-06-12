using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powers : MonoBehaviour
{
    [SerializeField] float alpha = 0.6f;
    [SerializeField] GameObject islandOverLay, smokeParticle;

     public static float  powerDuration = 10f;

    float currentAlpha = 1f;
    
    Coroutine c_powerTimer = null;
    MeshRenderer mesh;
    Color color;
    BoxCollider me;
    bool power1 = false;
    public static bool powerTimer= false;
    // Start is called before the first frame update
    void Awake()
    {
        me = GetComponent<BoxCollider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        CheckPowers();
    }


    void OnTriggerEnter(Collider other){
        if( other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("NoObstacle")){
            TransparentPowerHandler(other);
            Debug.Log("ohhhh");
        }
    }

    void TransparentPowerHandler(Collider other){
        if(power1){
            other.gameObject.tag = "NoObstacle";
        }
        else{
            other.gameObject.tag = "Obstacle";
        }
        
        Transform parent = other.transform.parent;

        foreach(Transform child in parent){
            if(child.GetComponent<MeshRenderer>() != null){
                mesh = child.GetComponent<MeshRenderer>();
        

                Color redColor = new Color(1f, 0f, 0f, alpha); 
                            
                color = mesh.sharedMaterial.GetColor("_BaseColor");
                color.a = currentAlpha;
                
                mesh.material.SetColor("_BaseColor", color);                        
            }
        }          
    }
    void CheckPowers(){
        if(powerTimer && !power1){
            if(c_powerTimer != null){
                StopCoroutine(c_powerTimer);
            }
            power1 = true;
            currentAlpha = alpha;
            c_powerTimer = StartCoroutine(StartPowerTime());
            // powerTimer = false;
            me.enabled = false;
            me.enabled = true;
        }
    }




    IEnumerator StartPowerTime(){
        float elapsedTime = 0f;
        smokeParticle.SetActive(false);
        islandOverLay.SetActive(true);
        smokeParticle.SetActive(true);
        while (elapsedTime < powerDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        islandOverLay.SetActive(false);
        smokeParticle.SetActive(false);
        smokeParticle.SetActive(true);
        power1 = false;
        currentAlpha = 1;
        me.enabled = false;
        me.enabled = true;
        powerTimer = false;
    }

}
