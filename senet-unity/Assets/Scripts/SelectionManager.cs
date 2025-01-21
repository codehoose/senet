using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject selectionCursor;
    [SerializeField] private bool selectionAllowed;
    [SerializeField] private AnimationCurve scaleCurve;

    private float scaleTime;

    private enum SelectionState
    {
        None,
        Hover,
        Selected
    }

    private SelectionState state;

    void Start()
    {
        state = SelectionState.None;
    }


    // Update is called once per frame
    void Update()
    {
        if (state == SelectionState.Selected)
        {
            var scale = scaleCurve.Evaluate(scaleTime);
            selectionCursor.transform.localScale = new Vector3(scale, 1f, scale);
            scaleTime += Time.deltaTime / 0.5f;
            if (scaleTime > 1) scaleTime -= 1f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            selectionCursor.transform.localScale = new Vector3(1, 1, 1);
            state = SelectionState.None;
        }

        if (!selectionAllowed || state == SelectionState.Selected) return;

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

                if (Input.GetMouseButtonDown(0))
                {
                    state = SelectionState.Selected;
                }
            }
        }

        selectionCursor.SetActive(selectionCursorIsVisible);
    }
}
