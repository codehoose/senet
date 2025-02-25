using System;
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
        IsSelectionValid = IsSelectionValid ?? ((go) => SelectionMode.Valid);
        state = SelectionState.None;
    }

    public event EventHandler BlockSelected;
    public Func<Transform, SelectionMode> IsSelectionValid; 

    
    public void ClearSelection()
    {
        selectionCursor.transform.localScale = new Vector3(1, 1, 1);
        state = SelectionState.None;
    }

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
            ClearSelection();
        }

        if (!selectionAllowed || state == SelectionState.Selected) return;

        bool selectionCursorIsVisible = false;
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.tag == "PathBlock")
            {
                var selectionMode = IsSelectionValid(hit.transform.parent);
                if (selectionMode == SelectionMode.Invalid) return;

                var transform = hit.transform.parent;
                selectionCursor.transform.localPosition = Vector3.zero;
                selectionCursor.transform.position = transform.position + Vector3.up * 0.2f;
                selectionCursor.transform.SetParent(transform);
                selectionCursorIsVisible = true;

                if (selectionMode == SelectionMode.Valid && Input.GetMouseButtonDown(0))
                {
                    state = SelectionState.Selected;
                    BlockSelected?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        selectionCursor.SetActive(selectionCursorIsVisible);
    }
}
