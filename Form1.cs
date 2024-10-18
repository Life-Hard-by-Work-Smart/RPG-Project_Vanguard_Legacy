using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Net.Http.Headers;
using System.Runtime;

namespace RPG_Project_Vanguard;

public partial class Project_Vanguard : Form
{
    // Fields   ////////////////////////////////////////////////

    // Entities
    public Player player;
    public Enemy? current_Enemy;

    // screen panels
    private Screen_Panel panel_Name_Selcect_Screen;
    private Main_Screen panel_Main_Screen;
    private Backpack_Screen_Panel panel_Backpack_Screen;
    private Screen_Panel panel_Skills_Screen;

    // Name Select screen elements
    private TextBox textBox_Name_Input;
    private Menu_Button button_Name_Confirm;

    // main screen elements
    private Panel panel_GUIWindowL;
    private EntityStatLbael label_My_Current_Stats;
    private Panel panel_GUIWindowR1;
    private Panel panel_GUIWindowR2;
    private Panel panel_GUIWindowR3;

    private EntityStatLbael label_Enemy_Current_Stats;
    private Panel panel_Combat_Menu;
    private Menu_Button button_Attack;
    private Menu_Button button_Heal;

    private Menu_Button button_Select_Location1;
    private Menu_Button button_Select_Location2;
    private Menu_Button button_Select_Location3;
    private Menu_Button button_Select_Location4; 
    private Panel panel_Menu;
    private Menu_Button button_Skills_Open;
    private Menu_Button button_Backpack_Open;

    private Menu_Button button_Chest_Open;
    private Panel panel_Dialog;
    private Dialog_Button button_Dialog_Window;


    public List<Control> bottom_panels = new List<Control>();
    public List<Control> right_panels = new List<Control>();

    // backpack screen elements
    private Panel panel_Equipement_Slots;
    private Panel panel_Backpack_Grid;
    private Panel panel_Backpack_Menu;
    private Return_Button button_Backpack_Close;

    public List<Control> backpack_Control_List = new List<Control>();
    public List<Control> chest_Control_List = new List<Control>();

    // skills screen elements
    private Panel panel_Skills_Settings;
    private Panel panel_Skills_Stats;
    private Panel panel_Skills_StatVizualisation;
    private StatSettingsLabel label_StatPoints;
    private PlayerStatScreenLabel label_My_General_Stats;
    private Return_Button button_Skills_Close;
    private Stat_Button button_Increase_Health;
    private Stat_Button button_Decrease_Health;
    private Stat_Button button_Increase_Damage;
    private Stat_Button button_Decrease_Damage;
    private Stat_Button button_Increase_Healing;
    private Stat_Button button_Decrease_Healing;

    // chest screen elements
    private Panel panel_Chest_Grid;
    private Panel panel_Chest_Menu;
    private Return_Button button_Chest_Close;

    private List<Backpack_Slot> chest = new List<Backpack_Slot>();
    private All_Items location_drops = new All_Items();
    private List<Item> Drops = new List<Item>();

    // / backpack storage
    public Item_Slot? temp_slot = null;
    public List<Backpack_Slot> backpack_Storage = [];
    public List<Equipement_Slot> equipement_Storage = [];

    //custom variables

    // / tracks the current window
    public Screen_Panel current_Window;

    // / text animation timer
    private System.Windows.Forms.Timer textAnimationTimer;
    private string displayedText = "";
    private int currentTextIndex;

    // / dialog mode
    private string? mode;
    private string? text;

    //Methods   ////////////////////////////////////////////////

    // Backpack and chest backbone

