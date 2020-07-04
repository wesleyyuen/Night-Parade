using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {
    public string characterName;
    public enum SetSelectionMode {
        Random,
        Sequential
    }
    public SetSelectionMode setSelectionMode;
    public int currentSetIndex;

    [System.Serializable]
    public class SentenceSet {
        [TextArea (3, 10)]
        public string[] sentences;
    }
    public SentenceSet[] sentenceSets;

}