using System.Collections.Generic;
using Script.Entities;
using Script.Portals;
using UnityEngine;

namespace Script.Environment
{
    public class World : MonoBehaviour
    {
        [SerializeField] private Grid mainGrid;
        [SerializeField] private List<Portal> portals;
        [SerializeField] private List<Entity> entities;
    }
}