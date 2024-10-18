using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace RPG_Project_Vanguard;
public class Entity
{
    public string? name;
    public int health_lvl;
    public int max_hp;
    public int current_hp;
    public int healing_lvl;
    public int healing_amount;
    public int damage_lvl;
    public int damage_per_hit;
    public int lvl;
    public int exp;

    public void UpdateStats()
    {
        max_hp = health_lvl * 10;
        current_hp = max_hp;
        healing_amount = healing_lvl*healing_lvl;
        damage_per_hit = damage_lvl * 3;
    }
    public Entity()
    {
        UpdateStats();
    }
}



public class Player : Entity
{
    public int expTreshold;
    public int freeSkillPoints;
    public Player()
    {
        lvl = 1;
        freeSkillPoints = 0;
        health_lvl = 1;
        damage_lvl = 1;
        healing_lvl = 0;
        expTreshold = 100 + 50 * lvl;


        UpdateStats();
    }

    public void UpdateStats(List<Equipement_Slot> slots)
    {
        int health_modifier = 0;
        int damage_modifier = 0;
        foreach (var slot in slots)
        {
            if (slot.stored_item != null)
            {
                health_modifier += slot.stored_item.health_modifier;
                damage_modifier += slot.stored_item.damage_modifier;
            }
        }

        max_hp = health_lvl * 10 + health_modifier; // + armorstats
        current_hp = max_hp;
        healing_amount = healing_lvl*healing_lvl;
        damage_per_hit = damage_lvl * 3 + damage_modifier; // + weaponstats
        expTreshold = 100 + 50 * lvl;

    }

    public void GainXP(int gainedexp)
    {
        exp += gainedexp; 
        if (exp >= expTreshold)
        {   
            LvlUp();
        };
    }
    public void LvlUp()
    {
        lvl += 1;
        freeSkillPoints += 1;
        exp -= expTreshold;
        if (exp >= expTreshold)
        {
            LvlUp();
        }
    }
}

// "Life is hard, so let's make it harder by working smarter." - most sane brogrammer Burymuru 2024

/*
wip: vnitřnosti funkcí a práce s promněnnými
hodně enemies
*/