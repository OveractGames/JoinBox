using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptUtils;

public class GridCell : ObjectSequenceSystem
{
    public enum CubeType
    {
        EMPTY,
        PLAYER,
        TARGET,
        X_100X100,
        X_200X100,
        X_300X100,
        X_400X100,
        Y_100X200,
        Y_100X300,
        Y_100X400
    }
    public CubeType type = CubeType.EMPTY;
    private CubeType lastType = CubeType.EMPTY;
    new void Update()
    {
        if (type != lastType)
        {
            setCurrentChildIndex((int)type);
            lastType = type;
            if (type == CubeType.PLAYER)
                CurrentChild.tag = "Player";
            else if (type == CubeType.TARGET)
                CurrentChild.tag = "Target";
            else
                CurrentChild.tag = "Enemy";
        }
    }
}
