using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace RPG_Project_Vanguard;
public class Screen_Panel : Panel
{   
    static public Size size = new Size(960, 540);
    Point location = new Point(0, 0);
    public Screen_Panel()
    {
        Size = size;
        Location = location;
    }
    virtual public void Open_Me(ref Screen_Panel current_Win)
    {   
        current_Win.Visible = false;
        Visible = true;
        Screen_Panel previous_Win = this;
        (previous_Win, current_Win) = (current_Win, previous_Win);
        Debug.WriteLine(current_Win.Name + " is now the current window");
        Debug.WriteLine(previous_Win.Name + " is now closed");
    }
}

public class Main_Screen : Screen_Panel
{
    public Main_Screen()
    {
        BackColor = Color.Black;
        Name = "Main_Screen";
    }
    public void Open_Me(ref Screen_Panel current_Win, ref List<Control> b_panels, ref List<Control> r_panels)
    {
        current_Win.Visible = false;
        Visible = true;
        Screen_Panel previous_Win = this;
        (previous_Win, current_Win) = (current_Win, previous_Win);
        Debug.WriteLine(current_Win.Name + " is now the current window");
        Debug.WriteLine(previous_Win.Name + " is now closed");
        b_panels[0].Visible = true;
        r_panels[0].Visible = true;
        b_panels[1].Visible = false;
        r_panels[1].Visible = false;
        b_panels[2].Visible = false;
        r_panels[2].Visible = false;
    }
    public void ChangeMode(string mode, ref List<Control> b_panels, ref List<Control> r_panels)
    {
        if (mode == "Menu")
        {
            Debug.WriteLine("Changing to menu mode");
            b_panels[0].Visible = true;
            r_panels[0].Visible = true;
            b_panels[1].Visible = false;
            r_panels[1].Visible = false;
            b_panels[2].Visible = false;
            r_panels[2].Visible = false;
        }

        else if (mode == "Combat")
        {
            Debug.WriteLine("Changing to combat mode");
            b_panels[0].Visible = false;
            r_panels[0].Visible = false;
            b_panels[1].Visible = true;
            r_panels[1].Visible = true;
            b_panels[2].Visible = false;
            r_panels[2].Visible = false;
        }

        else if (mode == "Dialog")
        {
            Debug.WriteLine("Changing to dialog mode");
            b_panels[0].Visible = false;
            r_panels[0].Visible = false;
            b_panels[1].Visible = false;
            r_panels[1].Visible = true;
            b_panels[2].Visible = true;
            r_panels[2].Visible = false;
        }
        
        else if (mode == "Chest")
        {
            Debug.WriteLine("Changing to chest mode");
            b_panels[0].Visible = false;
            r_panels[0].Visible = false;
            b_panels[1].Visible = false;
            r_panels[1].Visible = false;
            b_panels[2].Visible = true;
            r_panels[2].Visible = true;
        
        }
    }
}


public class Backpack_Screen_Panel : Screen_Panel
{
    public Backpack_Screen_Panel()
    {

    }

    public void Open_Me(ref Screen_Panel current_Win, bool isbackpack, ref List<Control> tList, ref List<Control> fList)
    {
        current_Win.Visible = false;
        Visible = true;
        Screen_Panel previous_Win = this;
        (previous_Win, current_Win) = (current_Win, previous_Win);
        Debug.WriteLine(current_Win.Name + " is now the current window");
        Debug.WriteLine(previous_Win.Name + " is now closed");
        if (isbackpack)
        {
            for (int i = 0; i < tList.Count; i++)
            {
                tList[i].Visible = true;
                fList[i].Visible = false;
            }
        }
        else
        {
            for (int i = 0; i < tList.Count; i++)
            {
                tList[i].Visible = false;
                fList[i].Visible = true;
            }
            //přidat itemy z random droppu
        }
    }

}












public class Menu_Button : Button
{
    static public Size size = new Size(200, 80);
    public Menu_Button()
    {
        Size = size;
        ForeColor = Color.White;
        TextAlign = ContentAlignment.MiddleCenter;
    }
}

public class Return_Button : Button
{
    static public Size size = new Size(80, 80);
    public Return_Button()
    {
        Size = size;
        ForeColor = Color.White;
        TextAlign = ContentAlignment.MiddleCenter;
    }
}

