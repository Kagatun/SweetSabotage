using YG;

public class UpgradeExtraCharge : ImprovementHandler
{
    protected override int GetIndexBuy() =>
         YandexGame.savesData.ExtraCharge;

    protected override void UpdateIndex()
    {
        base.UpdateIndex();
        YandexGame.savesData.ExtraCharge++;
    }
}
