using UnityEngine;

public class RestarterLevel : ButtonHandler
{
    [SerializeField] private AdapterBetweenScenes _adapterBetweenScenes;

    protected override void OnButtonClick()
    {
        Time.timeScale = 1;
        _adapterBetweenScenes.RestartLevel();
    }
}
