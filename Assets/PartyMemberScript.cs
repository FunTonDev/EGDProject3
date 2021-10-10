using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Action
{
    public string actionName;
    public string actionDesc;
    public int actionType;

    public int damageType;  //0 - Type A, 1 - Type B, 2 - Type C  (A > B, B > C, C > A)
    public float damage;
}


public class PartyMemberScript : MonoBehaviour
{
    public Image HPBar;
    public Image StaminaBar;

    public string unitName;

    public float currentHP;
    public float maxHP;
    public float currentStamina;
    public float maxStamina;

    public List<Action> unitActions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
