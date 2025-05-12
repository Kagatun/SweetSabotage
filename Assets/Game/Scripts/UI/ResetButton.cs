using YG;

namespace InterfaceUI
{
    public class ResetButton : ButtonHandler
    {
        protected override void OnButtonClick()
        {
            YandexGame.ResetSaveProgress();
        }
        
        protected override void OnEnableAction(){}
        
        protected override void OnDisableAction(){}
    }
}
