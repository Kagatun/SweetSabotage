using YG;

public class UpgradeLuck : ImprovementHandler
{
    protected override int GetIndexBuy() =>
         YandexGame.savesData.ChanceLuck;

    protected override void UpdateIndex()
    {
        base.UpdateIndex();
        YandexGame.savesData.ChanceLuck++;
    }
}
