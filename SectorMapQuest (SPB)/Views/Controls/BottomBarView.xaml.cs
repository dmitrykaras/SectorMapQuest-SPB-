namespace SectorMapQuest__SPB_.Views.Controls;

public partial class BottomBarView : ContentView
{
    public event Action FeedClicked;
    public event Action MapClicked;
    public event Action StatsClicked;
    public event Action ProfileClicked;

    public BottomBarView()
    {
        InitializeComponent();
    }

    void OnFeedClicked(object sender, EventArgs e) => FeedClicked?.Invoke();
    void OnMapClicked(object sender, EventArgs e) => MapClicked?.Invoke();
    void OnStatsClicked(object sender, EventArgs e) => StatsClicked?.Invoke();
    void OnProfileClicked(object sender, EventArgs e) => ProfileClicked?.Invoke();
}