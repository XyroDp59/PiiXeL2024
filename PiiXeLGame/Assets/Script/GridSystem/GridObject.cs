using Script.Interface;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Script.GridSystem
{
    public class GridObject : MonoBehaviour, IHoverable, IClickable, IRightClickable, IContactable
    {
        [SerializeField] protected bool isVisible;
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
                    /* marche 95% du temps ? */
                    float theta;
                    if (position.x == cellPosition.x) theta = Mathf.Sign(cellPosition.y - position.y) * Mathf.PI / 2f;
                    else theta = Mathf.Atan((cellPosition.y - position.y) / (cellPosition.x - position.x));
                    if (position.x - cellPosition.x < 0) theta += Mathf.PI;
                    theta += 2 * Mathf.PI;
                    theta -= theta % (Mathf.PI/3f) - Mathf.PI/6f;
                    cellPosition += grid.cellSize * sqrt3/3f * new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
                }
            }
            if (Vector2.Distance(cellPosition, position) < 0.1) return;
            position = cellPosition;
            transform.position = position;
        }

        public void ToggleVisibility()
        {
            //isVisible = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("Portal")) != null;
            isVisible = Player.Singleton.currentGrid == grid;
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