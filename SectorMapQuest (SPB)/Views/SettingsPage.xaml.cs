namespace SectorMapQuest.Views.Settings;

public partial class SettingsView : ContentView
{
    public event Action? BackRequested;

    public SettingsView()
    {
        InitializeComponent();
    }
}