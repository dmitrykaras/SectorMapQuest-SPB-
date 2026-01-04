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
        //получаем сектор по координатам игрока
        var sector = map.GetAt(player.Q, player.R);

        if (sector == null)
            return false;

        if (sector.IsOpened)
            return false;

        sector.IsOpened = true;
        return true;
    }

}