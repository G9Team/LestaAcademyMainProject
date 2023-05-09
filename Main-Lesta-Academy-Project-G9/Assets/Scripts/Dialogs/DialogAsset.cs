using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogAsset", menuName = "Dialog Asset", order = 1)]
public class DialogAsset : ScriptableObject
{
    [System.Serializable]
    public class ArrayGroup
    {
        public enum GroupType
        {
            DIALOG = 0,
            EVENT = 1,
            ANSWER = 2
        }
        [System.Serializable]
        public struct AnswerGroup
        {
            public string msg;
            public int force;
        }

        public string eventName;
        public string characterName;
        public string dialog;
        public float minDialogTime;
        public GroupType groupType;
        public int force = 0;
        public bool hideName;
        public AudioClip clip;
        public AnswerGroup[] answers = new AnswerGroup[0];
    }

    [System.Serializable]
    public class ArrayCharacters
    {
        public enum CharacterPosition
        {
            LEFT = 0,
            RIGHT = 1
        }
        public string characterName;
        public CharacterPosition position;
        public Sprite sprite;
    }

    public ArrayCharacters[] arrayCharacters;
    public ArrayGroup[] arrayGroups;
}
