using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Different states of battle (turns)
public enum battleState { START, PLAYER, ATTACK, ENEMY, WIN, LOSE, FLEE, HUH }

public class BattleManager : MonoBehaviour
{
    //Use to determine state of the battle (turns, win/loss, etc.)
    public battleState state;

    public InputManager inputMan;

    //List of the positions of menus
    [System.Serializable]
    public struct MenuPositions
    {
        public List<Transform> positions;
    }

    public int cursor_position;     //Current position (index) the cursor is at
    public int active_menu;         //Current menu being moved through

    //A list of positions the cursor can go through
    [SerializeField]
    public List<MenuPositions> cursor_positions;

    private List<GameObject> menus;                 //The list of menu objects

    public Text DisplayText;

    //Units that are currently in battle
    public List<Unit> PartyMembers;
    public List<Unit> EnemyMembers;

    //Int to track how many abilities away from the bottom before the menu can start scrolling
    private int ability_offset;
    //Int to track how many actions away from the bottom before the menu can start scrolling
    private int action_offset;

    //Current ability being highlighted
    private int highlighted_ability;
    //Current action in base menu (index) being highlighed by cursor
    private int highlighted_action;
    //Bool to check whether the menu is accepting input
    private bool menu_input;

    //Bool to check whether the player has the ability menu open
    private bool ability_select_menu;
    //Bool to check whether the player is selecting an enemy to attack
    private bool enemy_select_menu;
    //Bool to check whether the player is selecting an ally for an ability
    private bool unit_select_menu;

    public GameObject background;

    //GameObjects to use as basis for battle characters
    public List<GameObject> partyPrefabs;

    public List<GameObject> enemyPrefabs;

    public List<Action> actions;

    //Int to track the number of units actually in the party
    int activeUnits = 0;

    //Int to track the number of deaths in the party
    int partyDeaths = 0;

    //Int to track the number of enemies encountered in the battle
    int activeEnemies = 0;

    //Number of enemies that have died
    int enemyDeaths = 0;

    //The current unit in the party that is choosing an action
    private int currentUnit = 0;

    //Current ally being selected for using an ability on/swapping
    private int currentAlly = 0;

    //The number of moves that should be done by the party
    private int moves = 0;

    //The enemy currently being highlighted
    private int currentEnemy = 0;

    public void playerTurn()
    {
        DisplayText.text = PartyMembers[currentUnit] + "'s turn";
    }

    //Function to scroll through main menu in battle to choose what to do
    public void BaseMenuRoutine()
    {

    }
    //Function to scroll through possible abilites/actions
    public void ActionMenuRoutine()
    {

    }
    //Function to try and recruit an enemy as a party member
    public void RecruitMenuRoutine()
    {

    }

    public void makeMenuVisible(int num)
    {
        if (num == 0)
        {
            menus[2 + num].SetActive(true);
            menus[2 + num + 1].SetActive(false);
        }
        else if (num == 1)
        {
            menus[2 + num].SetActive(true);
            menus[2 + num - 1].SetActive(false);
        }
    }

    IEnumerator setupBattle()
    {
        currentUnit = 0;
        for (int i = 0; i < 3; i++) PartyMembers.Add(null);
        for (int i = 0; i < 3; i++) EnemyMembers.Add(null);
        //Load in from json file or other source. Until available, load in prefabs
        PartyMembers[0] = new Pixal();
        PartyMembers[1] = new Mama();

        for (int i = 0; i < 3; i++)
        {
            if (PartyMembers[i] != null)
            {
                if (PartyMembers[i].sprites[0] != null)
                {
                    partyPrefabs[i].GetComponent<SpriteRenderer>().sprite = PartyMembers[i].sprites[0];
                }
                else
                {
                    partyPrefabs[i].GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f);
                }
                partyPrefabs[i].transform.GetChild(0).GetComponent<SpriteRenderer>().transform.localScale =
                    new Vector3(0.5f * PartyMembers[i].currentHP / PartyMembers[i].maxHP, 0.2f, 0.0f);
                partyPrefabs[i].transform.GetChild(1).GetComponent<SpriteRenderer>().transform.localScale =
                    new Vector3(0.5f * PartyMembers[i].currentStamina / PartyMembers[i].maxStamina, 0.2f, 0.0f);
                activeUnits += 1;
            }
            else
            {
                partyPrefabs[i].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            }
        }

        EnemyMembers[0] = new Slime();
        EnemyMembers[1] = new Skeleton();
        EnemyMembers[2] = new Hound();
        activeEnemies = 3;

        actions = new List<Action>();

        if (activeEnemies == 1)
        {
            DisplayText.text = "A " + EnemyMembers[0].unitName + " has appeared.";
        }
        else if (activeEnemies >= 2)
        {
            DisplayText.text = "A group of enemies appeared";
        }
        yield return new WaitForSeconds(1.0f);
        currentUnit = 0;

        state = battleState.PLAYER;
        playerTurn();

        yield return new WaitForSeconds(1.0f);
    }

    //Perform the selected actions, after they have been selected
    public IEnumerator PerformActions()
    {
        yield return new WaitForSeconds(0.0f);
    }

    //Fade into the battle scene (from black to screen)
    IEnumerator fadeIn()
    {
        Color ori = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        transform.GetChild(1).Find("Fader").GetComponent<Image>().color = ori;
        transform.GetChild(1).Find("Fader").GetComponent<Image>().CrossFadeAlpha(0, 2f, false);
        yield return new WaitForSeconds(0.5f);

    }

    //Fade out of the battle scene (from scene to black)
    IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(1f);
        Color ori = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        //transform.GetChild(1).Find("Fader").GetComponent<Image>().color = ori;
        transform.GetChild(1).Find("Fader").GetComponent<Image>().CrossFadeAlpha(1, 2f, false);
    }


    // Start is called before the first frame update
    void Start()
    {
        menus = new List<GameObject>();
        for (int i = 2; i < transform.GetChild(1).childCount-1; i++)
        {
            menus.Add(transform.GetChild(1).GetChild(i).gameObject);
        }

        //StartCoroutine(fadeIn());
        menu_input = false;

        PartyMembers = new List<Unit>();
        EnemyMembers = new List<Unit>();


        state = battleState.START;
        StartCoroutine(setupBattle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
