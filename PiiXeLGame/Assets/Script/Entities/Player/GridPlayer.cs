using Script.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlayer : Entity
{

    public List<IAction> moveset = new List<IAction>();
    public int moveIndex = 0;


    public override void previewActionInMoveset()
    {
        if (!isVisible) return;
        throw new System.NotImplementedException();
    }

    public override void doActionInMoveset()
    {
        if (!isVisible) return;
        //moveset[moveIndex].act();
    }
}
