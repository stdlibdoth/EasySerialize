using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasySave;
using System.IO;
using System.Threading.Tasks;

public class EasySaveManager : MonoBehaviour
{

    [SerializeField]private Camera m_cam;
    private static EasySaveManager m_singleton = null;
    private static IFormatter m_formatter;

    private static Vector3 m_mousePos;

    private void Awake()
    {
        if (m_singleton == null)
        {
            m_singleton = this;
            m_formatter = GetComponent<IFormatter>();
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    public static Vector3 MousePos
    {
        get
        {
            return m_mousePos;
        }
    }


    private void Update()
    {
        Ray ray = m_cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitinfo, 100f, LayerMask.GetMask("Floor")))
            m_mousePos = hitinfo.point;
    }
}
