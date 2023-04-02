using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    // holds basic level information (label name, level number, scene name for loading, thumbnail graphic for display, etc.)
    [CreateAssetMenu(fileName = "Assets/Resources/GameData/Levels/LevelData", menuName = "MRid/Level", order = 11)]
    public class LevelSO : ScriptableObject
    {
        public int levelNumber;
        public string levelHardness;
        public Sprite thumbnail;
        public string sceneName;
        public int goldEarning;
        public int xpGetting;
    }

