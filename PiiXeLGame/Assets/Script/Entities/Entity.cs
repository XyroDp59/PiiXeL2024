using System.Collections.Generic;
using UnityEngine;

namespace Script.Entities
{
    public abstract class Entity : Script.GridSystem.GridObject
    {
        public static int TOTAL_ACTION_BAR { get; private set; } = 50; //
        [SerializeField] private float actionFrequency;
        [SerializeField] private float movementSpeed;
        private float theta;

        private Vector2 prevPos;
        private bool isMoving = false;
        public float actionBar { get; private set; } = 0;
        private float currRotation;

        private List<IAction> moveset;
        abstract public void doActionInMoveset();

        private void Start()
        {
            prevPos = transform.position;
            if (grid.gridType == Script.GridSystem.GridType.Square)
            {
                theta = 2 * Mathf.PI / 4;
            }
            if (grid.gridType == Script.GridSystem.GridType.Triangle)
            {
                theta = 2 * Mathf.PI / 3;
            }
            if (grid.gridType == Script.GridSystem.GridType.Hexagon)
            {
                theta = 2 * Mathf.PI / 6;
            }

            currRotation = Mathf.PI / 2;
        }

        public void Rotate(int nbFace)
        {
            currRotation += theta * nbFace;
        }

        public void MoveForward()
        {
            isMoving = true;
        }

        // Update is called once per frame
        void Update()
        {
            if(!isMoving && actionBar >= TOTAL_ACTION_BAR)
            {
                actionBar = 0;
                prevPos = transform.position;
                doActionInMoveset();
            }
            if (actionBar <= TOTAL_ACTION_BAR) actionBar += actionFrequency * Time.deltaTime;

            if (isMoving)
            {
                if (Vector3.Distance(prevPos, transform.position) < grid.distBetweenCells)
                {
                    transform.position += new Vector3(Mathf.Cos(currRotation), Mathf.Sin(currRotation), 0) * movementSpeed * Time.deltaTime;
                }

                else
                {
                    isMoving = false;
                    currRotation += Mathf.PI;
                    currRotation %= 2 * Mathf.PI;
                    Debug.Log("change de dir");
                    prevPos = transform.position;

                    ForcePositionOntoGrid();
                }

            }
        }
    }
}