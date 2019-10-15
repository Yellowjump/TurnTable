using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct rewardItemDate{
    public int type;
    public int value;
    public rewardItemDate(int t,int v){
        this.type=t;
        this.value=v;
    }
}

public class Wheel : MonoBehaviour
{
    Dictionary<int ,Color> typeToColor=new Dictionary<int, Color>{
        {1,Color.red},
        {2,Color.yellow}
    };
    List<rewardItemDate> itemsDate;
    bool canWhirl;
    public GameObject wheelRewardItemPrefab;
    public Transform itemParent;
    public AnimationCurve wheelSpeed;//控制旋转速度的曲线，设置3个区间，分别是起步，匀速，停止。
    void Start()
    {
        canWhirl=true;
        WheelInit();
    }
    void WheelInit(){
        var tempItemsDate=new List<rewardItemDate>{
            new rewardItemDate(1,1000),
            new rewardItemDate(1,2000),
            new rewardItemDate(1,3000),
            new rewardItemDate(1,4000),
            new rewardItemDate(1,5000),
            new rewardItemDate(2,6),
            new rewardItemDate(2,7),
            new rewardItemDate(2,8),
            new rewardItemDate(2,9),
            new rewardItemDate(2,10)
        };
        itemsDate=tempItemsDate;
        for(var itemNum=0;itemNum< itemsDate.Count;itemNum++){
            var itemTra= Instantiate(wheelRewardItemPrefab,itemParent.position,Quaternion.Euler(0,0,-36*itemNum),itemParent);
            ItemInit(itemTra,itemNum);
        }
    }
    public void ItemInit(GameObject item,int index){
        item.transform.Find("RewardImg").GetComponent<Image>().color=typeToColor[itemsDate[index].type];
        item.transform.Find("ValueText").GetComponent<Text>().text=itemsDate[index].value.ToString();
    }
    public void WhirlButton(){
        if(canWhirl){
            canWhirl=false;
            int result=Random.Range(0,10);//这里每一项概率平均，没有加权
            Debug.Log("wheelResult:"+result);
            
            StartCoroutine(Whirligig(result));
        }
    }
    IEnumerator Whirligig(int result){
        float time=0;
        //转个5圈加 (360-36*result)度
        int rotateAngle=1800+(360-result*36);//总要旋转的度数
        var wheelTrans=itemParent.parent;
        var tempZ=0f;//z值会%=360，所以需要一个值暂存
        wheelTrans.rotation=new Quaternion(0,0,0,0);//归零消除上次旋转的偏移角，当然也可以保留对下次进行，但懒得改
        while((time+=Time.deltaTime)<4f){
            tempZ=(-rotateAngle)*wheelSpeed.Evaluate(time/4f);
            wheelTrans.rotation=Quaternion.Euler(0,0,tempZ);
            yield return null;
        }
        wheelTrans.rotation=Quaternion.Euler(0,0,-rotateAngle);
        canWhirl=true;
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
