using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapUIScript : MonoBehaviour
{
    [SerializeField]
    Transform Content;
    [SerializeField]
    GameObject MapIconRes;
    [SerializeField]
    GameObject PathDotRes;
    [SerializeField]
    GameObject CancelButton;
    Vector3 CancelButtonOriginPos;

    Dictionary<int, List<GameObject>> MapIcons;

    bool IsEnableMap;
    void Awake()
    {
        Content.localPosition = new Vector3(0, -1080, 0);
        IsEnableMap = false;
        MapIcons = new Dictionary<int, List<GameObject>>();
        CancelButtonOriginPos = CancelButton.transform.localPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StageProgressMapOpen()
    {
        IsEnableMap = true;
    }
    public void CancelButtonEvent(Delvoid del)
    {
        CancelButton.GetComponentInChildren<Button>().onClick.AddListener(() => { del(); });
    }
    //맵 그리는거 관련된 함수들

    public void DrawIcon(ref Dictionary<int,List<MapNode>> map)
    {
        for(int j=1;j<16;++j)
        {
            List<GameObject> tmp = new List<GameObject>();
            for(int i = 0; i < map[j].Count; ++i)
            {
                GameObject obj = Instantiate(MapIconRes);
                obj.transform.SetParent(Content);
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = map[j][i].GetPos();
                obj.name = "(" + map[j][i].GetGridPos().x.ToString() + "," + map[j][i].GetGridPos().y.ToString() + ")";
                obj.GetComponent<MapIconScript>().SetNodeType(map[j][i].GetNodeType());
                tmp.Add(obj);
            }
            MapIcons.Add(j, tmp);
        }
    }
    public void DrawPath(ref Dictionary<int, List<MapNode>> map)
    {
        for (int h=1;h<15;++h)
        {
            
            for (int i = 0; i < map[h].Count; ++i)
            {
                List<int> tmp = map[h][i].GetUpPath();
                int floor = map[h][0].GetFloor();

                if (tmp.Count > 0)
                {
                    for (int j = 0; j < tmp.Count; ++j)
                    {
                        float dist = Vector2.Distance(map[h][i].GetPos(), map[floor+1][tmp[j]].GetPos());

                        int count = (int)(dist / 30) + 1;
                        for (int k = 0; k < count; ++k)
                        {
                            Vector2 TmpPos = Vector2.Lerp(map[h][i].GetPos(), map[floor + 1][tmp[j]].GetPos(),(float)(k +1)/ (float)count);
                            Quaternion TmpRot = Quaternion.FromToRotation(map[floor + 1][tmp[j]].GetPos(), map[h][i].GetPos());
                            GameObject obj = Instantiate(PathDotRes);
                            obj.transform.SetParent(Content);
                            obj.transform.localScale = Vector3.one;
                            obj.transform.localPosition = TmpPos;
                            obj.transform.localRotation = TmpRot;
                        }
                    }
                }
            }
        }
    }
    public void DrawBossIcon(MapNode node,BossType type, ref Dictionary<int, List<MapNode>> map)
    {
        GameObject tmp = Instantiate(MapIconRes);
        tmp.transform.SetParent(Content);
        tmp.transform.localScale = Vector3.one;
        tmp.transform.localPosition = node.GetPos();
        tmp.name = "Boss";
        tmp.GetComponent<MapIconScript>().SetBossType(type);

        List<GameObject> objTmpList = new List<GameObject>();
        objTmpList.Add(tmp);

        MapIcons.Add(16,objTmpList);
        for (int i = 0; i < map[15].Count; ++i)
        {
            List<int> tmpList = map[15][i].GetUpPath();

            if (tmpList.Count > 0)
            {
                for (int j = 0; j < tmpList.Count; ++j)
                {
                    float dist = Vector2.Distance(map[15][i].GetPos(), node.GetPos());

                    int count = (int)(dist / 30) + 1;
                    for (int k = 0; k < count; ++k)
                    {
                        Vector2 TmpPos = Vector2.Lerp(map[15][i].GetPos(), node.GetPos(), (float)(k + 1) / (float)count);
                        Quaternion TmpRot = Quaternion.FromToRotation(node.GetPos(), map[15][i].GetPos());
                        GameObject obj = Instantiate(PathDotRes);
                        obj.transform.SetParent(Content);
                        obj.transform.localScale = Vector3.one;
                        obj.transform.localPosition = TmpPos;
                        obj.transform.localRotation = TmpRot;
                    }
                }
            }
        }

    }
    
    //맵 켜고 끄는거 관련된 함수들


    public void HideMap()
    {
        StopCoroutine(CancelButtonMove());
        IsEnableMap = false;
        CancelButton.transform.localPosition = CancelButtonOriginPos + new Vector3(-250.0f, 0.0f, 0.0f);
        this.gameObject.SetActive(false);
    }
    public void OpenMapInfoBar()
    {
        IsEnableMap = false;
        this.gameObject.SetActive(true);
        int floor = MainSceneController.Instance.PlayerData.CurrentFloor;
        Content.localPosition = new Vector3(0, 1080 - 100 * floor, 0);
        StartCoroutine(CancelButtonMove());
        AllButtonDisabled();
    }
    public void OpenMapProgress()
    {
        IsEnableMap = true;
        this.gameObject.SetActive(true);
        int floor = MainSceneController.Instance.PlayerData.CurrentFloor;
        Content.localPosition = new Vector3(0,1080-100* floor, 0);
        StartCoroutine(CancelButtonMove());
        ButtonRefresh();
    }
    public IEnumerator FirstMapOpen()
    {

        Vector3 target = new Vector3(0, 1080, 0);

        float speed = 0.0f;
        while (Vector3.Distance(Content.localPosition, target) > 10.0f)
        {
            Content.localPosition = Vector3.MoveTowards(Content.localPosition, target, speed);
            yield return null;
            speed += Time.deltaTime;
        }
        Content.localPosition = target;
    }

    IEnumerator CancelButtonMove()
    {
        Vector3 target = CancelButtonOriginPos;

        while (Vector3.Distance(CancelButton.transform.localPosition, target) > 1.0f)
        {
            CancelButton.transform.localPosition = Vector3.MoveTowards(CancelButton.transform.localPosition, target, 30.0f);
            yield return null;
        }
        CancelButton.transform.localPosition = target;
    }



    public void FloorProgress(int floor,int floorIndex)
    {
        List<int> NextPath = MainSceneController.Instance.GetMapControl().FindCanMoveNode(floor,floorIndex);
        for (int i = 0; i < NextPath.Count; ++i) {
            int tmpInt = NextPath[i];
            MapIcons[floor + 1][NextPath[i]].GetComponent<MapIconScript>().ButtonOn(
                ()=> {
                    SaveStage(floor + 1, tmpInt);
                });
        }
    }
    public void SaveStage(int floor,int floorIndex)
    {
        MainSceneController.Instance.PlayerData.CurrentFloor = floor;
        MainSceneController.Instance.PlayerData.CurrentFloorIndex = floorIndex;

        MapNodeType type = MainSceneController.Instance.GetMapControl().GetMapNodeType(floor, floorIndex);
        MapIcons[floor][floorIndex].GetComponent<MapIconScript>().Check();
        MainSceneController.Instance.EventStateChange(type);
    }
    public MapNodeType ClearStage(int floor, int floorIndex)
    {
        MainSceneController.Instance.PlayerData.CurrentFloor = floor;
        MainSceneController.Instance.PlayerData.CurrentFloorIndex = floorIndex;

        MapNodeType type = MainSceneController.Instance.GetMapControl().GetMapNodeType(floor, floorIndex);
        MapIcons[floor][floorIndex].GetComponent<MapIconScript>().Check();
        return type;
    }
    void AllButtonDisabled()
    {
        foreach (var row in MapIcons)
        {
            for (int i = 0; i < row.Value.Count; ++i)
            {
                row.Value[i].GetComponent<MapIconScript>().ButtonOff();
            }
        }
    }
    public void ButtonRefresh()
    {
        foreach(var row in MapIcons)
        {
            for(int i = 0; i < row.Value.Count; ++i)
            {
                row.Value[i].GetComponent<MapIconScript>().ButtonOff();
            }
        }
        FloorProgress(MainSceneController.Instance.PlayerData.CurrentFloor, MainSceneController.Instance.PlayerData.CurrentFloorIndex);
    }
  
}