    // / Transfers items between slots
    public void Transfer(Control clicked_element)  // highlight temp slotu by bylo cool
    {
        bool transferable = false;
        if (clicked_element is Item_Slot)
        {   
            Item_Slot recieving_element = (Item_Slot)clicked_element;

            if (temp_slot != null)
            {
                if (temp_slot == recieving_element)
                {
                    transferable = false;
                }
                else if (temp_slot is Backpack_Slot && recieving_element is Backpack_Slot)
                {
                    transferable = true;
                    Debug.WriteLine("Transfering from backpack to backpack");
                }
                else if(temp_slot is Equipement_Slot && recieving_element is Equipement_Slot)
                {   
                    Equipement_Slot equipement_slot_1 = (Equipement_Slot)temp_slot; 
                    Equipement_Slot equipement_slot_2 = (Equipement_Slot)recieving_element;
                    if(equipement_slot_1.equipement_type == equipement_slot_2.equipement_type)
                    {
                        transferable = true;
                        Debug.WriteLine("Transfering from equipement to equipement");
                    }
                }
                else if(temp_slot is Backpack_Slot && recieving_element is Equipement_Slot)
                {
                    Equipement_Slot equipement_slot = (Equipement_Slot)recieving_element;
                    if (temp_slot.stored_item?.equipement_type == equipement_slot.equipement_type)
                    {
                        transferable = true;
                        Debug.WriteLine("Transfering from backpack to equipement");
                    }
                }
                else if(temp_slot is Equipement_Slot && recieving_element is Backpack_Slot)
                {
                    Equipement_Slot equipement_slot = (Equipement_Slot)temp_slot;
                    if (recieving_element.stored_item == null)
                    {
                        transferable = true;
                        Debug.WriteLine("Transfering from equipement to backpack");
                    }
                    else if (recieving_element.stored_item?.equipement_type == equipement_slot.equipement_type)
                    {
                        transferable = true;
                        Debug.WriteLine("Transfering from equipement to backpack");
                    }
                    else{Debug.WriteLine("Transfering failed, somehow?????");};
                }
                if (transferable == true)
                {
                    Debug.WriteLine("Transfering" + temp_slot.stored_item?.name + "from " + temp_slot.Text + " to " + recieving_element.Text);
                    (temp_slot.stored_item, recieving_element.stored_item)=(recieving_element.stored_item, temp_slot.stored_item);
                    temp_slot.UpdateText();
                    recieving_element.UpdateText();
                    temp_slot = null;
                    player.UpdateStats(equipement_Storage);
                }
                else
                {
                    if (temp_slot.stored_item == null)
                    {
                        temp_slot = recieving_element;
                    }
                    Debug.WriteLine("Transfering failed");
                }
            }
            else
            {   
                if (recieving_element.stored_item == null)
                {
                    Debug.WriteLine("Nothing to store");
                    temp_slot = null;
                }
                else
                {
                    Debug.WriteLine("Item stored");
                    temp_slot = recieving_element;
                }
            }

        }
        else
        {
            Debug.WriteLine("Transfering failed, isn't a slot");
            temp_slot = null;
        }
    }
    // / Assigns Drops to chest
    public void AssignDrops()
    {
        for (int i = 0; i < chest.Count; i++)
        {
            if (i < Drops.Count)
            {
                chest[i].stored_item = Drops[i];
                chest[i].UpdateText();
            }
            else
            {
                chest[i].stored_item = null;
                chest[i].UpdateText();
            }
        }
    }



    // Action button methods

    public void EngageIntoCombat(Enemy enemy, Main_Screen panel)
        {
            current_Enemy = enemy;
            Debug.WriteLine($"{current_Enemy.name} has been summoned!, LVL: {current_Enemy.lvl} HP:{current_Enemy.current_hp}/{current_Enemy.max_hp}");
            label_Enemy_Current_Stats.UpdateText(current_Enemy);

            panel.ChangeMode("Dialog", ref bottom_panels, ref right_panels);
            StartDialog($"You have encountered a {current_Enemy.name}!");
            mode = "Combat";
            text = null;
        }


