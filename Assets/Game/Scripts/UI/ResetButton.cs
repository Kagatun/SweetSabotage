using YG;

public class ResetButton : ButtonHandler
{
    protected override void OnButtonClick()
    {
        YandexGame.ResetSaveProgress();
    }
}
