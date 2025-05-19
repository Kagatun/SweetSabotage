using YG;

namespace Shop
{
    public class UpgradePaintCooldown : ImprovementHandler
    {
        protected override int GetIndexBuy() =>
            YandexGame.savesData.CooldownPaint;

        protected override void UpdateIndex()
        {
            base.UpdateIndex();
            YandexGame.savesData.CooldownPaint++;
        }
    }
}