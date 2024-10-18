namespace RPG_Project_Vanguard;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new Project_Vanguard());
    }    
} 

// "Life is hard, so let's make it harder by working smarter." - most sane brogrammer Burymuru 2024