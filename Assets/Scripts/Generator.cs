using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{//200 900
    [SerializeField] List<GameObject> objects , trains, cars,objectsEgy;
    [SerializeField] List<GameObject> planeEgy, bonusObjects;
    [SerializeField] GameObject firstPlane, plane , planeTunnel, planeBridge  , powerUp1;
    [SerializeField] Transform playerTransform;
    [SerializeField] float chanceForGoldenCoin = 0.2f;

    struct car
    {
        public GameObject c;
        public int speed;
    }   
    GameObject newObj, uniqueF;
    public static List<GameObject> Planes= new List<GameObject>();
    List<car> carsSpawned = new List<car>();
    float planeHalfLength;
    public static int latestIndex = 0;
    public static  int level = 1 ;
    int playerLatestIndex = 0;
    int counter = 0, trainCount = 0, xBarrierCount= 0, counterPlane = 0;
    Renderer rend;// = plane.GetComponent<Renderer>();
    Bounds boundaries;// = rend.bounds;
    float maxBound;
    Animator anime;
    int limit = 2  , planeEgyCount = 1; 
    bool off = false;

    public static float[] splits = new float[] {-3.4f, -0.22f , 2.73f} ;
    
    
    Quaternion obstacleRotation = Quaternion.Euler(0f, 90f, 0f) , trainRotation = Quaternion.Euler(0,180f,0);

    //[SerializeField] Transform lastLeft , lastRight;

    // Start is called before the first frame update
    void Awake()
    { 
        latestIndex = 0;
        level = 1;
        Planes.Clear();
        Planes.Add(firstPlane);
        Debug.Log("latest Index is  :"  + latestIndex);
        planeHalfLength = (Planes[latestIndex].transform.Find("lastBound").transform.position.z - Planes[latestIndex].transform.Find("firstBound").transform.position.z ) /2;

        maxBound = Planes[latestIndex].transform.Find("lastBound").transform.position.z;
        newObj = Instantiate(planeEgy[planeEgyCount] , new Vector3(plane.transform.position.x,plane.transform.position.y, maxBound+planeHalfLength) , transform.rotation);
        Planes.Add(newObj);
        latestIndex += 1;
        // Time.timeScale = 0;

        for(int i = 0; i <1 ; i++){
            createObjects(planeEgy[planeEgyCount]);
                    
            planeEgyCount = (planeEgyCount + 1) % 2;
        }
       
   

    }

    // Update is called once per frame
    void Update()
    {
        
        CheckPlayer();
        MoveCars();
        
    }
    void CheckPlayer(){
            if(playerTransform.position.z >= 3000){
                limit = 4;
                level = 4;
            }
            else if (playerTransform.position.z >= 1000){
                limit = 3;
                level = 3;
            }
            else if(playerTransform.position.z >= 400 ){
                limit = 2;
                level = 2;
            }
        maxBound = Planes[latestIndex].transform.Find("lastBound").transform.position.z;
        float old = Planes[latestIndex-1].transform.Find("lastBound").transform.position.z;

        if(playerTransform.position.z >= maxBound-220f){
            float oldBound = old;
            playerLatestIndex +=1;
            if(level >2){
                createObjects(plane);
            }
            else if(level == 2){

                if(counterPlane >2){
                    off = false;
                    createObjects(planeBridge);
                }
                else{

                    off = true;
                    if(counterPlane == 2){
                        createObjects(planeBridge);
                    }
                    else{
                        createObjects(planeTunnel);
                    }

                    counterPlane++;
                }
            }
            else if (level == 1){
                
                createObjects(planeEgy[planeEgyCount]);
                    
                planeEgyCount = (planeEgyCount + 1) % 2;
            }
        }
    }



    void createObjects(GameObject plane){

        maxBound = Planes[latestIndex].transform.Find("lastBound").transform.position.z;
        
        //GetBound(Planes[latestIndex], 'z', "max");
        int choice = 0;
        newObj = Instantiate(plane , new Vector3(plane.transform.position.x,plane.transform.position.y, maxBound+planeHalfLength) , transform.rotation);
        Planes.Add(newObj);
        if(counter > 1 && !off){
            for(int i = 0; i <3; i++){
                choice = Random.Range(1, limit); 
                if(choice == 1){
                    if(level != 1){
                        GenerateObstacles(splits[i] , objects);                       
                    }
                    else{
                        GenerateObstacles(splits[i] , objectsEgy);     
                    }

                }   
                else if (choice == 2){
                    GenerateTrain(splits[i]);
                }
                else if (choice == 3){
                    GenerateCars(splits[i]);
                }
            }
            trainCount = 0;
            xBarrierCount = 0;





            
        }
        else{
            counter ++;
        }
        

        
        latestIndex += 1;
        
        

    }
    void GenerateObstacles(float xPos, List<GameObject> objects){

        float[] zPositions = new float[] {0,0} ;  // First , Mid , Last
        float   yPos = -0.5f, tempY = 1f, factorY = 1.8f;
        float randomValue;

        // float maxBound = Planes[latestIndex].transform.Find("lastBound").transform.position.z;
        float maxBound =Planes[latestIndex].transform.Find("lastBound").transform.position.z ;
        float minBound =Planes[latestIndex].transform.Find("firstBound").transform.position.z ;
        float midBound = minBound+ (maxBound - minBound)/ 2;
        zPositions[0] =  maxBound;
        zPositions[1] = midBound;
    



        float zPos = zPositions[Random.Range(0, zPositions.Length)];
        // foreach (float zPos in zPositions){

            randomValue = Random.Range(0f, 1f);


            if(randomValue <=0.7){
                
                
                GameObject temp = objects[Random.Range(0, objects.Count)];

                if(temp.name.Contains("xbarrier") ){
                    if(xBarrierCount <=1 && trainCount <=1){
                        newObj = Instantiate( temp, new Vector3(xPos,yPos, zPos), obstacleRotation);
                        xBarrierCount++;
                    }
                }
                else{
                    newObj = Instantiate( temp, new Vector3(xPos,yPos, zPos), obstacleRotation);
                    if(!newObj.name.Contains("xbarrier") ){
                        tempY = newObj.transform.Find("maxHeight").transform.position.y;
                    }
                }
            randomValue = Random.Range(0f, 1f);
            if((!newObj.name.Contains("xbarrier")&&(!newObj.name.Contains("green"))) && randomValue <= 0.1){
                newObj = Instantiate(powerUp1 , new Vector3(xPos,tempY, zPos), powerUp1.transform.rotation);
                
                

                
                // anime = newObj.GetComponent<Animator>();
                // anime.Play("CoinAnim");
                // tempY = 1f;
            }
                
            }


            
        // }
    }

    void GenerateTrain(float xPos){
        float[] zPositions = new float[] {0,0} ;  // First , Mid , Last
        float  factorZ = 5f , yPos = -0.5f, tempY = 1f, factorY = 1.8f;
        float randomValue;

        // float maxBound = Planes[latestIndex].transform.Find("lastBound").transform.position.z;
        float maxBound =Planes[latestIndex].transform.Find("lastBound").transform.position.z ;
        float minBound =Planes[latestIndex].transform.Find("firstBound").transform.position.z ;
        float midBound = minBound+(maxBound - minBound)/ 2;
        zPositions[0] =  maxBound;
        zPositions[1] = midBound;
        float zPos = zPositions[Random.Range(0, zPositions.Length)];


        randomValue = Random.Range(0f, 1f);


        if(randomValue <=0.7 ){
        
            
            GameObject tempObj = trains[Random.Range(0, trains.Count)];


            trainCount++;


            if((trainCount <=1 && xBarrierCount <=1 )  ) {
                newObj = Instantiate(tempObj , new Vector3(xPos,yPos, zPos), tempObj.transform.rotation * trainRotation );
                //* new Quaternion (0,trainRotation, 0)
                // tempY = newObj.transform.Find("maxHeight").transform.position.y;
                trainCount++;
            }           

            
        }


    }
    void GenerateCars(float xPos){
        float zPos;  
        float  yPos = -0.5f;
        float randomValue ;
        int s;
        float maxBound =Planes[latestIndex].transform.Find("firstBound").transform.position.z ;
        zPos =  maxBound;

        randomValue = Random.Range(0f, 1f);
        s = Random.Range(3,6);


        if(randomValue <=0.7){
            
            
            GameObject temp = cars[Random.Range(0, objects.Count)];
            newObj = Instantiate( temp, new Vector3(xPos,yPos, zPos), temp.transform.rotation);
            carsSpawned.Add(new car { c = newObj, speed = s });

        }      
    }



    void MoveCars(){
        if(carsSpawned.Count > 0){
            for(int i = 0; i <carsSpawned.Count; i++){
                car selected = carsSpawned[i];
                if(selected.c != null){
                    if(selected.c.transform.position.z - playerTransform.position.z <= 60f){
                    int s = selected.speed;
                    Vector3 position = selected.c.transform.position;
                    position.z = position.z - s * Time.fixedDeltaTime;
                    selected.c.transform.position = position;
                    }
                }
                else{
                    carsSpawned.RemoveAt(i);
                }
                i++;

            }
        }
    }
}
