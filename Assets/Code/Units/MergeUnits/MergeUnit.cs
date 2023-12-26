using System;
using UnityEngine;

public class MergeUnit : MonoBehaviour
{
    public int Rank
    {
        get => _rank;

        set
        {
            if (value > 0)
                _rank = value;
        }
    }
    
    protected bool unitTouched;
    
    private int _rank;
    private float sensitivity = 0.04f;

    private bool _collisionWithUnit;
    
    private Vector3 lastMousePositionPC;
    private Vector3 lastMousePositionPhone;
    private Vector3 _startPos;

    private GameObject _mergedUnit;
    
    private RaycastHit _hit;
    private SkinnedMeshRenderer _skinnedMeshRenderer;

    protected void Start()
    {
        _startPos = transform.position;
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    protected virtual void Update()
    {
        if (Input.touchCount > 0)
            DragNDrop(Input.GetTouch(0).position, ref lastMousePositionPhone);
        else
            DragNDrop(Input.mousePosition, ref lastMousePositionPC);
    }

    protected void OnTriggerStay(Collider coll)
    {
        if (coll?.gameObject?.TryGetComponent(out MergeUnit mergeUnit) == true && unitTouched)
        {
            _collisionWithUnit = true;
            _mergedUnit = coll.gameObject;
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll?.gameObject?.TryGetComponent(out MergeUnit mergeUnit) == true && unitTouched)
        {
            _collisionWithUnit = false;
            _mergedUnit = null;
        }
    }

    private void DragNDrop(Vector3 inputPos, ref Vector3 lastPosition)
    {
        if (Input.GetMouseButtonDown(0) || TouchBegan())
        {
            Ray ray = Camera.main.ScreenPointToRay(inputPos);
            if (Physics.Raycast(ray, out _hit, 1000f) && _hit.collider.gameObject == gameObject)
            {
                HitObject(inputPos, ref lastPosition);
            }
        }

        if (unitTouched)
        {
            MoveUnit(inputPos, ref lastPosition);
        }

        if ((Input.GetMouseButtonUp(0) || TouchEnded()) && unitTouched)
        {
            if (_collisionWithUnit && Merge.instance.Connect(gameObject, _mergedUnit))
            {
                
            }
            else
            {
                StopTouchObject();
            }
        }
    }


    private void SetColor(Color color)
    {
        _skinnedMeshRenderer.materials[0].color = color;
    }
    
    private void MoveUnit(Vector3 mousePos, ref Vector3 lastPosition)
    {
        Vector3 delta = (mousePos - lastPosition) * sensitivity;
            
        transform.position += new Vector3(delta.x, 0f, delta.y);

        lastPosition = mousePos;
    }

    private void HitObject(Vector3 mousePos, ref Vector3 lastPosition)
    {
        unitTouched = true;
        lastPosition = mousePos;
        SetColor(Color.red);
    }

    private void StopTouchObject()
    {
        unitTouched = false;
        SetColor(Color.white);
        gameObject.transform.position = _startPos;
    }

    private bool TouchBegan()
    {
        return (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
    }

    private bool TouchEnded()
    {
        return (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended);
    }
}
