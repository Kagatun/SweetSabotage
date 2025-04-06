using TMPro;
using UnityEngine;
using YG;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentGold;

    private void Start()
    {
        DisplayInvoice();
    }

    public void DisplayInvoice()
    {
        _currentGold.text = $"{YandexGame.savesData.Gold}";
    }
}
