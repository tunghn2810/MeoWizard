using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControlScript : MonoBehaviour
{
    private RectTransform _cursor;
    private int _cursorIndex = 1;
    private Vector2 _cursorPosOffset = new Vector3(-64, 0);
    private Vector2 _cursorStep = new Vector3(0, 48);

    private void Awake()
    {
        _cursor = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void MoveCursor(bool isMoveDown)
    {
        if (isMoveDown)
        {
            if (_cursorIndex < transform.childCount - 1)
            {
                _cursor.anchoredPosition -= _cursorStep;
                _cursorIndex += 1;
            }
        }
        else
        {
            if (_cursorIndex > 1)
            {
                _cursor.anchoredPosition += _cursorStep;
                _cursorIndex -= 1;
            }
        }
    }

    public void Submit()
    {
        transform.GetChild(_cursorIndex).GetComponent<Button>().Submit();
    }
}
