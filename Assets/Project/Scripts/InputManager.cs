using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    /*Input values: 
    inputX, inputY => [-1, 1]
    * => [0, 1]*/
    [Header("Button Input")]
    [SerializeField] public float inputX;      //A-D
    [SerializeField] public bool inputX_U;
    [SerializeField] public bool inputX_D;
    [SerializeField] public float inputY;      //W-S
    [SerializeField] public bool inputY_U;
    [SerializeField] public bool inputY_D;
    [SerializeField] public float inputSubmit; //Enter
    [SerializeField] public bool inputSubmit_U;
    [SerializeField] public bool inputSubmit_D;
    [SerializeField] public float inputCancel; //Escape
    [SerializeField] public bool inputCancel_U;
    [SerializeField] public bool inputCancel_D;
    [SerializeField] public float inputAlt1;   //Space
    [SerializeField] public bool inputAlt1_U;
    [SerializeField] public bool inputAlt1_D;
    [SerializeField] public float inputAlt2;   //Tab
    [SerializeField] public bool inputAlt2_U;
    [SerializeField] public bool inputAlt2_D;
    [SerializeField] public float inputAlt3;   //Shift
    [SerializeField] public bool inputAlt3_U;
    [SerializeField] public bool inputAlt3_D;
    [SerializeField] public float inputAlt4;   //Ctrl
    [SerializeField] public bool inputAlt4_U;
    [SerializeField] public bool inputAlt4_D;

    /*Input values:
     inputMX => [0, screen width]
     inputMY => [0, screen height]
     inputScroll => [-1, 1]
     * => [0, 1]*/
    [Header("Cursor Input")]
    [SerializeField] public float inputMX;     //Cursor X Pos
    [SerializeField] public float inputMY;     //Cursor Y Pos
    [SerializeField] public float inputScroll; //"Scroll"
    [SerializeField] public bool inputScroll_U;
    [SerializeField] public bool inputScroll_D;
    [SerializeField] public float inputFire1;  //LMB
    [SerializeField] public bool inputFire1_U;
    [SerializeField] public bool inputFire1_D;
    [SerializeField] public float inputFire2;  //RMB
    [SerializeField] public bool inputFire2_U;
    [SerializeField] public bool inputFire2_D;
    [SerializeField] public float inputFire3;  //MMB
    [SerializeField] public bool inputFire3_U;
    [SerializeField] public bool inputFire3_D;

    /*============================================================================
     * DEFAULT UNITY METHODS
     ============================================================================*/
    private void Update()
    {
        ButtonUpdate();
        CursorUpdate();
    }

    //UPDATE THIS LATER TO ACCOMODATE CONTROLLER JOYSTICK POSITION
    /*============================================================================
     * INPUT METHOD(S)
     ============================================================================*/
    private void ButtonUpdate()
    {
        UpdateVirtualAxis(ref inputX, ref inputX_U, ref inputX_D, "Horizontal");
        UpdateVirtualAxis(ref inputY, ref inputY_U, ref inputY_D, "Vertical");
        UpdateVirtualAxis(ref inputSubmit, ref inputSubmit_U, ref inputSubmit_D, "Submit");
        UpdateVirtualAxis(ref inputCancel, ref inputCancel_U, ref inputCancel_D, "Cancel");
        UpdateVirtualAxis(ref inputAlt1, ref inputAlt1_U, ref inputAlt1_D, "Alt1");
        UpdateVirtualAxis(ref inputAlt2, ref inputAlt2_U, ref inputAlt2_D, "Alt2");
        UpdateVirtualAxis(ref inputAlt3, ref inputAlt3_U, ref inputAlt3_D, "Alt3");
        UpdateVirtualAxis(ref inputAlt4, ref inputAlt4_U, ref inputAlt4_D, "Alt4");
    }

    private void CursorUpdate()
    {
        inputMX = Input.mousePosition.x;
        inputMY = Input.mousePosition.y;
        UpdateVirtualAxis(ref inputScroll, ref inputScroll_U, ref inputScroll_D, "Mouse ScrollWheel");
        inputScroll /= Mathf.Abs(inputScroll);
        UpdateVirtualAxis(ref inputFire1, ref inputFire1_U, ref inputFire1_D, "Fire1");
        UpdateVirtualAxis(ref inputFire2, ref inputFire2_U, ref inputFire2_D, "Fire2");
        UpdateVirtualAxis(ref inputFire3, ref inputFire3_U, ref inputFire3_D, "Fire3");
    }

    private void UpdateVirtualAxis(ref float axisVal, ref bool axisUp, ref bool axisDown, string virtualAxis)
    {
        axisVal = Input.GetAxisRaw(virtualAxis);
        axisUp = Input.GetButtonUp(virtualAxis);
        axisDown = Input.GetButtonDown(virtualAxis);
    }
}
