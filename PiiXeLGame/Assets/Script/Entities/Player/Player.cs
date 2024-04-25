using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IHealthSystem
{

    /** TODO
- exp : int 
- currentLevel : int
- currentGrid : Grid 
- spawnpoint : Vector2
- gridPlayerList : List(GridPlayer)
+ changeToGridPlayer(GridPlayer) : void
' update la position des GridPlayer invisible vers celle du GridPlayer visible, 
' en "arrondissant" les coord de ceux-ci de sorte à ce qu'ils soient toujours dans leur grille.
' ensuite, elle rend invisible tout les GridPlayer sauf celui passé en paramètre
+ choseNextMove() : void 
' recuperes le moveset du GridPlayer visible et permet de choisir l'action du tour en cours, et appelle previewActionInMoveset du GridPlayer
+ gameOver() : void
+ gainExp() : void
+ buyNewSkillSet() : void
' appelée par gainExp si le joueur lvl up
+ interact() : void
' méthode triviale qui va exécuter (si possible) la méthode interact du IPlayerInteraction
' de l'objet en face
     * 
     * 
     */

    [SerializeField] private int maxHealth = 100;
    private int _health;

    private int currentLevel = 0;
    [SerializeField] private int defaultMaxExp = 100;
    private int currentMaxExp;
    private int currentExp;

    public Script.GridSystem.GameGrid currentGrid;
    private List<GridPlayer> gridPlayersList;
    private GridPlayer currentGridPlayer;

    public static Player Singleton { get; private set; }

    private void Awake()
    {
        Singleton = this;
        _health = maxHealth;
    }

    private void choseNextMove() {
        float dirX = Input.GetAxisRaw("Horizontal");
        currentGridPlayer.moveIndex += (int)(dirX / Mathf.Abs(dirX));
        if (Input.GetButtonDown("Fire1")) { currentGridPlayer.doActionInMoveset(); }
    }

    public void gainExp(int exp) {
        currentExp += exp;
        if(currentExp > currentMaxExp)
        {
            currentLevel += 1;
            currentMaxExp = (defaultMaxExp * currentLevel);  
            currentExp = 0;
            BuyNewSkillSet();
        }
    }


    private void BuyNewSkillSet() { 
        //TODO
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public void AddToHealth(int health)
    {
        _health += health;
        if (_health > maxHealth) { _health = maxHealth; }
        if (_health <= 0)
        {
            _health = 0;
            Die();
        }
    }
}
