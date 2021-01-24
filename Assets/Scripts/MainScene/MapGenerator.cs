using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum MapNodeType
{
    None, Monster, Merchant, Rest, Elite, Boss, Mistery,Treasure
}
public class MapNode
{
    Vector2 Pos;
    Vector2Int GridPos;
    MapNodeType nodeType;
    int Floor;
    int FloorIndex;
    List<int> UpPath;
    List<int> DownPath;


    public MapNode(Vector2 pos, Vector2Int gridPos, MapNodeType type, int floor,int floorIndex)
    {
        Pos = pos;
        nodeType = type;
        GridPos = gridPos;
        Floor = floor;
        FloorIndex = floorIndex;
        UpPath = new List<int>();
        DownPath = new List<int>();
    }

    public Vector2 GetPos()
    {
        return Pos;
    }

    public Vector2Int GetGridPos()
    {
        return GridPos;
    }

    public int GetFloor()
    {
        return Floor;
    }

    public int GetFloorIndex()
    {
        return FloorIndex;
    }
    public void AddUpPath(int node)
    {
        UpPath.Add(node);
    }
    public void AddDownPath(int node)
    {
        DownPath.Add(node);
    }
    public List<int> GetUpPath()
    {
        return UpPath;
    }
    public List<int> GetDownPath()
    {
        return DownPath;
    }
    public MapNodeType GetNodeType()
    {
        return nodeType;
    }
    public void SetNodeType(MapNodeType type)
    {
        nodeType = type;
    }
}
public class MapGenerator : MonoBehaviour
{
    [Header("Map Generator Attribute")]
    [SerializeField]
    int EventMarkSpawnRate = 20;
    [SerializeField]
    int ShopMarkSpawnRate = 30;
    [SerializeField]
    int ShopMarkSpawnTerm = 2;
    [SerializeField]
    int ShopMarkSpawnMinHeight = 3;
    [SerializeField]
    int ShopMarkSpawnMaxHeight = 13;
    [SerializeField]
    int RestMarkSpawnTerm = 2;
    [SerializeField]
    int RestMarkSpawnRate = 30;
    [SerializeField]
    int RestMarkSpawnMinHeight = 4;
    [SerializeField]
    int RestMarkSpawnMaxHeight = 13;
    [SerializeField]
    int EliteMarkSpawnTerm = 2;
    [SerializeField]
    int EliteMarkSpawnMinHeight = 5;
    [SerializeField]
    int EliteMarkSpawnMaxHeight = 13;


    [SerializeField]
    int MaxHeight;
    [SerializeField]
    int MaxWidth;
    [SerializeField]
    int FloorCount = 0;

    [SerializeField]
    int StartPointNodeCount;
    [SerializeField]
    int PreBossRoomNodeCount;

    [SerializeField]
    bool[,] BaseMap;

    [SerializeField]
    float MapHeight;
    [SerializeField]
    float MapWidth;

    [SerializeField]
    float HeightDelta;
    [SerializeField]
    float WidthDelta;

    Dictionary<int, List<MapNode>> Map;
    List<MapNode> PastPath;
    BossType Boss;
    MapNode StartNode;
    MapNode BossNode;
    int NodeSelectCount;

    

