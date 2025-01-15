using System.Collections;
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

    private float gridSize = 1.02f;

    IEnumerator Start()
    {
        float zDirection = 1f;
        float x = 0;
        float z = 0;

        for (int i = 0; i < 30; i++)
        {
            if (i > 0 && (i % 10) == 0)
            {
                x += gridSize;
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
            go.transform.localPosition = new Vector3(x, 0, z);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private GameObject GetPieceFromIndex(int index)
    {
        switch(index)
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
