using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;




public class Player : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] float RunningSpeed = 2;
    [SerializeField] float acceleration = 0.5f;
    [SerializeField] float jumpForce = 5f , gravityScale = 5f;
    [SerializeField] float duration = 0.5f; // how much sec to reach lane
    [SerializeField] float crouchScale = 1f, crouchDuration = 1.5f; // how much sec to reach lane

    [SerializeField] LayerMask groundLayer;

    [SerializeField] private InputManager inputs = null;
    [SerializeField] Animator playerAnimation;
    [SerializeField] GameObject scooter;
   
    Vector3 realGravity = Physics.gravity;
   
    float groundRaycastDistance = 0.5f;
    int score = 0 , stateCount = 0, quizeCounter = 0;
    float rs , upSpeed =0;
    int state = 1 ;  // 0 left  , 1 mid , 2 right
    int jumpState= 0;
    Vector3 direction = new Vector3(0, 0,1);
    Vector3 position ;

    CharacterController bodyC;
    BoxCollider bodyCollider;
    
    bool change = false, crouched = false, stopCrouching = false, crouching = false;

    float originalPosY , PrevPosY , constPosY;

    float[] lanes = Generator.splits;

    Coroutine c_move = null;
    Coroutine c_crouch = null;
    Coroutine c_updateJump = null;

    // Trying 

    float maxSlopeAngle = 50f , angle;
    RaycastHit slopeHit;
    bool afterSlope = false, jumping= false;

    // Start is called before the first frame update
    void Awake()
    {
        
        rs = RunningSpeed;
        constPosY = transform.position.y;
        bodyC = GetComponent<CharacterController>();
        
        inputs.Move += MoveBody;
        inputs.Jump += JumpBody;
        inputs.Crouch += GoDown;

        originalPosY = transform.position.y;
        c_crouch = StartCoroutine(UpdateSpeed());
    }


    void FixedUpdate(){


        if(jumping){
            upSpeed += Physics.gravity.y * gravityScale * Time.fixedDeltaTime;
        }
        
         if (Physics.Raycast(transform.position, Vector3.down, groundRaycastDistance, groundLayer )){
            if(jumping){
                jumping = false;
                transform.position = new Vector3(transform.position.x, constPosY, transform.position.z);
                upSpeed = 0;

            }
            
         }
         
         

    }

       // Update is called once per frame
    void Update()
    {
        
        
        position = transform.position;
        position.y = position.y + upSpeed * Time.deltaTime;
        FixPlayer();
        position.z = position.z + rs *direction.z * Time.deltaTime;

        transform.position = position;
        CheckCrouch();
        CheckPowerTimer();

        
        
    }
    

    void MoveBody(int state, bool change){
        if(change){
            if(!Powers.powerTimer && !jumping && !crouching){
                if(position.x < lanes[state]){
                    playerAnimation.SetTrigger("Right");
                }
                else{
                    playerAnimation.SetTrigger("Left");
                }
            }

            position.x = lanes[state];
            c_move = StartCoroutine(MoveSmoothly(transform.position, position));
        }
    }

    void JumpBody(bool jumped){
        if(jumped&& !jumping ){
                playerAnimation.SetTrigger("Jump");
           //if (Physics.Raycast(transform.position, Vector3.down, groundRaycastDistance, groundLayer )) {
                stopCrouching = true;
                upSpeed = jumpForce;
                jumping = true;
                
           // }
        }
    }
    
    void GoDown(bool c){
        if(c){
            if(jumping){
                upSpeed = -30f;
                
            }
            if(!crouching && !Powers.powerTimer){
                crouched = true;
                crouching = true;
            }
            
        }
    }
    void CheckCrouch(){
        if(crouched && !jumping){
            stopCrouching = false;
            playerAnimation.SetTrigger("Dive");
            c_crouch = StartCoroutine(Crouch());
            crouched = false;
        }
    }
 





    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Obstacle"))
        {

            // rs = 0;
            Time.timeScale = 0;
            if(quizeCounter == 0){
                GameManager.QuestionOn();
                quizeCounter++;
            }
            else{
                GameManager.GameOver();
            }
            


            transform.position = new Vector3(transform.position.x ,transform.position.y,other.transform.position.z - 1f);

        }
        else if(other.gameObject.CompareTag("Coin1") ){
            // score+=1;
            // scoreText.text = "Score: " + score.ToString();
            ScoreManager.UpdateScore(1);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Power")){
            
            playerAnimation.SetBool("Power", true);
            ScoreManager.UpdatePowerScore('p');
            scooter.SetActive(true);
            Powers.powerTimer = true;
            Powers.powerDuration = 10f;
            Destroy(other.transform.parent.gameObject);
        }
        else if (other.gameObject.CompareTag("Giga")){
            ScoreManager.UpdateScore(10);
            ScoreManager.UpdatePowerScore('g');
            Destroy(other.transform.parent.gameObject);
        }
        else if (other.gameObject.CompareTag("Deca")){
            ScoreManager.UpdateScore(5);
            Destroy(other.transform.parent.gameObject);
            ScoreManager.UpdatePowerScore('d');
        }        
        else if (other.gameObject.CompareTag("Easy")){
            ScoreManager.UpdateScore(1);
            Destroy(other.transform.parent.gameObject);
            ScoreManager.UpdatePowerScore('e');
        }


    }

    IEnumerator MoveSmoothly(Vector3 current , Vector3 end){
        float timeElaapsed = 0;
        Vector3 endCamera = end * 0.5f;
        while(timeElaapsed < duration){
            //Charcter
            position.y = position.y + direction.y * Time.fixedDeltaTime;
            current.z = current.z + rs *direction.z * Time.fixedDeltaTime;
            float t = timeElaapsed / duration;
            float newX = Mathf.Lerp(current.x , end.x, t);
            transform.position = new Vector3(newX, current.y, current.z);
           

            //Camera
            float newCameraX = Mathf.Lerp(cameraTransform.position.x , endCamera.x, t);
            cameraTransform.position = new Vector3(newCameraX, cameraTransform.position.y, cameraTransform.position.z);
            
            
            timeElaapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        // rs = RunningSpeed;
        change = false;
    }

    IEnumerator Crouch(){
        float tempY;
        Vector3 tempCenter , constScale, tempPos ;
        tempY = bodyC.height;
        tempCenter = bodyC.center;

        bodyC.height = crouchScale;
        bodyC.center -= new Vector3(0,crouchScale*0.5f, 0);
        float elapsedTime = 0f;
        while (elapsedTime < crouchDuration)
        {
            if (stopCrouching)
            {
                stopCrouching = false; // Reset the flag
                break; // Exit the loop to skip the wait
            }
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        crouching = false;
        bodyC.height = tempY;
        bodyC.center = tempCenter;



    }

    IEnumerator UpdateSpeed(){

        while (true)
            {
                yield return new WaitForSeconds(5f); // Wait for 10 seconds

                // Update the value
                rs += acceleration;

            }

    }



    bool OnSlope(){
        if(Physics.Raycast(transform.position, Vector3.down,out slopeHit, 1f)){
            angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            return angle < maxSlopeAngle && angle!=0 ;
        }
        return false;
    }
    void FixPlayer(){
        if(transform.position.y<constPosY){
            if(jumping){
            position.y = constPosY;
            
                jumping = false;
                upSpeed = 0;
                // playerAnimation.SetBool("Jumping", false);
            }
            
        }
    }
    void CheckPowerTimer(){
        if(Powers.powerTimer == false){
            scooter.SetActive(false);
            playerAnimation.SetBool("Power", false);
        }
    }



}

