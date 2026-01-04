namespace SectorMapQuest.Managers;

public class ProgressManager
{
    //список всех секторов за которыми отслеживается прогресс
    public List<Sector> Sectors { get; } = new();

    //открытие указанного сектора, если он ещё не октрыт
    public void OpenSector(Sector sector)
    {
        if (sector.IsOpened)
            return;

        sector.IsOpened = true;
    }

    //попытка открыть сектор в текущей позиции игрока
    public bool TryOpenSectorAtPlayerPosition(
        PlayerPositionManager player,
        MapManager map)
    {
        var sector = map.GetSectorAtWorldPosition(player.Position);

        if (sector == null || sector.IsOpened)
            return false;

        sector.Open();
        SectorOpened?.Invoke(sector);
        return true;
    }

    public event Action<Sector>? SectorOpened;


}