    public void Attack(ref Enemy enemy, ref Player player, ref Main_Screen panel)
    {
        enemy.current_hp -= player.damage_per_hit;
        Debug.WriteLine($"{player.name} dealt {player.damage_per_hit} points of damage to {enemy.name}");
        label_Enemy_Current_Stats.UpdateText(enemy);
        string damageReport = $"{player.name} dealt {player.damage_per_hit} points of damage to {enemy.name}.";

        panel.ChangeMode("Dialog", ref bottom_panels, ref right_panels);

        if (enemy.current_hp <= 0)
        {
            player.GainXP(enemy.exp);
            StartDialog(damageReport + " " + $"{player.name} has defeated the {enemy.name}!");
            mode = "Chest";
            Drops = location_drops.GenerateDrops(enemy);
        }
        else
        {   
            string play = enemy.Play(ref player);
            if (play == "Defeat")
            {
                label_My_Current_Stats.UpdateText(player);
                StartDialog(damageReport + " " + $"{enemy.name} dealt {enemy.damage_per_hit} points of damage to {player.name}." + " Sadly " + $"{player.name} have been defeated by the {enemy.name}!");
                mode = "Menu";
                return;
            }
            else if (play == "Healed")
            {
                string healed = $"{enemy.healing_amount}";
                if (enemy.current_hp == enemy.max_hp)
                {
                    healed = "to maximum";
                }
                
                label_Enemy_Current_Stats.UpdateText(enemy);
                StartDialog(damageReport + " " + $"{enemy.name} healed {healed} points of health.");
            }
            else if (play == "Attacked")
            {
                label_My_Current_Stats.UpdateText(player);
                StartDialog(damageReport + " " + $"{enemy.name} dealt {enemy.damage_per_hit} points of damage to {player.name}.");
            }
            mode = "Combat";
        }
    }
    public void Heal(ref Enemy enemy, ref Player player, ref Main_Screen panel)
    {
        string healed;
        if ((player.current_hp + player.healing_amount) >= player.max_hp)
        {
            healed = $"{player.max_hp - player.current_hp}";
            player.current_hp = player.max_hp;
        }
        else
        {
            healed = $"{player.healing_amount}";
            player.current_hp += player.healing_amount;
        }

        Debug.WriteLine($"{player.name} healed {healed} points of health.");
        label_My_Current_Stats.UpdateText(player);
        string healReport = $"{player.name} healed {healed} points of health.";

        panel.ChangeMode("Dialog", ref bottom_panels, ref right_panels);

        
        string play = enemy.Play(ref player);
        if (play == "Defeat")
        {
            label_My_Current_Stats.UpdateText(player);
            StartDialog(healReport + " " + $"{enemy.name} dealt {enemy.damage_per_hit} points of damage to {player.name}." + " " + $"Sadly {player.name} has been defeated by the {enemy.name}!");
            mode = "Menu";
            return;
        }
        else if (play == "Healed")
        {
            healed = $"{enemy.healing_amount}";
            label_Enemy_Current_Stats.UpdateText(enemy);
            StartDialog(healReport + " " + $"{enemy.name} healed {healed} points of health.");
        }
        else if (play == "Attacked")
        {
            label_My_Current_Stats.UpdateText(player);
            StartDialog(healReport + " " + $"{enemy.name} dealt {enemy.damage_per_hit} points of damage to {player.name}.");
        }
        mode = "Combat";
    }


    // Dialog window functions

    // / Starts the dialog

