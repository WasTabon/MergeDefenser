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

    public void SetTile(Transform tile)
    {
        _myTile = tile;
    }

    public Transform GetTile()
    {
        return _myTile;
    }
    
    public bool UnitTouched { get; private set; }
    
    private int _rank;
    private const float Sensitivity = 0.04f;

    private bool _collisionWithUnit;

    public event Action mainAbility;
    public event Action idle;
    public event Action merged;

    private Transform _myTile;
    
    private Vector3 _lastMousePosition;
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
            DragNDrop(Input.GetTouch(0).position, ref _lastMousePosition);
        else
            DragNDrop(Input.mousePosition, ref _lastMousePosition);
    }

    protected void OnTriggerStay(Collider coll)
    {
        if (TriggeredMergedUnit(coll) && UnitTouched)
        {
            _collisionWithUnit = true;
            _mergedUnit = coll.gameObject;
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (TriggeredMergedUnit(coll) && UnitTouched)
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

        if (UnitTouched)
        {
            MoveUnit(inputPos, ref lastPosition);
        }

        if ((Input.GetMouseButtonUp(0) || TouchEnded()) && UnitTouched)
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

    protected virtual void MainAbility()
    {
        mainAbility?.Invoke();
    }

    protected virtual void Idle()
    {
        idle?.Invoke();
    }

    protected virtual void Merged()
    {
        merged?.Invoke();
    }

    private void SetColor(Color color)
    {
        _skinnedMeshRenderer.materials[0].color = color;
    }

    private void MoveUnit(Vector3 mousePos, ref Vector3 lastPosition)
    {
        Vector3 delta = (mousePos - lastPosition) * Sensitivity;
            
        transform.position += new Vector3(delta.x, 0f, delta.y);

        lastPosition = mousePos;
    }

    private void HitObject(Vector3 mousePos, ref Vector3 lastPosition)
    {
        UnitTouched = true;
        lastPosition = mousePos;
        SetColor(Color.red);
    }

    private void StopTouchObject()
    {
        UnitTouched = false;
        SetColor(Color.white);
        gameObject.transform.position = _startPos;
    }

    private bool TriggeredMergedUnit(Collider coll)
    {
        return coll.gameObject.TryGetComponent(out MergeUnit _);
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
