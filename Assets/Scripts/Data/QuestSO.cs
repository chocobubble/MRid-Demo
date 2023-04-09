using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public enum questState
    {
        NOTACCEPT,
        ACCEPT,
        SUCCESS
    }
    // holds basic level information (label name, level number, scene name for loading, thumbnail graphic for display, etc.)
    [CreateAssetMenu(fileName = "Assets/Resources/GameData/Quest/QuestData", menuName = "MRid/Quest", order = 15)]

    public class QuestSO : ScriptableObject
    {
        public string questName;
        public int questNumber = 1;
        public questState state = questState.NOTACCEPT;
        //public bool isSuccess = false;


    }

