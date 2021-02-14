using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text Text;


    TMP_TextInfo TextInfo;
    float Timer;

    bool IsReady = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsReady) Shake();
    }

    public void SetDescription(string Description)
    {
        Text.text = Description;
        Timer = 0;

        StartCoroutine(Excute());
    }

    IEnumerator Excute()
    {

        IsReady = true;

        while (Timer < 2.0f) {
            Timer += Time.deltaTime;

            yield return null;
        }

        Destroy(this.gameObject);
    }

    void Shake()
    {
        Text.ForceMeshUpdate();
        TextInfo = Text.textInfo;

        for (int i = 0; i < TextInfo.characterCount; ++i)
        {
            var charInfo = TextInfo.characterInfo[i];

            if (!charInfo.isVisible)
            {
                continue;
            }

            var verts = TextInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            int rand = Random.Range(0, 2);

            for (int j = 0; j < 4; ++j)
            {
                var origin = verts[charInfo.vertexIndex + j];
                if (rand == 0)
                {
                    verts[charInfo.vertexIndex + j] = origin + new Vector3(0, Mathf.Sin(Time.time) * 5);
                }
                else
                {
                    verts[charInfo.vertexIndex + j] = origin + new Vector3(Mathf.Cos(Time.time) * 5, 0);
                }
            }
        }
        for (int i = 0; i < TextInfo.meshInfo.Length; ++i)
        {
            var meshInfo = TextInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            Text.UpdateGeometry(meshInfo.mesh, i);
        }
    }

}
