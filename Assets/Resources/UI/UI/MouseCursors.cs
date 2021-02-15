using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursors : MonoBehaviour
{
    static MouseCursors _instance = null;
    public static MouseCursors Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MouseCursors>();
            }
            return _instance;
        }
    }
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
