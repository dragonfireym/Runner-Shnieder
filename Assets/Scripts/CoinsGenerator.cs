using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsGenerator : MonoBehaviour
{
    [SerializeField] List<GameObject> contactors;

    [SerializeField] float gap = 3f, factorZ = 20f, groupCount = 3;




    float currentZ = 50f, startPoint, coinY;
    int numberOfComp = 2;
    float[] splits = Generator.splits;
    GameObject newObj= null, obj = null;
    Animator anime;
    // Start is called before the first frame update
    void Awake()
    {
        coinY = 0;
    }

    // Update is called once per frame
    void Update()
    {

        CheckSpawn();
    }

    void CheckSpawn(){
        int latestIndex = Generator.latestIndex;
        float zChecker = Generator.Planes[latestIndex].transform.Find("firstBound").transform.position.z;
        int lvl = Generator.level;
        
        if(currentZ < zChecker){
            currentZ = zChecker;
            // if(lvl == 1){
            //     obj = contactors[0];
            // }
            // else if (lvl == 2){
            //     obj = contactors[1];
            // }
            // else{
            //     obj = contactors[2];
            // }

            obj = contactors[Random.Range(0, contactors.Count)];
            SpawnCoins();
        }


    }

    void SpawnCoins(){
        float sel1, sel2;
        sel1 = splits[Random.Range(0, 3)];
        sel2 = splits[Random.Range(0, 3)];
        while(sel2 == sel1){
            sel2 = splits[Random.Range(0, 3)];
        }
        startPoint = currentZ + factorZ;
        int rand = Random.Range(0,numberOfComp);
        if(rand == 0){
            DoComp1(sel1, sel2, obj);
        }
        else if (rand == 1){
            DoComp2(sel1, sel2, obj);
        }
    }


    void DoComp1(float sel1, float sel2, GameObject coin){

        float tempZ = startPoint;
        for(int i = 0 ; i<groupCount; i++){
                newObj = Instantiate(coin , new Vector3(sel1,coinY, tempZ), coin.transform.rotation);
                // anime = newObj.GetComponent<Animator>();
                // anime.Play("CoinAnim");
                tempZ += gap;            
        }



        for(int i = 0 ; i<groupCount; i++){
            newObj = Instantiate(coin , new Vector3(sel2,coinY, tempZ), coin.transform.rotation);
            // anime = newObj.GetComponent<Animator>();
            // anime.Play("CoinAnim");
            tempZ += gap;            
        }
    }

    void DoComp2(float sel1, float sel2, GameObject coin){

        float tempZ1 = startPoint, tempZ2 = startPoint+gap;

        for(int i = 0 ; i<groupCount; i++){
                newObj = Instantiate(coin , new Vector3(sel1,coinY, tempZ1), coin.transform.rotation);
                // anime = newObj.GetComponent<Animator>();
                // anime.Play("CoinAnim");
                tempZ1 += gap*2;            
        }



        for(int i = 0 ; i<groupCount; i++){
            newObj = Instantiate(coin , new Vector3(sel2,coinY, tempZ2), coin.transform.rotation);
            // anime = newObj.GetComponent<Animator>();
            // anime.Play("CoinAnim");
            tempZ2 += gap*2;            
        }
    }


}