    public void StartDialog(string text)
    {
        try
        {
            button_Dialog_Window.Text = "";
            currentTextIndex = 0;
            displayedText = text;
            textAnimationTimer.Start();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }

    // / Text animation
    private void TextAnimationTimer_Tick()
    {
        if (currentTextIndex >= displayedText.Length)
        {
            textAnimationTimer.Stop();
            displayedText = "";
            Debug.WriteLine("Dialog has ended");
        }
        else
        {
            // Add the next character to the Label
            button_Dialog_Window.Text += displayedText[currentTextIndex];
            currentTextIndex++;
        }
    
    }

    // / dialog on click

    public void Continue(ref Main_Screen panel,ref string? mode,ref string? text)
    {
        //continue dialog
        if (mode != null)
        {
            if (mode == "Menu")
            {
                panel.ChangeMode("Menu", ref bottom_panels, ref right_panels);
                player.UpdateStats(equipement_Storage);
                label_My_Current_Stats.UpdateText(player);
            }
            else if (mode == "Combat")
            {
                panel.ChangeMode("Combat", ref bottom_panels, ref right_panels);
            }
            else if (mode == "Chest")
            {
                mode = null;
                panel.ChangeMode("Chest", ref bottom_panels, ref right_panels);
                StartDialog($"The enemy has dropped some items. Have a look!");
            } 
        }
        else
        {
            if (text != null)
            {
                StartDialog(text);
            }
            else
            {
                Debug.WriteLine("interaction disabled");
            } 
        }

    }



    // Project_Vanguard constructor ////////////////////////////////////////////////////////////////////////////////////////////////
    public Project_Vanguard()
    {
        // Form setup 
        InitializeComponent();
        BackColor = Color.Black;

        //initializes the text animation timer
        textAnimationTimer = new()
        {
            Interval = 50 // Adjust this to change the speed of the text animation
        };
        textAnimationTimer.Tick += (_, e) => TextAnimationTimer_Tick();

        // Player setup  
        player = new() 
        {   
            name = "",
        };

        // Screen setups    ////////////////////////////////////////////////////////////////////////////////////////////////

        // Name Screen setup    ////////////////////////////////////////////////

        // / Creates the name select screen
        panel_Name_Selcect_Screen = new() 
        { 
            Name = "Name_Selcect_Screen",
            Visible = true,
        };
        Controls.Add(panel_Name_Selcect_Screen);
        current_Window = panel_Name_Selcect_Screen;

        // / Creates the elements for the name select screen

        textBox_Name_Input = new() 
        {
            BackColor = Color.Black,
            ForeColor = Color.White,
            Location = new Point(320, 240),
            Size = new Size(320, 40),
            MaxLength = 8,
        };
        panel_Name_Selcect_Screen.Controls.Add(textBox_Name_Input);


        button_Name_Confirm = new() 
        {
            Location = new Point(320, 320),
            Text = "Confirm",
        };
        panel_Name_Selcect_Screen.Controls.Add(button_Name_Confirm);

        // Main panel setup     ////////////////////////////////////////////////

        // / Creates the main screen
        panel_Main_Screen = new()
        {
            Name = "Main_Screen",
            Visible = false,
        };
        Controls.Add(panel_Main_Screen);

        // / Creates panels and corresponding elements for the main screen
        panel_GUIWindowL = new() 
        { 
            Size = new Size(480, 420), 
            Location = new Point(0, 0), 
            BackColor = Color.FromArgb(100, 100, 100),
        };
        panel_Main_Screen.Controls.Add(panel_GUIWindowL);

        label_My_Current_Stats = new(player)
        {
            Location = new Point(0, 20),
            Size = new Size(480, 100),
        };
        panel_GUIWindowL.Controls.Add(label_My_Current_Stats);



        panel_GUIWindowR1 = new() 
        { 
            Size = new Size(480, 420), 
            Location = new Point(480, 0), 
            BackColor = Color.FromArgb(50, 50, 50),
        };
        panel_Main_Screen.Controls.Add(panel_GUIWindowR1);
        right_panels.Add(panel_GUIWindowR1);

        button_Select_Location1 = new() 
        {
            Location = new Point(20, 20),
            Text = "Slime Plains",
        };
        panel_GUIWindowR1.Controls.Add(button_Select_Location1);

        button_Select_Location2 = new() 
        {
            Location = new Point(20, 100),
            Text = "Golem Ruins",
        };
        panel_GUIWindowR1.Controls.Add(button_Select_Location2);

        button_Select_Location3 = new() 
        {
            Location = new Point(20, 180),
            Text = "Wyvern Mountains",
        };
        panel_GUIWindowR1.Controls.Add(button_Select_Location3);

        button_Select_Location4 = new() 
        {
            Location = new Point(20, 260),
            Text = "Dragon's Lair",
        };
        panel_GUIWindowR1.Controls.Add(button_Select_Location4);

        panel_Menu = new() 
        {
            Size = new Size(960, 120), 
            Location = new Point(0, 420), 
            BackColor = Color.FromArgb(50, 50, 50),
        };
        panel_Main_Screen.Controls.Add(panel_Menu);
        bottom_panels.Add(panel_Menu);

        button_Skills_Open = new() 
        { 
            Location = new Point(496, 20),
            Text = "Skills",
        };
        panel_Menu.Controls.Add(button_Skills_Open);
        
        button_Backpack_Open = new() 
        {
            Location = new Point(728, 20),
            Text = "Backpack", 
        };
        panel_Menu.Controls.Add(button_Backpack_Open);



        panel_GUIWindowR2 = new() 
        { 
            Size = new Size(480, 420), 
            Location = new Point(480, 0), 
            BackColor = Color.FromArgb(50, 50, 50),
            Visible = false,
        };
        panel_Main_Screen.Controls.Add(panel_GUIWindowR2);
        right_panels.Add(panel_GUIWindowR2);

#pragma warning disable CS8604
        label_Enemy_Current_Stats = new(current_Enemy)
        {
            Location = new Point(0, 320),
            Size = new Size(480, 100),
        };
#pragma warning restore CS8604
        panel_GUIWindowR2.Controls.Add(label_Enemy_Current_Stats);

        panel_Combat_Menu = new() 
        {
            Size = new Size(960, 120), 
            Location = new Point(0, 420), 
            BackColor = Color.FromArgb(50, 50, 50),
            Visible = false,
        };
        panel_Main_Screen.Controls.Add(panel_Combat_Menu);
        bottom_panels.Add(panel_Combat_Menu);

        button_Attack = new() 
        {  
            Location = new Point(32, 20),
            Text = "Attack",
        };
        panel_Combat_Menu.Controls.Add(button_Attack);
        
        button_Heal = new() 
        {
            Location = new Point(264, 20),
            Text = "Heal",
        };
        panel_Combat_Menu.Controls.Add(button_Heal);



        panel_GUIWindowR3 = new() 
        { 
            Size = new Size(480, 420), 
            Location = new Point(480, 0), 
            BackColor = Color.FromArgb(50, 50, 50),
            Visible = false,
        };
        panel_Main_Screen.Controls.Add(panel_GUIWindowR3);
        right_panels.Add(panel_GUIWindowR3);

        button_Chest_Open = new() 
        {
            Location = new Point(20, 20),
            Text = "Chest", 
            Size = new Size(100, 40),
        };
        panel_GUIWindowR3.Controls.Add(button_Chest_Open);

        panel_Dialog = new() 
        {
            Size = new Size(960, 120), 
            Location = new Point(0, 420), 
            BackColor = Color.FromArgb(50, 50, 50),
            Visible = false,
        };
        panel_Main_Screen.Controls.Add(panel_Dialog);
        bottom_panels.Add(panel_Dialog);

        button_Dialog_Window = new()
        {
            Location = new Point(0, 0),
        };
        panel_Dialog.Controls.Add(button_Dialog_Window);



        // Backpack screen setup    ////////////////////////////////////////////////

        // / Creates the backpack screen
        panel_Backpack_Screen = new()
        { 
            Name = "Backpack_Screen",
            Visible = false,
        };
        Controls.Add(panel_Backpack_Screen);

        // / Creates panels and their coresponding emements for the backpack screen

        panel_Equipement_Slots = new()
        {
            Size = new Size(240, 540),
            Location = new Point(0, 0),
            BackColor = Color.FromArgb(50, 50, 50),
        };
        panel_Backpack_Screen.Controls.Add(panel_Equipement_Slots);
        backpack_Control_List.Add(panel_Equipement_Slots);

        for (int i = 0; i < 4; i++)
        {
            int x = 75;
            int y =90 + i * 90;
            string type_of_slot = "";

            if (i == 0)
            {
                type_of_slot = "Head";
            }
            else if (i == 1)
            {
                type_of_slot = "Weapon";
            } 
            else if (i == 2)
            {   
                type_of_slot = "Chest";
            }
            else if (i == 3)
            {
                type_of_slot = "Legs";
            }
            Equipement_Slot button_Equipement_slot = new(type_of_slot)
            {
                Location = new Point(x, y),
                Size = new Size(90, 90),
                Text = $"{i}",
            };
            panel_Equipement_Slots.Controls.Add(button_Equipement_slot);
            button_Equipement_slot.Click += (_, e) => Transfer(button_Equipement_slot);
            button_Equipement_slot.UpdateText();
            equipement_Storage.Add(button_Equipement_slot);
        }



        panel_Chest_Grid = new()
        {
            Size = new Size(240, 540),
            Location = new Point(0, 0),
            BackColor = Color.FromArgb(50, 50, 50),
        };
        panel_Backpack_Screen.Controls.Add(panel_Chest_Grid);
        chest_Control_List.Add(panel_Chest_Grid);

        for (int i = 0; i < 6; i++){
            Backpack_Slot button_Chest_grid_element = new()
            {
                Location = new Point(75, i * 90),
                Size = new Size(90, 90),
                Text = $"{6*i}",
            };
            panel_Chest_Grid.Controls.Add(button_Chest_grid_element);
            button_Chest_grid_element.Click += (_, e) => Transfer(button_Chest_grid_element);
            button_Chest_grid_element.UpdateText();
            chest.Add(button_Chest_grid_element);
        }

        panel_Backpack_Grid = new()
        {
            Size = new Size(600, 540),
            Location = new Point(panel_Equipement_Slots.Width, 0),
            BackColor = Color.FromArgb(100, 100, 100),
        };

        panel_Backpack_Screen.Controls.Add(panel_Backpack_Grid);
      
        for (int i = 0; i < 6; i++){
            for (int ii = 0; ii < 6; ii++){
                Backpack_Slot button_Backpack_grid_element = new()
                {
                    Location = new Point(30 + ii * 90, i * 90),
                    Size = new Size(90, 90),
                    Text = $"{6*i + ii}",
                };
                panel_Backpack_Grid.Controls.Add(button_Backpack_grid_element);
                button_Backpack_grid_element.Click += (_, e) => Transfer(button_Backpack_grid_element);
                button_Backpack_grid_element.UpdateText();
                backpack_Storage.Add(button_Backpack_grid_element);
            }
        }



        panel_Backpack_Menu = new()
        {
            Size = new Size(120, 540),
            Location = new Point(panel_Equipement_Slots.Width + panel_Backpack_Grid.Width, 0),
            BackColor = Color.FromArgb(50, 50, 50),
        };
        panel_Backpack_Screen.Controls.Add(panel_Backpack_Menu);
        backpack_Control_List.Add(panel_Backpack_Menu);

        button_Backpack_Close = new()
        { 
            Location = new Point(20, 20), 
            Text = "Close Backpack",
        };
        panel_Backpack_Menu.Controls.Add(button_Backpack_Close);



        panel_Chest_Menu = new()
        {
            Size = new Size(120, 540),
            Location = new Point(panel_Equipement_Slots.Width + panel_Backpack_Grid.Width, 0),
            BackColor = Color.FromArgb(50, 50, 50),
        };
        panel_Backpack_Screen.Controls.Add(panel_Chest_Menu);
        chest_Control_List.Add(panel_Chest_Menu);

        button_Chest_Close = new()
        {
            Location = new Point(20, 20),
            Text = "Close Chest",
        };
        panel_Chest_Menu.Controls.Add(button_Chest_Close);



        // Skills screen setup  ////////////////////////////////////////////////

        // / Creates the skills screen
        panel_Skills_Screen = new() 
        {
            Name = "Skills_Screen",
            Visible = false,
        };
        Controls.Add(panel_Skills_Screen);

        // / Creates panels and coresponding elements for the skills screen
        panel_Skills_Settings = new()
        {
            Size = new Size(480, 270),
            Location = new Point(0, 0),
            BackColor = Color.FromArgb(50, 50, 50),
        };
        panel_Skills_Screen.Controls.Add(panel_Skills_Settings);
        
        label_StatPoints = new(player)
        {
            Location = new Point(20, 50),
            Size = new Size(300, 160),
        };
        panel_Skills_Settings.Controls.Add(label_StatPoints);

        button_Increase_Health = new("Health", true)
        {
            Location = new Point(label_StatPoints.Width + label_StatPoints.Location.X, label_StatPoints.Location.Y + Stat_Button.size.Height - 5),
            Text = "+",
        };
        panel_Skills_Settings.Controls.Add(button_Increase_Health);

        button_Decrease_Health = new("Health", false)
        {
            Location = new Point(button_Increase_Health.Location.X + Stat_Button.size.Width, button_Increase_Health.Location.Y),
            Text = "-",
        };
        panel_Skills_Settings.Controls.Add(button_Decrease_Health);

        button_Increase_Damage = new("Damage", true)
        {
            Location = new Point(button_Increase_Health.Location.X, button_Increase_Health.Location.Y + Stat_Button.size.Height),
            Text = "+",
        };  
        panel_Skills_Settings.Controls.Add(button_Increase_Damage);

        button_Decrease_Damage = new("Damage", false)
        {
            Location = new Point(button_Increase_Damage.Location.X + Stat_Button.size.Width, button_Increase_Damage.Location.Y),
            Text = "-",
        };
        panel_Skills_Settings.Controls.Add(button_Decrease_Damage);

        button_Increase_Healing = new("Healing", true)
        {
            Location = new Point(button_Increase_Damage.Location.X, button_Increase_Damage.Location.Y + Stat_Button.size.Height),
            Text = "+",
        };
        panel_Skills_Settings.Controls.Add(button_Increase_Healing);

        button_Decrease_Healing = new("Healing", false)
        {
            Location = new Point(button_Increase_Healing.Location.X + Stat_Button.size.Width, button_Increase_Healing.Location.Y),
            Text = "-",
        };
        panel_Skills_Settings.Controls.Add(button_Decrease_Healing);



        panel_Skills_Stats = new()
        {
            Size = new Size(480, 540),
            Location = new Point(panel_Skills_Settings.Width, 0),
            BackColor = Color.FromArgb(100, 100, 100),
        };
        panel_Skills_Screen.Controls.Add(panel_Skills_Stats);

        label_My_General_Stats = new(player)
        {
            Location = new Point(20, 20),
            Size = new Size(300, 540),
        };
        panel_Skills_Stats.Controls.Add(label_My_General_Stats);

        button_Skills_Close = new() 
        {
            Location = new Point(panel_Skills_Stats.Width - Return_Button.size.Width - 20, 20), // implementuj později všude, aby fungoval fullscreen XD
            Text = "Close Skills",
        };
        panel_Skills_Stats.Controls.Add(button_Skills_Close);



        panel_Skills_StatVizualisation = new()
        {
            Size = new Size(480, 270),
            Location = new Point(0, panel_Skills_Settings.Height),
            BackColor = Color.FromArgb(0, 0, 0),
        };
        panel_Skills_Screen.Controls.Add(panel_Skills_StatVizualisation);



        // event handlers   ////////////////////////////////////////////////////////////////////////////////////////////////
        // / creates event handlers for name select screen
        button_Name_Confirm.Click += (_, e) => 
        {   
            if (textBox_Name_Input.Text == "")
            {
                //tudum tam není ^^
            }
            else
            {
                player.name = textBox_Name_Input.Text;
                label_My_Current_Stats.UpdateText(player);
                label_My_General_Stats.UpdateText(player);
                panel_Main_Screen.Open_Me(ref current_Window);
            }
        };

        // / creates event handlers for main screen
        button_Select_Location1.Click += (_, e) => EngageIntoCombat(new Slime(), panel_Main_Screen);
        button_Select_Location2.Click += (_, e) => EngageIntoCombat(new Golem(), panel_Main_Screen);
        button_Select_Location3.Click += (_, e) => EngageIntoCombat(new Wyvern(), panel_Main_Screen);
        button_Select_Location4.Click += (_, e) => EngageIntoCombat(new Dragon(), panel_Main_Screen);

        button_Skills_Open.Click += (_, e) => {panel_Skills_Screen.Open_Me(ref current_Window); label_StatPoints.UpdateText(player); label_My_General_Stats.UpdateText(player);};
        button_Backpack_Open.Click += (_, e) => panel_Backpack_Screen.Open_Me(ref current_Window, true, ref backpack_Control_List, ref chest_Control_List);
        
        button_Attack.Click += (_, e) => Attack(ref current_Enemy, ref player, ref panel_Main_Screen);
        button_Heal.Click += (_, e) => Heal(ref current_Enemy ,ref player, ref panel_Main_Screen);

        button_Dialog_Window.Click += (_, e) => Continue(ref panel_Main_Screen, ref mode, ref text);

        button_Chest_Open.Click += (_, e) => {panel_Backpack_Screen.Open_Me(ref current_Window, false, ref backpack_Control_List, ref chest_Control_List); AssignDrops();};

        // / creates event handlers for backpack screen
        panel_Equipement_Slots.Click += (_, e) => Transfer(panel_Equipement_Slots);
        panel_Chest_Grid.Click += (_, e) => Transfer(panel_Chest_Grid);        
        
        panel_Backpack_Grid.Click += (_, e) => Transfer(panel_Backpack_Grid);
        
        button_Chest_Close.Click += (_, e) => {panel_Main_Screen.Open_Me(ref current_Window, ref bottom_panels, ref right_panels); player.UpdateStats(equipement_Storage); label_My_Current_Stats.UpdateText(player);};
        button_Backpack_Close.Click += (_, e) => {panel_Main_Screen.Open_Me(ref current_Window); player.UpdateStats(equipement_Storage); label_My_Current_Stats.UpdateText(player);};

        // / creates event handlers for skills screen
        button_Increase_Health.Click += (_, e) => {button_Increase_Health.ChangeStat(button_Increase_Health.mestat, button_Increase_Health.meincrease, player, label_StatPoints, equipement_Storage); label_My_General_Stats.UpdateText(player); label_StatPoints.UpdateText(player);};
        button_Decrease_Health.Click += (_, e) => {button_Decrease_Health.ChangeStat(button_Decrease_Health.mestat, button_Decrease_Health.meincrease, player, label_StatPoints, equipement_Storage); label_My_General_Stats.UpdateText(player); label_StatPoints.UpdateText(player);};
        
        button_Increase_Damage.Click += (_, e) => {button_Increase_Damage.ChangeStat(button_Increase_Damage.mestat, button_Increase_Damage.meincrease, player, label_StatPoints, equipement_Storage); label_My_General_Stats.UpdateText(player); label_StatPoints.UpdateText(player);};
        button_Decrease_Damage.Click += (_, e) => {button_Decrease_Damage.ChangeStat(button_Decrease_Damage.mestat, button_Decrease_Damage.meincrease, player, label_StatPoints, equipement_Storage); label_My_General_Stats.UpdateText(player); label_StatPoints.UpdateText(player);};
        
        button_Increase_Healing.Click += (_, e) => {button_Increase_Healing.ChangeStat(button_Increase_Healing.mestat, button_Increase_Healing.meincrease, player, label_StatPoints, equipement_Storage); label_My_General_Stats.UpdateText(player); label_StatPoints.UpdateText(player);};
        button_Decrease_Healing.Click += (_, e) => {button_Decrease_Healing.ChangeStat(button_Decrease_Healing.mestat, button_Decrease_Healing.meincrease, player, label_StatPoints, equipement_Storage); label_My_General_Stats.UpdateText(player); label_StatPoints.UpdateText(player);};
        
        button_Skills_Close.Click += (_, e) => {panel_Main_Screen.Open_Me(ref current_Window);  label_My_Current_Stats.UpdateText(player);};



        // game Objects and narrative   ///////////////////////////////////////////////////////////////////////////////////////////////

        // / Intro gear
        /*
        equipement_Storage[0].stored_item = new()
        {
            name = "Protector's Helmet",
            equipement_type = "Head",
        };

        equipement_Storage[1].stored_item = new()
        {
            name = "Elite Guard's Twohandedsword",
            equipement_type = "Weapon",
        };

        equipement_Storage[2].stored_item = new()
        {
            name = "Protector's Chestplate",
            equipement_type = "Chest",
        };

        equipement_Storage[3].stored_item = new()
        {
            name = "Protector's Leggings",
            equipement_type = "Legs",
        };
        
        */
        foreach (Equipement_Slot slot in equipement_Storage)
        {
            slot.UpdateText();
        }
    }
}

// "Life is hard, so let's make it harder by working smarter." - most sane brogrammer Burymuru 2024

/*

Roadmap:

flee button after 10 rounds

discard item slot
loose stuff on death

UI polish
balance
AI

story
sounds
entity reprezentace (2D graphics)

bugs:

exotic names (třeba, že sou to jen mezery nebo čísla)

*/

/*
story:
Intro:
vojáku! ber vybavení a hned na pozici, útočí drak!
//veme vybavení z bedny
To to trvalo, ale vidím že jsi připravený na útok. Bohužel ten drak už rozcupoval většinu našich vojáků, takže to bude na tobě.
//útok
//souboj je rigged a prohraješ XD
//naštěstí jsi přežil a drak je pryč, ale město taky. tvoje vybavení je na paděru, takže budeš muset sehnat nový.
Midgame:
//něco XD
Endgame:
Znovu se setkáváš s drakem, ale tentokrát jsi připravený. Po dlouhém souboji drak padá k zemi a ty jsi hrdina, ale čeho? Město je zničené a všichni tví přátelé jsou mrtví. 
*/