public class Stat_Button : Button
{
    public string mestat;
    public bool meincrease;
    static public Size size = new Size(40, 40);
    public Stat_Button(string stat, bool increase)
    {
        mestat = stat;
        meincrease = increase;
        Size = size;
        ForeColor = Color.White;
        Font = new Font(Font.FontFamily, 15, FontStyle.Regular);
        TextAlign = ContentAlignment.MiddleCenter;
        Padding = new Padding(0);
    }

    public void ChangeStat(string stat, bool meincrease, Player player, StatSettingsLabel label, List<Equipement_Slot> equipement_Storage) //dolaď hodnoty
    {
        bool can = false;
        int val = 0;
        if (meincrease)
        {
            val += 1;
        }
        else
        {
            val -= 1;
        }

        if (val == 1 && player.freeSkillPoints > 0)
        {
            can = true;
        }
        else if (val == -1)
        {
            if (stat == "Health" && player.health_lvl > 1)
            {
                can = true;
            }
            else if (stat == "Damage" && player.damage_lvl > 1)
            {
                can = true;
            }
            else if (stat == "Healing" && player.healing_lvl > 0)
            {
                can = true;
            }
        }

        if (can)
        {
            if (stat == "Health")
            {
                player.health_lvl += val;
                Debug.WriteLine("Health lvl is now " + player.health_lvl);
            }   
            else if (stat == "Damage")
            {
                player.damage_lvl += val;
                Debug.WriteLine("Damage lvl is now " + player.damage_lvl);
            }
            else if (stat == "Healing")
            {
                player.healing_lvl += val;
                Debug.WriteLine("Healing lvl is now " + player.healing_lvl);
            }
            player.freeSkillPoints -= val;
        }
        
        player.UpdateStats(equipement_Storage);
        label.UpdateText(player);
    }
}

class Dialog_Button : Button
{
    public string text = "";
    public Dialog_Button()
    {
        Text = text;
        Size = new Size(960, 120);
        BackColor = Color.FromArgb(0, 0, 0);
        ForeColor = Color.White;
    }
}













public class CustomLabel : Label
{
    public CustomLabel()
    {
        ForeColor = Color.White;
    }
}

public class EntityStatLbael : CustomLabel
{
    public string? name;
    public int lvl;
    public double current_hp;
    public int max_hp;
    public EntityStatLbael(Entity entity)
    {
        Font = new Font(Font.FontFamily, 15, FontStyle.Regular);
        if (entity == null)
        {
            return;
        }
        else
        {
            UpdateText(entity);
        }
    }
    public void UpdateText(Entity entity)
    {
        name = entity.name;
        lvl = entity.lvl;
        current_hp = entity.current_hp;
        max_hp = entity.max_hp;
        Text = $"Name: {name} Lvl: {lvl}\nHP: {current_hp}/{max_hp}";
    }
}


public class PlayerStatScreenLabel : CustomLabel
{
    public string? name;
    public int lvl;
    public int exp;
    public int expTreshold;
    public int max_hp;
    public int damage_per_hit;
    public int healing_lvl;
    public PlayerStatScreenLabel(Player player)
    {
        Font = new Font(Font.FontFamily, 15, FontStyle.Regular);
        UpdateText(player);
    }
    public void UpdateText(Player player)
    {
        name = player.name;
        lvl = player.lvl;
        exp = player.exp;
        expTreshold = player.expTreshold;
        max_hp = player.max_hp;
        damage_per_hit = player.damage_per_hit;
        healing_lvl = player.healing_lvl;
        Text = $"Name: {name}\nLvl: {lvl}\nExp: {exp}/{expTreshold}\nMax HP: {max_hp}\nDamage: {damage_per_hit}\nHealing: {healing_lvl}";
    }
}


public class StatSettingsLabel : CustomLabel
{
    public int freePoints;
    public int health;
    public int damage;
    public int healing;
    public StatSettingsLabel(Player player)
    {
        Font = new Font(Font.FontFamily, 15, FontStyle.Regular);
        UpdateText(player);
    }
    public void UpdateText(Player player)
    {
        freePoints = player.freeSkillPoints;
        health = player.health_lvl;
        damage = player.damage_lvl;
        healing = player.healing_lvl;
        Text = $"Free Points: {freePoints}\nHealth lvl: {health}\nDamage lvl: {damage}\nHealing lvl: {healing}";
    }
}