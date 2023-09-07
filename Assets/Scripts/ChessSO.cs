using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ChessSO")]
public class ChessSO : ScriptableObject
{
    public Sprite sprite;

    public bool canRunThroughOstacle;

    public bool isMainChess;
}
