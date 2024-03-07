using Script.Interface;
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
            Vector3 cellPosition = new Vector3(
                Mathf.Round((position.x - gridPosition.x) / grid.cellSize) * grid.cellSize + gridPosition.x,
                Mathf.Round((position.y - gridPosition.y) / grid.cellSize) * grid.cellSize + gridPosition.y,
                position.z
            );
            position = cellPosition;
            transform.position = position;
        }

        public void ToggleVisibility()
        {
            isVisible = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("Portal")) != null;
            _spriteRenderer.enabled = isVisible;
            _collider2D.enabled = isVisible;
        }
    }
}