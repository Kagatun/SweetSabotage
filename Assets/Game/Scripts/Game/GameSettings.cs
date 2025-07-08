using System.Collections.Generic;
using UnityEngine;
using YG;

namespace Game
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public int NumberLevel { get; private set; } = YandexGame.savesData.LevelNumber;
        
        [field: Header("Tutorials")] 
        [field: SerializeField] public bool HasActiveTutorial { get; private set; } 
        [field: SerializeField] public int IndexTutorial { get; private set; }
        
        [Header("Color")] 
        [SerializeField] private List<bool> _activeColors;

        public List<bool> ActiveСolors =>  new (_activeColors);
        
        [field:Header("Geese")] 
        [SerializeField] private List<int> _countGeese;
        
        public IReadOnlyList <int> CountGeese => _countGeese;
        
        [Header("Teleport Figures")] 
        [SerializeField] private List<int> _countTeleportFigures;
        
        public List<int> CountTeleportFigures => new (_countTeleportFigures) ;
        
        [Header("Grid Generator")] 
        [SerializeField] private List<GridGeneratorSettings> _gridGeneratorsSettings;
        
        public IReadOnlyList<GridGeneratorSettings> GridGeneratorsSettings => _gridGeneratorsSettings;
        
        [field:Header("BoostsGenerator")] 
        [field: SerializeField] public int BoostsCount { get; private set; }
        
        [Header("Paint")] 
        [SerializeField] private List<bool> _activePaints;
        
        public IReadOnlyList <bool> ActivePaints => _activePaints;
        
        [field: Header("Time / Max Score")]
        [field: SerializeField] public int Time { get; private set; }
        [field: SerializeField] public int MaxScore { get; private set; }
    }
}