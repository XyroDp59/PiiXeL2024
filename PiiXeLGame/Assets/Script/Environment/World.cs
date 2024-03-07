using System.Collections.Generic;
using Script.Entities;
using Script.GridSystem;
using Script.Portals;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Environment
{
    public class World : MonoBehaviour
    {
        [FormerlySerializedAs("mainGrid")] [SerializeField] private GameGrid mainGameGrid;
        [SerializeField] private List<Portal> portals;
        [SerializeField] private List<Entity> entities;
    }
}