using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanelScript : MonoBehaviour
{
    public GameObject CancelButton;
    public GameObject StartButton;
    public Image Background;
    public Sprite[] BackgroundSpirtes;
    public GameObject CharDescriptionPanel;

    public CharacterType m_CurrentChar = CharacterType.None;

    SEManager m_SEManager;

    // Start is called before the first frame update

    private void Awake()
    {
        m_SEManager = GameObject.Find("SEManager").GetComponent<SEManager>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitMenu()
    {
        StartCoroutine(MoveButton(CancelButton,false));
    }

    IEnumerator MoveButton(GameObject obj,bool IsLeftMoving)
    {
        Vector3 target;
        if (IsLeftMoving)
        {
            target = obj.transform.localPosition - new Vector3(obj.GetComponent<RectTransform>().rect.width, 0.0f, 0.0f);
        }
        else
        {
            target = obj.transform.localPosition + new Vector3(obj.GetComponent<RectTransform>().rect.width, 0.0f, 0.0f);
        }

        while (Vector3.Distance(obj.transform.localPosition, target) >= 1.0f)
        {
            obj.transform.localPosition = Vector3.MoveTowards(obj.transform.localPosition, target, 20.0f);
            yield return null;
        }
        obj.transform.localPosition = target;
    }

    public void PushCacnelButton()
    {
        Destroy(this.gameObject);
    }
    public void PushStartButton()
    {
        TitleSceneController.Instance.SceneChange();
    }

    public void CharTypeToBackground(CharacterType Type,Delvoid Del)
    {
        if (m_CurrentChar == Type) return;
        if (m_CurrentChar == CharacterType.None)
        {
            StartCoroutine(MoveButton(StartButton, true));
        }
        m_CurrentChar = Type;
        Camera.main.GetComponent<CameraController>().CameraShakeFunc(0.1f, 0.2f);
        switch (m_CurrentChar)
        {
            case CharacterType.None:
                break;
            case CharacterType.Ironclad:
                Background.sprite = BackgroundSpirtes[0];
                CharDB.Instance.SetPlayChar(m_CurrentChar);
                CharDescriptionPanel.GetComponent<CharDescriptionPanel>().PanelRefresh(m_CurrentChar);
                Del();
                break;
            case CharacterType.Silent:
                CharDB.Instance.SetPlayChar(m_CurrentChar);
                break;
            case CharacterType.Defect:
                CharDB.Instance.SetPlayChar(m_CurrentChar);
                break;
            case CharacterType.Watcher:
                CharDB.Instance.SetPlayChar(m_CurrentChar);
                break;
        }
    }
}
