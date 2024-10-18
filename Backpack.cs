using System.Data;

namespace RPG_Project_Vanguard;

public class Item_Slot : Button
{
    public Item? stored_item;

    public Item_Slot()
    {   
        stored_item = null;
    }
    virtual public void UpdateText()
    {
        if (stored_item != null)
        {
            Text = stored_item.name;
        }
        else
        {
            Text = "";
        }
    }
}

public class Equipement_Slot : Item_Slot
{
    public string equipement_type;
    public Equipement_Slot(string type)
    {
        equipement_type = type;
    }
    public override void UpdateText()
    {
        if (stored_item != null)
        {
            Text = stored_item.name;
        }
        else
        {
            Text = equipement_type;
        }
    }
}

public class Backpack_Slot : Item_Slot
{
    public Backpack_Slot()
    {
    }
}
