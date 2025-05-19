using YG;

namespace Shop
{
    public class UpgradeStun : ImprovementHandler
    {
        protected override int GetIndexBuy() =>
            YandexGame.savesData.ExtraStun;

        protected override void UpdateIndex()
        {
            base.UpdateIndex();
            YandexGame.savesData.ExtraStun++;
        }
    }
}