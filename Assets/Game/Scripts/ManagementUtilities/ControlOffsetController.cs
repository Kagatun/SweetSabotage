using InterfaceUI;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace ManagementUtilities
{
    public class ControlOffsetController : ButtonHandler
    {
        [SerializeField] private Slider _offsetFigureHorizontal;
        [SerializeField] private Slider _offsetFigureVertical;

        [SerializeField] private Slider _offsetPaintLeftHorizontal;
        [SerializeField] private Slider _offsetPaintLeftVertical;

        [SerializeField] private Slider _offsetPaintRightHorizontal;
        [SerializeField] private Slider _offsetPaintRightVertical;

        private float _defaultOffsetFigureHorizontal = -2.5f;
        private float _defaultOffsetFigureVertical = 0.8f;
        private float _defaultOffsetPaintHorizontal = 3.5f;
        private float _defaultOffsetPaintVertical = -3.5f;

        private void Start()
        {
            if (YandexGame.SDKEnabled)
                LoadSettings();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _offsetFigureHorizontal.onValueChanged.AddListener(ChangeVolumeOffsetFigureHorizontal);
            _offsetFigureVertical.onValueChanged.AddListener(ChangeVolumeOffsetFigureVertical);

            _offsetPaintLeftHorizontal.onValueChanged.AddListener(ChangeVolumeOffsetPaintLeftHorizontal);
            _offsetPaintLeftVertical.onValueChanged.AddListener(ChangeVolumeOffsetPaintLeftVertical);


            _offsetPaintRightHorizontal.onValueChanged.AddListener(ChangeVolumeOffsetPaintRightHorizontal);
            _offsetPaintRightVertical.onValueChanged.AddListener(ChangeVolumeOffsetPaintRightVertical);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _offsetFigureHorizontal.onValueChanged.RemoveListener(ChangeVolumeOffsetFigureHorizontal);
            _offsetFigureVertical.onValueChanged.RemoveListener(ChangeVolumeOffsetFigureVertical);

            _offsetPaintLeftHorizontal.onValueChanged.RemoveListener(ChangeVolumeOffsetPaintLeftHorizontal);
            _offsetPaintLeftVertical.onValueChanged.RemoveListener(ChangeVolumeOffsetPaintLeftVertical);

            _offsetPaintRightHorizontal.onValueChanged.RemoveListener(ChangeVolumeOffsetPaintRightHorizontal);
            _offsetPaintRightVertical.onValueChanged.RemoveListener(ChangeVolumeOffsetPaintRightVertical);
        }

        protected override void OnButtonClick()
        {
            _offsetFigureHorizontal.value = _defaultOffsetFigureHorizontal;
            _offsetFigureVertical.value = _defaultOffsetFigureVertical;

            _offsetPaintLeftHorizontal.value = _defaultOffsetPaintHorizontal;
            _offsetPaintLeftVertical.value = _defaultOffsetPaintVertical;

            _offsetPaintRightHorizontal.value = -_defaultOffsetPaintHorizontal;
            _offsetPaintRightVertical.value = _defaultOffsetPaintVertical;

            ChangeVolumeOffsetFigureHorizontal(_defaultOffsetFigureHorizontal);
            ChangeVolumeOffsetFigureVertical(_defaultOffsetFigureVertical);

            ChangeVolumeOffsetPaintLeftHorizontal(_defaultOffsetPaintHorizontal);
            ChangeVolumeOffsetPaintLeftVertical(_defaultOffsetPaintVertical);

            ChangeVolumeOffsetPaintRightHorizontal(-_defaultOffsetPaintHorizontal);
            ChangeVolumeOffsetPaintRightVertical(_defaultOffsetPaintVertical);
        }

        private void ChangeVolumeOffsetFigureHorizontal(float volume)
        {
            YandexGame.savesData.OffsetFigureHorizontal = volume;
            YandexGame.SaveProgress();
        }

        private void ChangeVolumeOffsetFigureVertical(float volume)
        {
            YandexGame.savesData.OffsetFigureVertical = volume;
            YandexGame.SaveProgress();
        }

        private void ChangeVolumeOffsetPaintLeftHorizontal(float volume)
        {
            YandexGame.savesData.OffsetPaintLeftHorizontal = volume;
            YandexGame.SaveProgress();
        }

        private void ChangeVolumeOffsetPaintLeftVertical(float volume)
        {
            YandexGame.savesData.OffsetPaintLeftVertical = volume;
            YandexGame.SaveProgress();
        }


        private void ChangeVolumeOffsetPaintRightHorizontal(float volume)
        {
            YandexGame.savesData.OffsetPaintRightHorizontal = volume;
            YandexGame.SaveProgress();
        }

        private void ChangeVolumeOffsetPaintRightVertical(float volume)
        {
            YandexGame.savesData.OffsetPaintRightVertical = volume;
            YandexGame.SaveProgress();
        }

        private void LoadSettings()
        {
            _offsetFigureHorizontal.value = YandexGame.savesData.OffsetFigureHorizontal;
            _offsetFigureVertical.value = YandexGame.savesData.OffsetFigureVertical;

            _offsetPaintLeftHorizontal.value = YandexGame.savesData.OffsetPaintLeftHorizontal;
            _offsetPaintLeftVertical.value = YandexGame.savesData.OffsetPaintLeftVertical;

            _offsetPaintRightHorizontal.value = YandexGame.savesData.OffsetPaintRightHorizontal;
            _offsetPaintRightVertical.value = YandexGame.savesData.OffsetPaintRightVertical;


            ChangeVolumeOffsetFigureHorizontal(YandexGame.savesData.OffsetFigureHorizontal);
            ChangeVolumeOffsetFigureVertical(YandexGame.savesData.OffsetFigureVertical);

            ChangeVolumeOffsetPaintLeftHorizontal(YandexGame.savesData.OffsetPaintLeftHorizontal);
            ChangeVolumeOffsetPaintLeftVertical(YandexGame.savesData.OffsetPaintLeftVertical);

            ChangeVolumeOffsetPaintRightHorizontal(YandexGame.savesData.OffsetPaintRightHorizontal);
            ChangeVolumeOffsetPaintRightVertical(YandexGame.savesData.OffsetPaintRightVertical);
        }
    }
}
