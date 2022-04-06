using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public int speakerId;
    public string name;
    
    [TextArea(3, 10)]
    public string[] sentences;
}
