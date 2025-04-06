using UnityEngine;
using YG;

public class LoaderImage : MonoBehaviour
{
    [SerializeField] private ImageLoadYG _imageLoadYG;

    private void Start()
    {
        if (_imageLoadYG && YandexGame.purchases[0].currencyImageURL != string.Empty && YandexGame.purchases[0].currencyImageURL != null)
            _imageLoadYG.Load(YandexGame.purchases[0].currencyImageURL);
    }
}
