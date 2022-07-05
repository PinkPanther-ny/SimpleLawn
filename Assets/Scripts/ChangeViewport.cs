using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeViewport : MonoBehaviour
{
    private Rect[] _rects = {new Rect(0.44f, 0.03f, 0.12f, 0.22f), new Rect(0.295f, 0.11f, 0.41f, 0.83f) , new Rect(0, 0, 1, 1) };

    private int _index = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (_index < _rects.Length - 1)
            {
                _index++;
            }
            else
            {
                _index = 0;
            }
            gameObject.GetComponent<Camera>().rect = _rects[_index];
        }
    }
}
