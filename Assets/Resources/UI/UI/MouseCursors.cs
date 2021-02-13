using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursors : MonoBehaviour
{
    public Texture2D NormalCursor;
    public Texture2D MapCursor;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(NormalCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
