using System.Collections;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SelectionManager selectionManager;
    [SerializeField] private BoardGrid grid;
    [SerializeField] private AnimationCurve bounceAnimation;
    [SerializeField] private DiceManager diceManager;

    private Player currentPlayer;
    private GameState gameState;
    private int dancerMoveCount;
    private Transform currentBlock;
    private float dancerHeightOffGround = 0.02f;

    IEnumerator Start()
    {
        gameState = GameState.FirstMove;
        currentPlayer = Player.Blue;
        dancerMoveCount = 4;
        selectionManager.IsSelectionValid = IsSelectionValid;

        while (!grid.Ready)
        {
            yield return null;
        }

        diceManager.ShowRollButton();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && currentBlock != null)
        //{
        //    StartCoroutine(MoveDancer());
        //}
    }

    private bool IsSelectionValid(Transform t)
    {
        if (gameState == GameState.FirstMove && t.name != "Square 9") return false;

        var found = t.Find(currentPlayer.ToString()) != null;
        currentBlock = found ? t : null;
        return found;
    }

    private IEnumerator MoveDancer()
    {
        var index = int.Parse(currentBlock.name.Substring("Square ".Length));
        var dancer = currentBlock.Find(currentPlayer.ToString());
        var startBlock = currentBlock;
        
        for (int i = 1; i <= dancerMoveCount; i++)
        {
            var newSquareName = $"Square {index + i}";
            var targetBlock = grid.Blocks.FirstOrDefault(block => block.name == newSquareName);

            var startPos = dancer.transform.position;
            var targetPos = targetBlock.position + new Vector3(0, dancerHeightOffGround, 0);

            var time = 0f;
            while (time < 1f)
            {
                var pos = Vector3.Lerp(startPos, targetPos, time);
                dancer.transform.position = pos + Vector3.up * bounceAnimation.Evaluate(time);
                time += Time.deltaTime / 0.25f;
                yield return null;
            }

            dancer.transform.position = targetBlock.position + new Vector3(0, dancerHeightOffGround, 0);
            dancer.transform.SetParent(targetBlock);
            startBlock = targetBlock;
        }
        selectionManager.ClearSelection();
    }
}
