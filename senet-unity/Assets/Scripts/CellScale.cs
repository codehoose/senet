using System.Collections;
using UnityEngine;

public class CellScale : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float duration = 1f;

    void Awake()
    {
        transform.localScale = Vector3.zero;
    }
    
    IEnumerator Start()
    {
        var time = 0f;
        while (time < 1f)
        {
            var scale = curve.Evaluate(time);
            transform.localScale = new Vector3(scale, scale, scale);
            time = time += Time.deltaTime / duration;
            yield return null;
        }

        transform.localScale = new Vector3(1, 1, 1);
    }
}
