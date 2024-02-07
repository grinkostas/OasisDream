using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UIScroller : MonoBehaviour
{
    [SerializeField] private float _sensitivity;
    private EventSystem EventSystem => EventSystem.current;
    private PointerEventData _pointerEventData;
    
    private bool _selected = false;
    
    protected abstract GameObject Target { get; }
    
    private void Update()
    {
        Select();
        Deselect(); 
    }

    private void Select()
    {
        if (Input.GetMouseButtonDown(0) == false || _selected)
            return;
        
        if (GetRaycasts().Has(x => x.gameObject == Target))
            _selected = true; 
    }
    
    private List<RaycastResult> GetRaycasts()
    {
        _pointerEventData = new PointerEventData(EventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.RaycastAll(_pointerEventData, results);
        return results;
    }


    private void Deselect()
    {
        if (Input.GetMouseButtonUp(0) && _selected)
            _selected = false;
    }
    
    private void OnGUI()
    {
        Event moveEvent = Event.current;
        
        if(NeedToUpdateUI() == false)
            return;
        
        Move(moveEvent.delta);
    }

    private bool NeedToUpdateUI()
    {
        if(_selected == false)
            return false;
        
        Event moveEvent = Event.current;
        if (moveEvent.isMouse == false)
            return false;
        
        if(moveEvent.delta == Vector2.zero)
            return false;
        
        return NeedToUpdateGUIExternal();
    }
    
    
    private void Move(Vector2 delta)
    {
        Vector2 normalizedVector = new Vector2(delta.x / Screen.width, delta.y / Screen.height);
        OnHorizontalScroll(_sensitivity * normalizedVector.x);
        OnVerticalScroll(_sensitivity * normalizedVector.y);
    }

    protected virtual bool NeedToUpdateGUIExternal()
    {
        return true;
        
    }
    protected virtual void OnHorizontalScroll(float delta){}
    protected virtual void OnVerticalScroll(float delta){}
}
