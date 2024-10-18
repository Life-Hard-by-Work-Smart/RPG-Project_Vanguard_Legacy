namespace RPG_Project_Vanguard;

public class Item
{ 
    public string name;
    public string equipement_type = "";
    public int damage_modifier = 0;
    public int health_modifier = 0;
    public double dropChance = 0.0;

    public Item()
    {
        name = "Item";
    }
}

public class Weapon : Item
{
    public Weapon()
    {
        equipement_type = "Weapon";
    }
}


public class Armor : Item
{
    
}

public class Head_Gear : Armor
{
    public Head_Gear()
    {
        equipement_type = "Head";
    }
}
public class Chest_Gear : Armor
{
    public Chest_Gear()
    {
        equipement_type = "Chest";
    }
}
public class Leg_Gear : Armor
{   
    public Leg_Gear()
    {
        equipement_type = "Legs";
    }
}

public class All_Items
{
    public List<Item> AllSlimeDrops = new List<Item>();

    public List<Item> AllGolemDrops = new List<Item>();

    public List<Item> AllWivernDrops = new List<Item>();

    public List<Item> AllDragonDrops = new List<Item>();

    public List<Item> Drops = new List<Item>();

    public List<Item> GenerateDrops(Enemy enemy)
    {
        List<Item> AllDrops = new List<Item>();

        if (enemy.name == "Slime")
        {
            AllDrops = AllSlimeDrops;
        }
        else if (enemy.name == "Golem")
        {
            AllDrops = AllGolemDrops;
        }
        else if (enemy.name == "Wyvern")
        {
            AllDrops = AllWivernDrops;
        }
        else if (enemy.name == "Dragon")
        {
            AllDrops = AllDragonDrops;
        }

        Drops.Clear();

        foreach (Item item in AllDrops)
        {
            double random = new Random().NextDouble();
            if (random < item.dropChance)
            {
                if (Drops.Count < 6)
                {
                    Drops.Add(item);
                }
            }
        }
        return Drops;
    }





