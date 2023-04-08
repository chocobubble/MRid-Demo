using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRidDemo
{
    // holds basic level information (label name, level number, scene name for loading, thumbnail graphic for display, etc.)
    [CreateAssetMenu(fileName = "Assets/Resources/GameData/GameData", menuName = "MRid/GameData", order = 16)]
    public class GameDataSO : ScriptableObject
    {
        public int dungeonNumber = 1;
        public List<int> dungeonLevels = new List<int>();
        public List<LevelSO> dungeons = new List<LevelSO>();

        //public int curFirstDungeonLevel = 1;
        //public int curSecondDungeonLevel = 1;

    }
}

