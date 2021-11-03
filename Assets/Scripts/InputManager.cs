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
    [HideInInspector] public bool inputX_U;
    [HideInInspector] public bool inputX_D;
    [SerializeField] public float inputY;      //W-S
    [HideInInspector] public bool inputY_U;
    [HideInInspector] public bool inputY_D;
    [SerializeField] public float inputSubmit; //Enter
    [HideInInspector] public bool inputSubmit_U;
    [HideInInspector] public bool inputSubmit_D;
    [SerializeField] public float inputCancel; //Escape
    [HideInInspector] public bool inputCancel_U;
    [HideInInspector] public bool inputCancel_D;
    [SerializeField] public float inputAct1;   //E
    [HideInInspector] public bool inputAct1_U;
    [HideInInspector] public bool inputAct1_D;
    [SerializeField] public float inputAct2;   //R
    [HideInInspector] public bool inputAct2_U;
    [HideInInspector] public bool inputAct2_D;
    [SerializeField] public float inputAct3;   //Q
    [HideInInspector] public bool inputAct3_U;
    [HideInInspector] public bool inputAct3_D;
    [SerializeField] public float inputAct4;   //Space
    [HideInInspector] public bool inputAct4_U;
    [HideInInspector] public bool inputAct4_D;
    [SerializeField] public float inputAct5;   //Tab
    [HideInInspector] public bool inputAct5_U;
    [HideInInspector] public bool inputAct5_D;
    [SerializeField] public float inputAct6;   //LShift
    [HideInInspector] public bool inputAct6_U;
    [HideInInspector] public bool inputAct6_D;
    [SerializeField] public float inputAct7;   //LCtrl
    [HideInInspector] public bool inputAct7_U;
    [HideInInspector] public bool inputAct7_D;

    /*Input values:
     inputMX => [0, screen width]
     inputMY => [0, screen height]
     inputScroll => [-1, 1]
     * => [0, 1]*/
    [Header("Cursor Input")]
    [SerializeField] public float inputMX;     //Cursor X/Y Pos
    [SerializeField] public float inputMY;
    [SerializeField] public float inputScroll; //"Scroll"
    [HideInInspector] public bool inputScroll_U;
    [HideInInspector] public bool inputScroll_D;
    [SerializeField] public float inputFire1;  //LMB
    [HideInInspector] public bool inputFire1_U;
    [HideInInspector] public bool inputFire1_D;
    [SerializeField] public float inputFire2;  //RMB
    [HideInInspector] public bool inputFire2_U;
    [HideInInspector] public bool inputFire2_D;
    [SerializeField] public float inputFire3;  //MMB
    [HideInInspector] public bool inputFire3_U;
    [HideInInspector] public bool inputFire3_D;

    /*============================================================================
     * DEFAULT UNITY METHODS
     ============================================================================*/
    private void Update()
    {
        CursorUpdate();
        ButtonUpdate();
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
        UpdateVirtualAxis(ref inputAct1, ref inputAct1_U, ref inputAct1_D, "Action1");
        UpdateVirtualAxis(ref inputAct2, ref inputAct2_U, ref inputAct2_D, "Action2");
        UpdateVirtualAxis(ref inputAct3, ref inputAct3_U, ref inputAct3_D, "Action3");
        UpdateVirtualAxis(ref inputAct4, ref inputAct4_U, ref inputAct4_D, "Action4");
        UpdateVirtualAxis(ref inputAct5, ref inputAct5_U, ref inputAct5_D, "Action5");
        UpdateVirtualAxis(ref inputAct6, ref inputAct6_U, ref inputAct6_D, "Action6");
        UpdateVirtualAxis(ref inputAct7, ref inputAct7_U, ref inputAct7_D, "Action7");
        UpdateVirtualAxis(ref inputFire1, ref inputFire1_U, ref inputFire1_D, "Fire1");
        UpdateVirtualAxis(ref inputFire2, ref inputFire2_U, ref inputFire2_D, "Fire2");
        UpdateVirtualAxis(ref inputFire3, ref inputFire3_U, ref inputFire3_D, "Fire3");
    }

    private void CursorUpdate()
    {
        inputMX = Input.mousePosition.x;
        inputMY = Input.mousePosition.y;
        UpdateVirtualAxis(ref inputScroll, ref inputScroll_U, ref inputScroll_D, "Mouse ScrollWheel");
        inputScroll /= Mathf.Abs(inputScroll);
    }

    private void UpdateVirtualAxis(ref float axisVal, ref bool axisUp, ref bool axisDown, string virtualAxis)
    {
        axisVal = Input.GetAxisRaw(virtualAxis);
        axisUp = Input.GetButtonUp(virtualAxis);
        axisDown = Input.GetButtonDown(virtualAxis);
    }
}
