using YG;

namespace Shop
{
    public class UpgradeChanceRemoveTime : ImprovementHandler
    {
        protected override int GetIndexBuy() =>
             YandexGame.savesData.ChanceRemoveTime;

        protected override void UpdateIndex()
        {
            base.UpdateIndex();
            YandexGame.savesData.ChanceRemoveTime++;
        }

        protected override void OnEnableAction(){}
        
        protected override void OnDisableAction(){}
    }
}
