using YG;

namespace InterfaceUI
{
    public class ResetButton : ButtonHandler
    {
        protected override void OnButtonClick()
        {
            YandexGame.ResetSaveProgress();
        }
    }
}