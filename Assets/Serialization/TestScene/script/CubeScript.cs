using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasySave;
using UnityEngine.EventSystems;
public class CubeScript : MonoBehaviour,IDragHandler,IEndDragHandler
{

    [Savable("V3Serializer")] private Vector3 m_position;


    private void Awake()
    {
        m_position = transform.position;

    }

    public void SetPosition()
    {
        transform.position = m_position;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        m_position = transform.position;
    }


    public void OnDrag(PointerEventData eventData)
    {
        transform.position = new Vector3(EasySaveManager.MousePos.x, 0.5f, EasySaveManager.MousePos.z);
    }
}
