using System.Diagnostics;

namespace RPG_Project_Vanguard;
public class Enemy : Entity
{
    public Enemy()
    {
    }//
    public string Play(ref Player player)
    {
        if (current_hp <= healing_amount)
        {
            Heal();
            return "Healed";
        }
        else
        {
            Attack(ref player);
            if (player.current_hp <= 0)
            {
                Debug.WriteLine($"{name} has defeated {player.name}");
                return "Defeat";
            }
            else
            {
                return "Attacked";
            }
        }

    }
    public void Heal()
    {
        int healed;
        if ((current_hp + healing_amount) > max_hp)
        {
            healed = max_hp - current_hp;
            current_hp = max_hp;
        }
        else
        {
            healed = healing_amount;
            current_hp += healing_amount;
        }

        Debug.WriteLine($"{name} healed {healed} points of health");
    }

    public void Attack(ref Player player)
    {
        player.current_hp -= damage_per_hit;
        Debug.WriteLine($"{name} dealt {damage_per_hit} points of damage to {player.name}");
    }
}

public class Slime : Enemy
{
    public Slime()
    {
        name = "Slime";
        health_lvl = 1;
        healing_lvl = 1;
        damage_lvl = 1;
        lvl = 1;
        exp = 50;

        UpdateStats();
    }
}

public class Golem : Enemy
{
    public Golem()
    {
        name = "Golem";
        health_lvl = 6;
        healing_lvl = 0;
        damage_lvl = 2;
        lvl = 6;
        exp = 500;

        UpdateStats();
    }
}

public class Wyvern : Enemy
{
    public Wyvern()
    {
        name = "Wyvern";
        health_lvl = 5;
        healing_lvl = 6;
        damage_lvl = 7;
        lvl = 16;
        exp = 2000;

        UpdateStats();
    }
}

public class Dragon : Enemy
{
    public Dragon()
    {
        name = "Dragon";
        health_lvl = 20;
        healing_lvl = 5;
        damage_lvl = 10;
        lvl = 35;
        exp = 100000;

        UpdateStats();
    }
}