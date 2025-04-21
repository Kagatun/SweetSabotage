using YG;

namespace Shop
{
    public class UpgradeMaxTime : ImprovementHandler
    {
        protected override int GetIndexBuy() =>
             YandexGame.savesData.ExtraTime;

        protected override void UpdateIndex()
        {
            base.UpdateIndex();
            YandexGame.savesData.ExtraTime++;
        }
    }
}
