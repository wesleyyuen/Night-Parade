using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue
{
    public string characterName;
    public enum SetSelectionMode
    {
        Random,
        Sequential
    }
    public SetSelectionMode setSelectionMode;
    [HideInInspector] public int currentSetIndex; // for Sequential

    [System.Serializable]
    public class Sentence
    {
        public Sentence(string sentenceString, UnityEvent sentenceCallback)
        {
            sentence = sentenceString;
            callback = sentenceCallback;
        }

        [TextArea (3, 10)]
        public string sentence;
        public UnityEvent callback;
    }
    public class SentenceSet
    {
        public Sentence[] sentences;
    }
    public SentenceSet[] sentenceSets;

}