    public All_Items()
    {
        //Slime Drops
        Weapon slime_weapon = new Weapon()
        {
            name = "Half-Liquid Sword",
            damage_modifier = 1,
            health_modifier = 0,
            dropChance = 0.3
        };
        AllSlimeDrops.Add(slime_weapon);
        Head_Gear slime_head = new Head_Gear()
        {
            name = "Bouncy Helmet",
            damage_modifier = 0,
            health_modifier = 3,
            dropChance = 0.3
        };
        AllSlimeDrops.Add(slime_head);
        Chest_Gear slime_chest = new Chest_Gear()
        {
            name = "Bouncy Chestplate",
            damage_modifier = 0,
            health_modifier = 6,
            dropChance = 0.25
        };
        AllSlimeDrops.Add(slime_chest);
        Leg_Gear slime_legs = new Leg_Gear()
        {
            name = "Bouncy Leggings",
            damage_modifier = 0,
            health_modifier = 3,
            dropChance = 0.3
        };
        AllSlimeDrops.Add(slime_legs);



        Weapon slime_weapon2 = new Weapon()
        {
            name = "Slime Slinging Slasher",
            damage_modifier = 4,
            health_modifier = 0,
            dropChance = 0.04
        };
        AllSlimeDrops.Add(slime_weapon2);
        Head_Gear slime_head2 = new Head_Gear()
        {
            name = "Gooey Helmet",
            damage_modifier = 0,
            health_modifier = 5,
            dropChance = 0.08
        };
        AllSlimeDrops.Add(slime_head2);
        Chest_Gear slime_chest2 = new Chest_Gear()
        {
            name = "Gooey Chestplate",
            damage_modifier = 0,
            health_modifier = 10,
            dropChance = 0.04
        };
        AllSlimeDrops.Add(slime_chest2);
        Leg_Gear slime_legs2 = new Leg_Gear()
        {
            name = "Gooey Leggings",
            damage_modifier = 0,
            health_modifier = 5,
            dropChance = 0.08
        };
        AllSlimeDrops.Add(slime_legs2);
        
        //Golem Drops
        Weapon golem_weapon = new Weapon()
        {
            name = "Golem's Arm",
            damage_modifier = 5,
            health_modifier = 2,
            dropChance = 0.5
        };
        AllGolemDrops.Add(golem_weapon);
        Head_Gear golem_head = new Head_Gear()
        {
            name = "Rock with eyelids",
            damage_modifier = -1,
            health_modifier = 8,
            dropChance = 0.5
        };
        AllGolemDrops.Add(golem_head);
        Chest_Gear golem_chest = new Chest_Gear()
        {
            name = "Rocklike Chestplate",
            damage_modifier = -2,
            health_modifier = 16,
            dropChance = 0.5
        };
        AllGolemDrops.Add(golem_chest);
        Leg_Gear golem_legs = new Leg_Gear()
        {
            name = "Rocks glued to legs",
            damage_modifier = -1,
            health_modifier = 8,
            dropChance = 0.5
        };
        AllGolemDrops.Add(golem_legs);



        Weapon golem_weapon2 = new Weapon()
        {
            name = "Floating Boulder",
            damage_modifier = 8,
            health_modifier = 5,
            dropChance = 0.02
        };
        AllGolemDrops.Add(golem_weapon2);
        Head_Gear golem_head2 = new Head_Gear()
        {
            name = "Living Helmet",
            damage_modifier = 0,
            health_modifier = 8,
            dropChance = 0.05
        };
        AllGolemDrops.Add(golem_head2);
        Chest_Gear golem_chest2 = new Chest_Gear()
        {
            name = "Living Chestplate",
            damage_modifier = 0,
            health_modifier = 16,
            dropChance = 0.05
        };
        AllGolemDrops.Add(golem_chest2);
        Leg_Gear golem_legs2 = new Leg_Gear()
        {
            name = "Living Leggings",
            damage_modifier = 0,
            health_modifier = 8,
            dropChance = 0.05
        };
        AllGolemDrops.Add(golem_legs2);
        
        //Wivern Drops
        Weapon wyvern_weapon = new Weapon()
        {
            name = "Wyvern Incisor Dagger",
            damage_modifier = 16,
            health_modifier = -5,
            dropChance = 0.3
        };
        AllWivernDrops.Add(wyvern_weapon);
        Head_Gear wyvern_head = new Head_Gear()
        {
            name = "Scale Helmet",
            damage_modifier = 0,
            health_modifier = 15,
            dropChance = 0.3
        };
        AllWivernDrops.Add(wyvern_head);
        Chest_Gear wyvern_chest = new Chest_Gear()
        {
            name = "Scale Chestplate",
            damage_modifier = 0,
            health_modifier = 30,
            dropChance = 0.3
        };
        AllWivernDrops.Add(wyvern_chest);
        Leg_Gear wyvern_legs = new Leg_Gear()
        {
            name = "Scale Leggings",
            damage_modifier = 0,
            health_modifier = 15,
            dropChance = 0.3
        };
        AllWivernDrops.Add(wyvern_legs);

        Weapon wyvern_weapon2 = new Weapon()
        {
            name = "Sword of The Fallen Hero",
            damage_modifier = 30,
            health_modifier = 0,
            dropChance = 0.02
        };
        AllWivernDrops.Add(wyvern_weapon2);
        Head_Gear wyvern_head2 = new Head_Gear()
        {
            name = "Bacta Helmet",
            damage_modifier = 0,
            health_modifier = 20,
            dropChance = 0.06
        };
        AllWivernDrops.Add(wyvern_head2);
        Chest_Gear wyvern_chest2 = new Chest_Gear()
        {
            name = "Bacta Chestplate",
            damage_modifier = 0,
            health_modifier = 40,
            dropChance = 0.06
        };
        AllWivernDrops.Add(wyvern_chest2);
        Leg_Gear wyvern_legs2 = new Leg_Gear()
        {
            name = "Bacta Leggings",
            damage_modifier = 0,
            health_modifier = 20,
            dropChance = 0.06
        };
        AllWivernDrops.Add(wyvern_legs2);

        //Dragon Drops
        Weapon dragon_weapon = new Weapon()
        {
            name = "Swift Dragon Webbing Scythe",
            damage_modifier = 999,
            health_modifier = 0,
            dropChance = 1
        };
        AllDragonDrops.Add(dragon_weapon);
        Head_Gear dragon_head = new Head_Gear()
        {
            name = "Melted Dragon Slayer Helmet",
            damage_modifier = 999,
            health_modifier = -4,
            dropChance = 1
        };
        AllDragonDrops.Add(dragon_head);
        Chest_Gear dragon_chest = new Chest_Gear()
        {
            name = "Triumph of the Dragon's Lair Chestplate",
            damage_modifier = -100,
            health_modifier = 20,
            dropChance = 1
        };
        AllDragonDrops.Add(dragon_chest);
        Leg_Gear dragon_legs = new Leg_Gear()
        {
            name = "Ice Walker Leggings",
            damage_modifier = 0,
            health_modifier = 300,
            dropChance = 1
        };
        AllDragonDrops.Add(dragon_legs);
    }
}