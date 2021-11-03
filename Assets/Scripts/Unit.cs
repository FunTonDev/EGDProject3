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
        actionName = "Basic Attack";
        actionDesc = "A standard offensive move, deals 5 points of damage. One of the hallmarks of any RPG character.";
        type = 0;
        damage = 5;
        priority = 2;
        target = 0;
    }
}

public class AOE : Action
{
    public AOE()
    {
        actionName = "AOE Attack";
        actionDesc = "An attack that covers a wide area of effect. Deals less damage, but hits more than usual.";
        type = 0;
        damage = 3;
        priority = 1;
        target = 1;
    }
}

public class BasicHeal : Action
{
    public BasicHeal()
    {
        actionName = "Basic Heal";
        actionDesc = "A simple healing spell, solves all your problems. The hallmark of any true white mage.";
        type = 1;
        damage = 5;
        priority = 1;
        target = 0;
    }
}

public class StatBoost : Action
{
    public StatBoost()
    {
        actionName = "Basic Heal";
        actionDesc = "A unique support move, used to amplify an ally's abilities. Helps in a pinch.";
        type = 1;
        damage = 0;
        priority = 1;
        target = 0;
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

    public void levelUp()
    {

    }

    public void loadSprites()
    {
        sprites[0] = (Sprite)AssetDatabase.LoadAssetAtPath(spriteFilePath, typeof(Sprite));
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
        unitName = "Mama";
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
        unitName = "Hound";
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
        unitName = "Slime";
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
        unitName = "Skeleton";
        spriteFilePath = "Art/Placeholder/Skeletoj";
        loadSprites();
        setStats(lv);
        abilities.Add(new Basic());
        abilities.Add(new BasicHeal());
    }
}
