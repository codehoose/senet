using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject selectionCursor;

    // Update is called once per frame
    void Update()
    {
        bool selectionCursorIsVisible = false;
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.tag == "PathBlock")
            {
                var transform = hit.transform.parent;
                selectionCursor.transform.localPosition = Vector3.zero;
                selectionCursor.transform.position = transform.position + Vector3.up * 0.2f;
                selectionCursor.transform.SetParent(transform);
                selectionCursorIsVisible = true;
            }
        }

        selectionCursor.SetActive(selectionCursorIsVisible);
    }
}
