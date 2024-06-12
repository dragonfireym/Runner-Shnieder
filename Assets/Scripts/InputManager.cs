using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public Action<int, bool> Move = null;
    public Action<bool> Jump = null;
    public Action<bool> Crouch = null;

    int state = 1;
    bool change= false, jumped= false, crouched = false;

    Vector2 startTouchPos , endTouchPos;

    bool swipedUp = false , swipedDown = false, swipedLeft = false, swipedRight = false;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForTouch();
        dispatchMoveAction();
        dispatchJumpAction();
        dispatchCrouchAction();
    }
    void dispatchMoveAction(){
        if(Move == null){
            return;
        }

        if (Input.GetKeyDown(KeyCode.A) || SwipeManager.swipeLeft){
            swipedLeft = false;
            if(state != 0){
                state = Mathf.Clamp(state - 1, 0, 2);
                change = true;
            }

         }
        else if (Input.GetKeyDown(KeyCode.D)|| SwipeManager.swipeRight){
            swipedRight = false;
            if(state != 2){
                state = Mathf.Clamp(state + 1, 0, 2);
                change = true;
            }

         }
        else{
             change = false;
         }

         Move(state, change);
    }

    void dispatchJumpAction(){
        if(Jump == null){
            return;
        }

        if((Input.GetKeyDown(KeyCode.W) || SwipeManager.swipeUp)&& !jumped ){
            swipedUp = false;
            jumped = true;
        }
        else{
            jumped  = false;
        }

        Jump(jumped);
    }

    void dispatchCrouchAction(){
        if(Crouch == null){
            return;
        }

        if((Input.GetKeyDown(KeyCode.S) || SwipeManager.swipeDown )&& !crouched ){
            swipedDown = false;
            crouched = true;
        }
        else{
            crouched  = false;
        }

        Crouch(crouched);

    }

    void CheckForTouch(){
        bool v = false , h= false;
        if(Input.touchCount >0 && Input.GetTouch(0).phase == TouchPhase.Began){
            startTouchPos = Input.GetTouch(0).position;
        }
        if(Input.touchCount >0 && Input.GetTouch(0).phase == TouchPhase.Ended){
            endTouchPos = Input.GetTouch(0).position;

            if (endTouchPos.x > startTouchPos.x){
                h = true;
                swipedRight = true;

            }
            else if (endTouchPos.x < startTouchPos.x){
                h = false;
                swipedLeft = true;

            }            
            if(endTouchPos.y > startTouchPos.y){
                v = true;
                swipedUp = true;

            }
            else if (endTouchPos.y < startTouchPos.y){
                v = false;
                swipedDown = true;

            }







            // if(!v && h){
            //     if(Math.Abs(endTouchPos.x) > Math.Abs(endTouchPos.y)){
            //         swipedRight = true;
            //     }
            //     else{
            //         swipedDown = true;
            //     }
            // }
            // else if (v && !h){
            //     if(Math.Abs(endTouchPos.x) > Math.Abs(endTouchPos.y)){
            //         swipedLeft = true;
            //     }
            //     else{
            //         swipedUp = true;
            //     }
            // }
            // else if (v && h ){
            //     if(Math.Abs(endTouchPos.x) > Math.Abs(endTouchPos.y)){
            //         swipedRight = true;
            //     }
            //     else{
            //         swipedUp = true;
            //     }                
            // }
            // else if (!v && !h){
            //     if(Math.Abs(endTouchPos.x) > Math.Abs(endTouchPos.y)){
            //         swipedLeft = true;
            //     }
            //     else{
            //         swipedDown = true;
            //     }
            // }



        }
    }
}
