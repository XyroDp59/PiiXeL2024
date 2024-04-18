using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
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

    public PlayerHealth Health;

    private int currentLevel = 0;
    [SerializeField] private int defaultMaxExp;
    private int currentMaxExp;
    private int currentExp;

    private Grid currentGrid;
    private List<GridPlayer> gridPlayersList;

    

    private void choseNextMove() {
        
    }

    public void gainExp(int exp) {
        currentExp += exp;
        if(currentExp > currentMaxExp)
        {
            currentLevel += 1;
            currentMaxExp = defaultMaxExp * currentLevel;
            currentExp = 0;
            BuyNewSkillSet();
        }
    }



    private void BuyNewSkillSet() { 
        //TODO
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
