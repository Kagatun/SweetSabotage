using System.Collections.Generic;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int money = 1;                       // Можно задать полям значения по умолчанию
        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];

        // Ваши сохранения
        public List<float> Times;
        public int Gold;

        public int ExtraTime;
        public int ExtraStun;
        public int CooldownPaint;
        public int ChanceLuck;
        public int ChanceRemoveTime;
        public int ExtraCharge;

        public int IndexLanguage;
        public int LevelIndex = 1;
        public int LevelNumber;

        public float MasterVolume = 1f;
        public float MusicVolume = 1f;
        public float EffectsVolume = 1f;

        public float OffsetFigureHorizontal = -2.5f;
        public float OffsetFigureVertical = 0.8f;

        public float OffsetPaintLeftHorizontal = 3.5f;
        public float OffsetPaintLeftVertical = -3.5f;

        public float OffsetPaintRightHorizontal = -3.5f;
        public float OffsetPaintRightVertical = -3.5f;

        public bool IsBuy;
        public bool IsDesktop = true;
        public bool IsFirstEntrance = true;

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива
            openLevels[1] = true;
        }
    }
}
