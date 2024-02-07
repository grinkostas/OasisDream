using NaughtyAttributes;
using UnityEngine;


[ExecuteInEditMode]
public class MonoGrid : MonoBehaviour
{
    [SerializeField] private Vector2 _paddings;
    [SerializeField] private Vector2 _gridSize;
    [SerializeField] private Vector3 _startPostion;
    [Space]
    [SerializeField] private bool _invertColumnsAndRows;
    [SerializeField] private bool _local;

    private Vector2 _currentDelta;
    
    [Button()]
    public void Align()
    {
        ResetGrid();
        int i = 0;
        foreach (Transform child in transform)
        {
            SetPosition(child);
            i++;
            
            if (i < _gridSize.x)
            {
                AddColumn();
                continue;
            }

            i = 0;
            AddRow();
        }    
    }

    public void SetPosition(Transform child)
    {
        Vector3 targetPosition = new Vector3(_currentDelta.x, _startPostion.y, _currentDelta.y);
        if (_local)
            child.localPosition = targetPosition;
        else
            child.position = targetPosition;
    }

    public void ResetGrid()
    {
        _currentDelta = new Vector2(_startPostion.x, _startPostion.y);
    }
    
    public void AddRow()
    {
        _currentDelta = new Vector2( _startPostion.x, _currentDelta.y + _paddings.y);
        if (_invertColumnsAndRows)
            _currentDelta = new Vector2(_currentDelta.x + _paddings.x, _startPostion.y);
    }

    public void AddColumn()
    {
        _currentDelta =new Vector2(_currentDelta.x + _paddings.x , _currentDelta.y  );
        if (_invertColumnsAndRows)
            _currentDelta =new Vector2(_currentDelta.x, _currentDelta.y + _paddings.y);
    }

}
