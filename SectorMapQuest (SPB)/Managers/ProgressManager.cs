namespace SectorMapQuest.Managers;

public class ProgressManager
{
    public List<Sector> Sectors { get; } = new();

    public void OpenSector(Sector sector)
    {
        if (sector.IsOpened)
            return;

        sector.IsOpened = true;
    }

    public bool TryOpenSectorAtPlayerPosition(
    PlayerPositionManager player,
    MapManager map)
    {
        var sector = map.GetAt(player.Q, player.R);

        if (sector == null)
            return false;

        if (sector.IsOpened)
            return false;

        sector.IsOpened = true;
        return true;
    }

}