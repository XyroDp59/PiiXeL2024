using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthSystem
{

    public void AddToHealth(int health);

    public void Die();
}
