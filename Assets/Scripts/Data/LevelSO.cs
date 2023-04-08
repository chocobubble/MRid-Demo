using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRidDemo
{

    // holds basic level information (label name, level number, scene name for loading, thumbnail graphic for display, etc.)
    [CreateAssetMenu(fileName = "Assets/Resources/GameData/Levels/LevelData", menuName = "MRid/Level", order = 11)]
    public class LevelSO : ScriptableObject
    {
        public string dungeonName;
        // kind of dungeon
        public int dungeonNumber;
        // level of dungeon
        public int dungeonHardness;
        public Sprite dungeonSprite;
        public string sceneName;
        public int goldEarning;
        public int xpGetting;
        public List<GameObject> mainEnemies = new List<GameObject>();
        public List<GameObject> minions = new List<GameObject>();
        public List<CharacterSO> mainEnemyList = new List<CharacterSO>();
        public List<CharacterSO> minionList = new List<CharacterSO>();
    }
}

