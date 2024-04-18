using Script.Interface;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Script.GridSystem
{
    public class GridObject : MonoBehaviour, IHoverable, IClickable, IRightClickable, IContactable
    {
        [SerializeField] private bool isVisible;
        [SerializeField] protected GameGrid grid;
        [SerializeField] private GameObject portal;
        [SerializeField] private bool blockCurrentCase;

        private SpriteRenderer _spriteRenderer;
        private IPlayerInteraction _playerInteraction;
        private Collider2D _collider2D;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _playerInteraction = GetComponent<IPlayerInteraction>();
            _collider2D = GetComponent<Collider2D>();
            ForcePositionOntoGrid();
        }

        public void OnHover()
        {
            _spriteRenderer.color = Color.yellow;
        }

        public void OnClick()
        {
            _playerInteraction?.Interact();
        }

        public void OnRightClick()
        {
            blockCurrentCase = !blockCurrentCase;
            _spriteRenderer.color = blockCurrentCase ? Color.red : Color.white;
        }

        public void OnContact()
        {
            if (CompareTag("Player"))
            {
                _spriteRenderer.color = Color.green;
            }
        }

        public GameGrid GetGrid()
        {
            return grid;
        }

        public void ForcePositionOntoGrid()
        {
            Vector3 gridPosition = grid.transform.position;
            Vector3 position = transform.position;
            float sqrt3 = Mathf.Sqrt(3f);
            Vector3 cellPosition = new Vector3();

            if (grid.gridType == GridType.Square)
            {
                cellPosition = new Vector3(
                    ((int)((position.x - gridPosition.x) / grid.cellSize) + 0.5f) * grid.cellSize + gridPosition.x,
                    ((int)((position.y - gridPosition.y) / grid.cellSize) + 0.5f) * grid.cellSize + gridPosition.y,
                    position.z
                );
            }
            else
            {
                cellPosition = new Vector3(
                                        ((int)((position.x - gridPosition.x) / (grid.cellSize * 1.5f) + 0.5f)) * grid.cellSize * 1.5f + gridPosition.x,
                                        (Mathf.Round((position.y - gridPosition.y) / (grid.cellSize * sqrt3))) * grid.cellSize * sqrt3 + gridPosition.y,
                                        position.z
                                    );
                if (cellPosition.x / (grid.cellSize * 1.5f) % 2 == 1f)
                {
                    cellPosition.y = ((int)((position.y - gridPosition.y) / (grid.cellSize * sqrt3)) + 0.5f) * grid.cellSize * sqrt3 + gridPosition.y;
                }

                if(grid.gridType==GridType.Triangle)
                {
                    /* TODO : Fix la grid triangle */
                    float theta = Mathf.Atan((cellPosition.y - position.y) / (cellPosition.x - position.x));
                    if (cellPosition.x - position.x < 0) theta += Mathf.PI/2;
                    theta -= theta % (Mathf.PI/3) - Mathf.PI/6;
                    Debug.Log(theta);
                    cellPosition += grid.cellSize * sqrt3/3 * new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
                }
            }
             
            position = cellPosition;
            transform.position = position;
        }

        public void ToggleVisibility()
        {
            isVisible = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("Portal")) != null;
            _spriteRenderer.enabled = isVisible;
            _collider2D.enabled = isVisible;
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                ForcePositionOntoGrid();
            }
        }
    }
}