    private void Awake()
    {
        
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MapGenerate()
    {
        MaxHeight = 15;
        MaxWidth = 7;
        BaseMapInit();
        for (int i = 0; i < MaxHeight; ++i)
        {
            BaseMapGenerator();
        }

        MapHeight = 2500;
        MapWidth = 1100;

        HeightDelta = MapHeight / MaxHeight;
        WidthDelta = MapWidth / MaxWidth;

        Map = new Dictionary<int, List<MapNode>>();
        PastPath = new List<MapNode>();


        NodeGenerator();
        PathConnect();
        MakeTreasureNode();
        NodeTypeSelector();
        MakeStartNode();
        MakeBossNode();
    }
    public void DrawMap(GameObject obj)
    {
        obj.GetComponent<MapUIScript>().DrawPath(ref Map);
        obj.GetComponent<MapUIScript>().DrawIcon(ref Map);
        obj.GetComponent<MapUIScript>().DrawBossIcon(BossNode, Boss, ref Map);
    }

    void BaseMapInit()
    {
        BaseMap = new bool[MaxHeight, MaxWidth];
        for (int i = 0; i < MaxHeight; ++i)
        {
            for(int j = 0; j < MaxWidth; ++j)
            {
                BaseMap[i, j] = false;
            }
        }
    }
    void BaseMapGenerator()
    {
        StartPointNodeCount = MapRandom();
        int mid = MaxWidth / 2;
        switch (FloorCount)
        {
            case 0:
                {
                    for (int i = 0; i < StartPointNodeCount / 2; ++i)
                    {
                        int rand;
                        do
                        {
                            rand = Random.Range(0, mid);
                        } while (BaseMap[FloorCount, rand]);
                        BaseMap[FloorCount, rand] = true;
                    }
                    for (int i = StartPointNodeCount / 2; i < StartPointNodeCount; ++i)
                    {
                        int rand;
                        do
                        {
                            rand = Random.Range(mid, MaxWidth);
                        } while (BaseMap[FloorCount, rand]);
                        BaseMap[FloorCount, rand] = true;
                    }
                }
                break;
            default:
                {
                    
                    BaseNodeSelector(mid);
                }
                break;
        }

        if (FloorCount < MaxHeight)
        {
            FloorCount++;
        }
    }
    void BaseNodeSelector(int middle)
    {
        int NodeCount = MapRandom();
        int CreateNode = 1;

        List<int> RandomNode = new List<int>();

        while (CreateNode < NodeCount)
        {
            for (int i = 0; i < MaxWidth; ++i)
            {
                if (BaseMap[FloorCount - 1, i])
                {
                    if (i == 0)
                    {
                        if (!RandomNode.Contains(i))
                        {
                            RandomNode.Add(i);
                        }
                        if (!RandomNode.Contains(i+1))
                        {
                            RandomNode.Add(i + 1);
                        }
                    }
                    else if (i == MaxWidth-1)
                    {
                        if (!RandomNode.Contains(i))
                        {
                            RandomNode.Add(i);
                        }
                        if (!RandomNode.Contains(i - 1))
                        {
                            RandomNode.Add(i - 1);
                        }
                    }
                    else
                    {
                        if (!RandomNode.Contains(i - 1))
                        {
                            RandomNode.Add(i - 1);
                        }
                        if (!RandomNode.Contains(i))
                        {
                            RandomNode.Add(i);
                        }
                        if (!RandomNode.Contains(i + 1))
                        {
                            RandomNode.Add(i + 1);
                        }
                    }

                }
            }
            while (RandomNode.Count< NodeCount)
            {
                int rand = Random.Range(0, MaxWidth);
                if (!RandomNode.Contains(rand))
                {
                    RandomNode.Add(rand);
                }
            }
            if (RandomNode.Count >= NodeCount)
            {
                int rand = 0;
                for (int i = 0; i < NodeCount; ++i)
                {
                    rand = Random.Range(0, RandomNode.Count);
                    BaseMap[FloorCount, RandomNode[rand]] = true;
                    RandomNode.Remove(RandomNode[rand]);
                    CreateNode++;
                }
            }

        }
        

    }
    void NodeGenerator()
    {
        
        for(int i = 0; i < MaxHeight; ++i)
        {
            List<MapNode> tmp = new List<MapNode>();
            for(int j = 0; j < MaxWidth; ++j)
            {
                float tmpx = Random.Range(-20, 20);
                float tmpy = Random.Range(-30, 30);
                Vector2 tmpVector = new Vector2(tmpx, tmpy);
                if (BaseMap[i, j])
                {
                    Vector2 nodepos = new Vector2(-(MapWidth/2)+(j*WidthDelta)+50, -(MapHeight/2)+(i*HeightDelta)-100)+ tmpVector;
                    Vector2Int grid = new Vector2Int(j, i+1) ;
                    MapNode tmpNode = new MapNode(nodepos, grid, MapNodeType.None, i + 1,tmp.Count);
                    tmp.Add(tmpNode);
                }
            }
            Map.Add(i + 1, tmp);
        }
    }

    void PathConnect()
    {
        for (int i = 1; i < Map.Count; ++i)
        {
            if (Map[i].Count == Map[i + 1].Count)
            {

                for (int j = 0; j < Map[i].Count; ++j)
                {
                    Map[i][j].AddUpPath(Map[i + 1][j].GetFloorIndex());
                    Map[i+1][j].AddDownPath(Map[i][j].GetFloorIndex());
                }

            }
            else if (Map[i].Count > Map[i + 1].Count)
            {

                for (int j = 0; j < Map[i].Count; ++j)
                {
                    int min = Mathf.Abs(Map[i + 1][0].GetGridPos().x - Map[i][j].GetGridPos().x);
                    int minNum = 0;
                    for (int k = 1; k < Map[i + 1].Count; ++k)
                    {
                        int tmp = Mathf.Abs(Map[i + 1][k].GetGridPos().x - Map[i][j].GetGridPos().x);
                        if (tmp < min)
                        {
                            min = tmp;
                            minNum = k;
                        }
                    }
                    Map[i][j].AddUpPath(Map[i + 1][minNum].GetFloorIndex());
                    Map[i+1][minNum].AddDownPath(Map[i][j].GetFloorIndex());
                }
                for(int j = 0; j < Map[i + 1].Count; ++j)
                {
                    if (Map[i + 1][j].GetDownPath().Count == 0)
                    {
                        int min = Mathf.Abs(Map[i + 1][j].GetGridPos().x - Map[i][0].GetGridPos().x);
                        int minNum = 0;
                        for(int k = 1; k < Map[i].Count; ++k)
                        {
                            int tmp = Mathf.Abs(Map[i + 1][j].GetGridPos().x - Map[i][k].GetGridPos().x);
                            if (tmp < min)
                            {
                                min = tmp;
                                minNum = k;
                            }
                        }
                        Map[i][minNum].AddUpPath(Map[i + 1][j].GetFloorIndex());
                        Map[i + 1][j].AddDownPath(Map[i][minNum].GetFloorIndex());
                    }
                }
            }
            else if (Map[i].Count < Map[i + 1].Count)
            {


                for (int j = 0; j < Map[i + 1].Count; ++j)
                {
                    int min = Mathf.Abs(Map[i][0].GetGridPos().x - Map[i + 1][j].GetGridPos().x);
                    int minNum = 0;
                    for (int k = 1; k < Map[i].Count; ++k)
                    {
                        int tmp = Mathf.Abs(Map[i][k].GetGridPos().x - Map[i + 1][j].GetGridPos().x);
                        if (tmp < min)
                        {
                            min = tmp;
                            minNum = k;

                        }
                    }
                    Map[i][minNum].AddUpPath(Map[i + 1][j].GetFloorIndex());
                    Map[i+1][j].AddDownPath(Map[i][minNum].GetFloorIndex());

                }
                for(int j = 0; j < Map[i].Count; ++j)
                {
                    
                    if (Map[i][j].GetUpPath().Count == 0)
                    {
                        int min = Mathf.Abs(Map[i + 1][0].GetGridPos().x - Map[i][j].GetGridPos().x);
                        int minNum = 0;

                        for (int k = 1; k < Map[i + 1].Count; ++k)
                        {
                            int tmp = Mathf.Abs(Map[i + 1][k].GetGridPos().x - Map[i][j].GetGridPos().x);
                            if (tmp < min)
                            {
                                min = tmp;
                                minNum = k;
                            }
                        }
                        Map[i][j].AddUpPath(Map[i + 1][minNum].GetFloorIndex());
                        Map[i + 1][minNum].AddDownPath(Map[i][j].GetFloorIndex());
                    }
                }
            }
        }
    }

    void NodeTypeSelector()
    {
        int rand;
        for (int i = 2; i < MaxHeight; ++i)
        {
            for (int j = 0; j < Map[i].Count; ++j)
            {
                rand = Random.Range(0, 100);
                if (rand <= ShopMarkSpawnRate)
                {
                        ShopStateSelector(Map[i][j]);
                }
                else if (rand > ShopMarkSpawnRate && rand <= (ShopMarkSpawnRate + EventMarkSpawnRate))
                {
                    if (Map[i][j].GetNodeType() == MapNodeType.None)
                    {
                        Map[i][j].SetNodeType(MapNodeType.Mistery);
                    }
                }
                else if (rand < (ShopMarkSpawnRate + EventMarkSpawnRate + RestMarkSpawnRate) && rand> (ShopMarkSpawnRate + EventMarkSpawnRate))
                {
                    RestStateSelector(Map[i][j]);
                }
                else
                {
                    EliteStateSelector(Map[i][j]);
                }
            }
        }
        for (int i = 1; i < MaxHeight; ++i)
        {
            for (int j = 0; j < Map[i].Count; ++j)
            {
                if (Map[i][j].GetNodeType() == MapNodeType.None)
                {
                    Map[i][j].SetNodeType(MapNodeType.Monster);
                }
            }
        }

        for (int j = 0; j < Map[MaxHeight].Count; ++j)
        {
            if (Map[MaxHeight][j].GetNodeType() == MapNodeType.None)
            {
                Map[MaxHeight][j].SetNodeType(MapNodeType.Rest);
            }
        }
    }
    bool RestStateSelector(MapNode node)
    {
        bool Condition = true;
        Condition &= node.GetNodeType() == MapNodeType.None;
        Condition &= node.GetFloor() >= RestMarkSpawnMinHeight;
        Condition &= SearchTypeInRange(node, MapNodeType.Rest, RestMarkSpawnTerm);
        Condition &= node.GetFloor() <= RestMarkSpawnMaxHeight;
        if (Condition)
        {
            node.SetNodeType(MapNodeType.Rest);
            return true;
        }
        return false;
    }

    bool EliteStateSelector(MapNode node)
    {
        bool Condition = true;
        Condition &= node.GetNodeType() == MapNodeType.None;
        Condition &= node.GetFloor() >= EliteMarkSpawnMinHeight;
        Condition &= SearchTypeInRange(node, MapNodeType.Elite, EliteMarkSpawnTerm);
        Condition &= node.GetFloor() <= EliteMarkSpawnMaxHeight;
        if (Condition)
        {
            node.SetNodeType(MapNodeType.Elite);
            return true;
        }
        return false;
    }

    bool ShopStateSelector(MapNode node)
    {
        bool Condition = true;
        Condition &= node.GetNodeType() == MapNodeType.None;
        Condition &= node.GetFloor() >= ShopMarkSpawnMinHeight;
        Condition &= SearchTypeInRange(node, MapNodeType.Merchant, EliteMarkSpawnTerm);
        Condition &= node.GetFloor() <= ShopMarkSpawnMaxHeight;
        if (Condition)
        {
            node.SetNodeType(MapNodeType.Merchant);
            return true;
        }
        return false;
    }

    bool SearchTypeInRange(MapNode node,MapNodeType type, int range)
    {
        if (range <= 0) return true;
        int Floor = node.GetFloor();
        List<int> tmp = node.GetDownPath();
        if (tmp.Count <= 0) return true;

        for(int i = 0; i < tmp.Count; ++i)
        {
            if (Map[Floor-1][tmp[i]].GetNodeType() == type) return false;
            if (SearchTypeInRange(Map[Floor-1][tmp[i]], type, range - 1) == false) return false;
        }

        return true;
    }

    int MapRandom()
    {
        int rand = Random.Range(0, 100);
        if (rand < 20)
        {
            return 3;
        }
        else if (rand >= 20 && rand < 80)
        {
            return 4;
        }
        else
        {
            return 5;
        }

        
    }
    void MakeStartNode()
    {
        StartNode = new MapNode(Vector3.one, Vector2Int.zero, MapNodeType.None, 0, 0);
        for (int i = 0; i < Map[1].Count; ++i) {
            StartNode.AddUpPath(Map[1][i].GetFloorIndex());
        }
        List<MapNode> tmp = new List<MapNode>();
        tmp.Add(StartNode);
        Map.Add(0, tmp);
    }
    void MakeTreasureNode()
    {
        for(int i = 0; i < Map[8].Count; ++i)
        {
            Map[8][i].SetNodeType(MapNodeType.Treasure);
        }
    }
    void MakeBossNode()
    {
        Boss = (BossType)Random.Range(0, 3);
        BossNode = new MapNode(new Vector2(0,MapHeight/2), Vector2Int.zero, MapNodeType.Boss, MaxHeight + 1, 0);
        for(int i = 0; i < Map[MaxHeight].Count; ++i) {
            Map[MaxHeight][i].AddUpPath(BossNode.GetFloorIndex());
        }
        List<MapNode> tmp = new List<MapNode>();
        tmp.Add(BossNode);
        Map.Add(MaxHeight + 1, tmp);
    }
    public List<int> FindCanMoveNode(int floor,int floorIndex)
    {
        return Map[floor][floorIndex].GetUpPath();
    }
    public MapNodeType GetMapNodeType(int floor, int floorIndex)
    {
        return Map[floor][floorIndex].GetNodeType();
    }
}
