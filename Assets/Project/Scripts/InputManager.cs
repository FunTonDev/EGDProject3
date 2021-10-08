using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum InputType { MouseKey, Controller };

public class InputManager : MonoBehaviour
{
    /*Input values: 
    inputX, inputY => [-1, 1]
    * => [0, 1]*/
    [Header("Button Input")]
    [SerializeField] public float inputX;      //A-D
    [SerializeField] public float inputY;      //W-S
    [SerializeField] public float inputSubmit; //Enter
    [SerializeField] public float inputCancel; //Escape
    [SerializeField] public float inputAlt1;   //Space
    [SerializeField] public float inputAlt2;   //Tab
    [SerializeField] public float inputAlt3;   //Shift
    [SerializeField] public float inputAlt4;   //Ctrl

    /*Input values:
     inputMX => [0, screen width]
     inputMY => [0, screen height]
     inputScroll => [-1, 1]
     * => [0, 1]*/
    [Header("Cursor Input")]
    [SerializeField] public float inputMX;     //Cursor X Pos
    [SerializeField] public float inputMY;     //Cursor Y Pos
    [SerializeField] public float inputScroll; //"Scroll"
    [SerializeField] public float inputFire1;  //LMB
    [SerializeField] public float inputFire2;  //RMB
    [SerializeField] public float inputFire3;  //MMB

    /*============================================================================
     * DEFAULT UNITY METHODS
     ============================================================================*/
    private void Update()
    {
        ButtonUpdate();
        CursorUpdate();
    }

    /*============================================================================
     * INPUT METHOD(S)
     ============================================================================*/
    private void ButtonUpdate()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        inputSubmit = Input.GetAxisRaw("Submit");
        inputCancel = Input.GetAxisRaw("Cancel");
        inputAlt1 = Input.GetAxisRaw("Alt1");
        inputAlt2 = Input.GetAxisRaw("Alt2");
        inputAlt3 = Input.GetAxisRaw("Alt3");
        inputAlt4 = Input.GetAxisRaw("Alt4");
    }

    private void CursorUpdate()
    {
        //UPDATE THIS LATER TO ACCOMODATE CONTROLLER JOYSTICK POSITION
        inputScroll = Input.GetAxisRaw("Mouse ScrollWheel");
        inputScroll /= Mathf.Abs(inputScroll);
        inputMX = Input.mousePosition.x;
        inputMY = Input.mousePosition.y;
        inputFire1 = Input.GetAxisRaw("Fire1");
        inputFire2 = Input.GetAxisRaw("Fire2");
        inputFire3 = Input.GetAxisRaw("Fire3");
    }
}
