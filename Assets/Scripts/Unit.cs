using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Action
{
    public Action()
    {

    }
    public string actionName;
    public string actionDesc;
    public int type;  //0 == attack, 1 == support/buff

    public int damageType;  //0 - Type A, 1 - Type B, 2 - Type C  (A > B, B > C, C > A)
    public int damage;

    public int statusEffect;

    public int cost;

    public int ATKBoost;
    public int DEFBoost;
    public int SPDBoost;

    public int priority = 1;

    public int target;  //0 == single, 1 == AOE/All
}

public class Basic : Action
{
    public Basic()
    {
        actionName = "Slashing Edge";
        actionDesc = "A standard offensive move, deals 5 points of damage. One of the hallmarks of any RPG character.";
        type = 0;
        damage = 5;
        priority = 2;
        target = 0;
        cost = 1;
    }
}

public class BasicStatus : Action
{
    public BasicStatus()
    {
        actionName = "Corrupting Edge";
        actionDesc = "A slightly more dangerous attack, has the chance of inflicting damage over time.";
        type = 0;
        damage = 5;
        priority = 2;
        target = 0;
        cost = 1;
        statusEffect = 1;
    }
}


public class AOE : Action
{
    public AOE()
    {
        actionName = "Grand Slam";
        actionDesc = "An attack that covers a wide area of effect. Deals less damage, but hits more than usual.";
        type = 0;
        damage = 3;
        priority = 1;
        target = 1;
        cost = 3;
    }
}

public class BasicHeal : Action
{
    public BasicHeal()
    {
        actionName = "HP Support";
        actionDesc = "A simple healing spell, solves all your problems. The hallmark of any true white mage.";
        type = 1;
        damage = 5;
        priority = 1;
        target = 0;
        cost = 2;
    }
}

public class StatBoost : Action
{
    public StatBoost()
    {
        actionName = "Stat Support";
        actionDesc = "A unique support move, used to amplify an ally's abilities. Helps in a pinch.";
        type = 1;
        damage = 0;
        priority = 1;
        target = 0;
        cost = 4;
    }
}



public class Unit
{
    public string unitName;
    public bool enemy;

    public float currentHP;
    public float maxHP;
    public float currentStamina;
    public float maxStamina;

    public int atk;
    public int def;
    public int spd;
    public int lck;
    public int level;

    public bool defending = false;

    public int hpLevelBoost;
    public int stmLevelBoost;
    public int atkLevelBoost;
    public int defLevelBoost;
    public int spdLevelBoost;

    public int hpItemBoost;
    public int stmItemBoost;
    public int atkItemBoost;
    public int defItemBoost;
    public int spdItemBoost;

    public List<int> statuses;  //0 == Dmg (Poison), 1 == Stun (Paralysis), 2 == Debuff(Burn), 3 == Buff
    public bool[] weaknesses;           //an array of integer codes for the weaknesses that a unit may have
    public bool[] resistances;          //an array of integer codes for the resistances that a unit may have

    public List<Action> abilities;

    public string spriteFilePath;
    public Sprite[] sprites;

    public int capital;


    public List<Action> unitActions;


    public Unit(int lv = 1)
    {
        sprites = new Sprite[1];
        abilities = new List<Action>();
        abilities.Add(new Basic());
        statuses = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            statuses.Add(-1);
        }
        weaknesses = new bool[5];
        resistances = new bool[5];
    }

    public bool takeDamage(int dam)
    {
        if (!defending)
            currentHP -= dam;
        else
            currentHP -= dam / 2;

        if (currentHP <= 0)
        {
            currentHP = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void setStats(int lv = 1)
    {
        switch (lv)
        {
            case 1:
                maxHP = 20;
                currentHP = 10;
                maxStamina = 20;
                currentStamina = 5;
                atk = 1;
                def = 1;
                spd = 1;
                lck = 1;
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }
    }

    public int getAtk()
    {
        int at = atk;
        if (statuses[2] != -1)
        {
            at = at / 2;
        }
        if (statuses[3] != -1)
        {
            at = at * 2;
        }
        return at;
    }

    public int getDef()
    {
        int de = def;
        if (statuses[2] != -1)
        {
            de = de / 2;
        }
        if (statuses[3] != -1)
        {
            de = de * 2;
        }
        return de;
    }

    public int getSpd()
    {
        int sp = spd;
        if (statuses[2] != -1)
        {
            sp = sp / 2;
        }
        if (statuses[3] != -1)
        {
            sp = sp * 2;
        }
        return sp;
    }

    public void levelUp()
    {

    }

    public void loadSprites()
    {
        sprites[0] = (Sprite)AssetDatabase.LoadAssetAtPath(spriteFilePath, typeof(Sprite));
    }

    public void statusTurn()
    {
        for (int i = 0; i < statuses.Count; i++)
        {
            if (statuses[i] == 0)
            {
                statuses[i] = -1;
            }
            if (statuses[i] > -1)
            {
                statuses[i] -= 1;
            }
        }
    }
}

public class Pixal : Unit
{
    public Pixal(int lv = 1)
    {
        unitName = "Pixal";
        spriteFilePath = "Art/Placeholder/Cube.png";
        loadSprites();
        setStats(lv);
        abilities.Add(new BasicHeal());
    }
}

public class Mama : Unit
{
    public Mama(int lv = 1)
    {
        unitName = "Knight";
        spriteFilePath = "Art/Placeholder/mama.jfif";
        loadSprites();
        setStats(lv);
        abilities.Add(new AOE());
    }
}

public class Hound : Unit
{
    public Hound(int lv = 1)
    {
        unitName = "Bear";
        spriteFilePath = "Art/Placeholder/Dog.jfif";
        loadSprites();
        setStats(lv);
        abilities.Add(new Basic());
        abilities.Add(new StatBoost());
    }
}

public class Slime : Unit
{
    public Slime(int lv = 1)
    {
        unitName = "Soldier";
        spriteFilePath = "Art/Placeholder/BasicSlime";
        loadSprites();
        setStats(lv);
        abilities.Add(new Basic());
        abilities.Add(new AOE());
    }
}

public class BossSlime : Unit
{
    public BossSlime(int lv = 1)
    {
        unitName = "Suspicious Slime";
        spriteFilePath = "Art/Placeholder/KingSlime";
        loadSprites();
        setStats(lv);
        abilities.Add(new Basic());
    }
}

public class Skeleton : Unit
{
    public Skeleton(int lv = 1)
    {
        unitName = "Wizard";
        spriteFilePath = "Art/Placeholder/Skeletoj";
        loadSprites();
        setStats(lv);
        abilities.Add(new Basic());
        abilities.Add(new BasicHeal());
    }
}
