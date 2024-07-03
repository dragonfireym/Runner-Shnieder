using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidesGenerator : MonoBehaviour
{
     [SerializeField] GameObject firstPlane;

    [SerializeField] List<GameObject> buildingsLeft, buildingsRight;
    [SerializeField] List<GameObject> buildingsLeft2,buildingsRight2 ;
    [SerializeField] List<GameObject> buildingsLeft3, buildingsRight3;
    [SerializeField] List<GameObject> buildingsLeft4, buildingsRight4;
    [SerializeField] List<GameObject> extraSides, extraSides2 , flags;




    // [SerializeField] Generator gen;


    GameObject newObj;
    int prevIndex = -1 , prevIndex2 = -1;
    float leftBound, rightBound, lastZLeft, lastZRight , leftBound2, rightBound2;
    float buildingCounter=0 ,treesCounter=0 , buildCountChoice , treesCountChoice ;
    float buildingCounter2=0 ,treesCounter2=0 ,buildCountChoice2 , treesCountChoice2;
    float factorZ = 20f,  factorZLvl1 = 30f , factorZLvl2 = 3f,factorZLvl3 = 100f;
    // Start is called before the first frame update
    void Awake()
    {

        lastZLeft = 15f;
        lastZRight = 15f;
        buildCountChoice = Random.Range(3,5);
        treesCountChoice = Random.Range(3,5);
        buildCountChoice2 = Random.Range(3,5);
        treesCountChoice2 = Random.Range(3,5);

        leftBound = firstPlane.transform.Find("leftBound").transform.position.x;
        rightBound = firstPlane.transform.Find("rightBound").transform.position.x;
        float factorX = 2;
        leftBound2 = leftBound+factorX ;
        rightBound2 = rightBound -factorX;


        for(int i = 0; i < 11; i++){
            GenerateSides(1, buildingsLeft3, buildingsRight3, null, null, factorZLvl1);
            GenerateSides(2, buildingsLeft3, buildingsRight3, null, null, factorZLvl1);
        }

    }

    // Update is called once per frame
    void Update()
    {
        CheckGeneration();
    }

    void CheckGeneration(){
        
        int latestIndex = Generator.latestIndex;
        float zChecker = Generator.Planes[latestIndex].transform.Find("lastBound").transform.position.z;
        int lev = Generator.level;
        GameObject uniqueF;

        if(Generator.Planes[latestIndex].name.Contains("tunnel")){
            lastZLeft = zChecker;
            lastZRight = zChecker;
        }
        else{
            if(lastZLeft <zChecker-factorZ){
                if (lev == 4){
                    uniqueF = extraSides[Random.Range(0, extraSides.Count)];
                    GenerateSides(1, buildingsLeft2, buildingsRight2, extraSides2, uniqueF, factorZLvl2);
                }
                else if(lev == 2){
                    lastZLeft = zChecker;
                }
                else if(lev == 3){

                        GameObject flag = flags[Random.Range(0, flags.Count)];
                        GenerateSides(1, buildingsLeft, buildingsRight,extraSides, flag, factorZLvl2);
                    // }
                    
                }
                else if (lev == 1){
                    GenerateSides(1, buildingsLeft3, buildingsRight3, null, null, factorZLvl1);
                }
            }



            if(lastZRight <zChecker -factorZ){
                if (lev == 4){
                    uniqueF = extraSides[Random.Range(0, extraSides.Count)];
                    GenerateSides(2, buildingsLeft2, buildingsRight2, extraSides2, uniqueF,factorZLvl2);
                }
                else if(lev == 2){
                    lastZRight = zChecker;
                }
                else if(lev == 3){
                        GameObject flag = flags[Random.Range(0, flags.Count)];
                        GenerateSides(2, buildingsLeft, buildingsRight,extraSides, flag,factorZLvl2);

                }
                else if (lev == 1){
                    GenerateSides(2, buildingsLeft3, buildingsRight3, null, null, factorZLvl1);
                }          
            }
        }


       
    }

    void GenerateSides(int choice , List<GameObject> buildingsL, List<GameObject> buildingsR ,  List<GameObject> extraSides, GameObject flag, float factorZ) {
        // 3 4 building >> tree
        //  3 4 tree >> flag
        // factoz 3
        
        float y = -0.5f;
        int sel = -1;
        GameObject temp, temp2;
        float x ,z ,x2;
        Quaternion flagRotation = Quaternion.Euler(0,0,0);
        if(choice == 1){
            sel = Random.Range(0, buildingsL.Count);
            buildingCounter++;
            while(sel == prevIndex){
                sel = Random.Range(0, buildingsL.Count);
            }
            prevIndex = sel;
            temp= buildingsL[sel];
            x = leftBound;
            z = lastZLeft +factorZ;
            x2 = leftBound2;
            if(flag != null){
                flagRotation = flag.transform.rotation;
            }

        }
        else{
            sel =   Random.Range(0, buildingsR.Count);
            while(sel == prevIndex2){
                sel = Random.Range(0, buildingsR.Count);
            }
            prevIndex2 = sel;
            buildingCounter2++;
            temp = buildingsR[sel];
            x = rightBound;
            z = lastZRight +factorZ; 
            x2 = rightBound2;
            if(flag != null){
                flagRotation =  Quaternion.Euler(-90f,-20f,0);
            }

            
        }
        

        newObj = Instantiate(temp , new Vector3(x,y, z), temp.transform.rotation );

        float tempZ = newObj.transform.Find("lastZ").transform.position.z;

        if(buildingCounter == buildCountChoice ){ //choice 1
            if(treesCounter == treesCountChoice && flag != null){
                
                newObj = Instantiate(flag , new Vector3(x2,y, z-1.5f), flagRotation );

                treesCountChoice = Random.Range(3,5);
                treesCounter =0;
                
            }
            else if(extraSides!= null) {
                temp2 = extraSides[Random.Range(0, extraSides.Count)];
                newObj = Instantiate(temp2 , new Vector3(x2,y, z-1.5f), temp2.transform.rotation );

                treesCounter++;
                buildCountChoice = Random.Range(3,5);
                
            }
            buildingCounter =0;
        }

        if(buildingCounter2 == buildCountChoice2 ){ //choice 2
            if(treesCounter2 == treesCountChoice2 && flag != null){
                
                newObj = Instantiate(flag , new Vector3(x2,y, z-1.5f), flagRotation );

                treesCountChoice2 = Random.Range(3,5);
                treesCounter2 =0;
                
            }
            else if(extraSides!= null){
                temp2 = extraSides[Random.Range(0, extraSides.Count)];
                newObj = Instantiate(temp2 , new Vector3(x2,y, z-1.5f), temp2.transform.rotation );

                treesCounter2++;
                buildCountChoice2 = Random.Range(3,5);
            }
            buildingCounter2 =0;
        }


        

        if(choice == 1){
            lastZLeft = tempZ;
        }
        else{
            lastZRight = tempZ;
        }
    }
}
