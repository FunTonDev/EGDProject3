using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Action
{
    public string actionName;
    public string actionDesc;
    public int actionType;  //0 == attack, 1 == support/buff

    public int damageType;  //0 - Type A, 1 - Type B, 2 - Type C  (A > B, B > C, C > A)
    public float damage;

    public int target;  //0 == single, 1 == AOE/All
}


public class Unit
{
    public string unitName;

    public float currentHP;
    public float maxHP;
    public float currentStamina;
    public float maxStamina;

    public int atk;
    public int def;
    public int spd;
    public int level;

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

    public string spriteFilePath;
    public Sprite[] sprites;


    public List<Action> unitActions;
    // Start is called before the first frame update
    public Unit(int lv = 1)
    {
        sprites = new Sprite[1];
    }

    public bool takeDamage(int dam)
    {
        currentHP -= dam;
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
    }
}
