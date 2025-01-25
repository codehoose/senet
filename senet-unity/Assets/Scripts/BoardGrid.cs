using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGrid : MonoBehaviour
{
    [SerializeField] private GameObject startBlock;
    [SerializeField] private GameObject endBlock;
    [SerializeField] private GameObject straightBlock;
    [SerializeField] private GameObject topLeftBlock;
    [SerializeField] private GameObject topRightBlock;
    [SerializeField] private GameObject bottomLeftBlock;
    [SerializeField] private GameObject bottomRightBlock;

    [SerializeField] private GameObject redPawn;
    [SerializeField] private GameObject bluePawn;

    [SerializeField] bool spawnPawns;

    private List<Transform> blocks;

    private float gridSize = 1.02f;

    public List<Transform> Blocks => blocks;

    IEnumerator Start()
    {
        blocks = new List<Transform>();

        float zDirection = 1f;
        float x = 0;
        float z = 0;
        bool isRed = true;

        for (int i = 0; i < 30; i++)
        {
            if (i > 0 && (i % 10) == 0)
            {
                x -= gridSize;
                zDirection *= -1;
            }
            else
            {
                z += zDirection * gridSize;
            }

            var go = GetPieceFromIndex(i);
            go.name = $"Square {i}";
            go.transform.SetParent(transform);
            go.transform.position = Vector3.zero;
            go.transform.localPosition = new Vector3(z, 0, x);
            blocks.Add(go.transform);
            if (spawnPawns && i < 10)
            {
                var name = $"{(isRed ? "Red" : "Blue")}";
                SpawnPawn(go.transform, isRed, name);
                isRed = !isRed;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void SpawnPawn(Transform t, bool isRed, string name)
    {
        var pawn = Instantiate(isRed ? redPawn : bluePawn, t);
        pawn.name = name;
        StartCoroutine(DropPawn(pawn, Vector3.up * 20f, Vector3.up * 0.02f));
    }

    IEnumerator DropPawn(GameObject go, Vector3 start, Vector3 end)
    {
        float time = 0f;
        while (time < 1f)
        {
            var pos = Vector3.Lerp(start, end, time);
            go.transform.localPosition = pos;
            time += Time.deltaTime;
            yield return null;
        }

        go.transform.localPosition = end;
    }

    private GameObject GetPieceFromIndex(int index)
    {
        switch (index)
        {
            case 0: return Instantiate(startBlock);
            case 9: return Instantiate(topRightBlock);
            case 10: return Instantiate(bottomRightBlock);
            case 19: return Instantiate(topLeftBlock);
            case 20: return Instantiate(bottomLeftBlock);
            case 29: return Instantiate(endBlock);
            default: return Instantiate(straightBlock);
        }
    }
}
