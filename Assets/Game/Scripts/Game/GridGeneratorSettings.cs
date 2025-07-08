using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class GridGeneratorSettings
    {
        [field: SerializeField] public Vector2Int GridSize { get; private set; }
        [field: SerializeField] public Vector3 StartPositionGridGenerator { get; private set; }
        [field: SerializeField] public int BrokenCellsCount { get; private set; }
        [field: SerializeField] public int RequiredCellsCount { get; private set; }
    }
}
