using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Entities
{
    public class testEntity : Entity
    {
        int n = 0;


        public override void doActionInMoveset()
        {
            if(n%2 == 0) MoveForward();
            else { Rotate(1); MoveForward(); }

            n++;
            //throw new System.NotImplementedException();
        }
    }
}