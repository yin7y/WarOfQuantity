using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoxSelection : MonoBehaviour
{
    /// &lt;summary&gt;
    /// 这是一个自定义类&#xff0c;用来表示一个生效的选定框&#xff1b;创建它时会自动算出包含四角坐标的四项数据
    /// &lt;/summary&gt;
    public class Selector{
        public float Xmin;
        public float Xmax;
        public float Ymin;
        public float Ymax;

        //构造函数&#xff0c;在创建选定框时自动计算Xmin/Xmax/Ymin/Ymax
        public Selector(Vector3 start, Vector3 end){
            Xmin = Mathf.Min(start.x, end.x);
            Xmax = Mathf.Max(start.x, end.x);
            Ymin = Mathf.Min(start.y, end.y);
            Ymax = Mathf.Max(start.y, end.y);
        }
    }
     bool onDrawingRect, canAttack, isSelecting;//是否正在画框(即鼠标左键处于按住的状态)
     Vector3 startPoint;//框的起始点&#xff0c;即按下鼠标左键时指针的位置
     Vector3 currentPoint;//在拖移过程中&#xff0c;玩家鼠标指针所在的实时位置
     Vector3 endPoint;//框的终止点&#xff0c;即放开鼠标左键时指针的位置
     Selector selector;
    List<MainCity> myCityList = new List<MainCity>();

    void Update(){        
        //玩家按下鼠标左键&#xff0c;此时进入画框状态&#xff0c;并确定框的起始点
        if(Input.GetKeyDown(KeyCode.Mouse1)){
            DefaultMainCity("City");
            onDrawingRect = true;
            startPoint = Input.mousePosition;
            Debug.LogFormat("开始画框`，起点:{0}", startPoint);
            isSelecting = true;
        }
        //在鼠标左键未放开时&#xff0c;实时记录鼠标指针的位置
        if(onDrawingRect){
            currentPoint = Input.mousePosition;            
        }
        //玩家放开鼠标左键&#xff0c;说明框画完&#xff0c;确定框的终止点&#xff0c;退出画框状态
        if(Input.GetKeyUp(KeyCode.Mouse1) && isSelecting){
            endPoint = Input.mousePosition;
            onDrawingRect = false;
            Debug.LogFormat("画框结束，终点:{0}", endPoint);
            //当框画完时&#xff0c;创建一个生效的选定框selector
            selector = new Selector(startPoint, endPoint);
            //执行框选事件
            CheckSelection(selector, "City");
            isSelecting = false;
        }
        // 多選後攻擊
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            if(canAttack){
                Vector2 mousePosition = GetWorldMousePosition();
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if(hit.collider != null){
                    GameObject hitObject = hit.collider.gameObject;
                    // canAttack = true;
                    MainCity[] allMyCity = myCityList.ToArray();
                    foreach(MainCity city in allMyCity){
                        if(city.GetTeamID() != 0) break;
                        city.SoldierGenerator(city.GetNum() - 1, hitObject);
                    }
                }
                myCityList.Clear();
                canAttack = false;
                GetComponent<ObjectSelection>().selectedHint.SetActive(false);
            }            
            DefaultMainCity("City");
        }
    }
    void DefaultMainCity(string tag){
        GameObject[] Units = GameObject.FindGameObjectsWithTag(tag);
        foreach(GameObject Unit in Units){
            Unit.GetComponent<MainCity>().selectedHint.SetActive(false);
        }
    }

    //框选事件
    //按照选定框的范围&#xff0c;捕获标签为tag的所有物体&#xff0c;并打印这些物体的名字
    
    void CheckSelection(Selector selector, string tag){
        GameObject[] Units = GameObject.FindGameObjectsWithTag(tag);
        foreach(GameObject Unit in Units){
            Vector3 screenPos = Camera.main.WorldToScreenPoint(Unit.transform.position);
            MainCity selectCity = Unit.GetComponent<MainCity>();
            if(screenPos.x > selector.Xmin && screenPos.x < selector.Xmax && screenPos.y > selector.Ymin && screenPos.y < selector.Ymax){
                if(selectCity.GetTeamID() == 0){
                    myCityList.Add(selectCity); // 將選中的土地加入一個列表
                    selectCity.selectedHint.SetActive(true);
                    GetComponent<ObjectSelection>().selectedHint.SetActive(true);
                    canAttack = true;
                }
            }
        }
        AudioManager audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        audioManager.PlayAudio(0);
    }

    public Material GLRectMat;//绘图的材质&#xff0c;在Inspector中设置
    public Color GLRectColor;//矩形的内部颜色&#xff0c;在Inspector中设置
    public Color GLRectEdgeColor;//矩形的边框颜色&#xff0c;在Inspector中设置

    void OnPostRender(){
        if(onDrawingRect){
            //准备工作:获取确定矩形框各角坐标所需的各个数值
            float Xmin = Mathf.Min(startPoint.x, currentPoint.x);
            float Xmax = Mathf.Max(startPoint.x, currentPoint.x);
            float Ymin = Mathf.Min(startPoint.y, currentPoint.y);
            float Ymax = Mathf.Max(startPoint.y, currentPoint.y);

            GL.PushMatrix();//GL入栈
                            //在这里&#xff0c;只需要知道GL.PushMatrix()和GL.PopMatrix()分别是画图的开始和结束信号,画图指令写在它们中间
            if(!GLRectMat)
                return;

            GLRectMat.SetPass(0);//启用线框材质rectMat

            GL.LoadPixelMatrix();//设置用屏幕坐标绘图

            /*------第一步&#xff0c;绘制矩形------*/
            GL.Begin(GL.QUADS);//开始绘制矩形,这一段画的是框中间的半透明部分&#xff0c;不包括边界线

            GL.Color(GLRectColor);//设置矩形的颜色&#xff0c;注意GLRectColor务必设置为半透明

            //陈述矩形的四个顶点
            GL.Vertex3(Xmin, Ymin, 0);//陈述第一个点&#xff0c;即框的左下角点&#xff0c;记为点1
            GL.Vertex3(Xmin, Ymax, 0);//陈述第二个点&#xff0c;即框的左上角点&#xff0c;记为点2
            GL.Vertex3(Xmax, Ymax, 0);//陈述第三个点&#xff0c;即框的右上角点&#xff0c;记为点3
            GL.Vertex3(Xmax, Ymin, 0);//陈述第四个点&#xff0c;即框的右下角点&#xff0c;记为点4

            GL.End();//告一段落&#xff0c;此时画好了一个无边框的矩形


            /*------第二步&#xff0c;绘制矩形的边框------*/
            GL.Begin(GL.LINES);//开始绘制线&#xff0c;用来描出矩形的边框

            GL.Color(GLRectEdgeColor);//设置方框的边框颜色&#xff0c;建议设置为不透明的

            //描第一条边
            GL.Vertex3(Xmin, Ymin, 0);//起始于点1
            GL.Vertex3(Xmin, Ymax, 0);//终止于点2

            //描第二条边
            GL.Vertex3(Xmin, Ymax, 0);//起始于点2
            GL.Vertex3(Xmax, Ymax, 0);//终止于点3

            //描第三条边
            GL.Vertex3(Xmax, Ymax, 0);//起始于点3
            GL.Vertex3(Xmax, Ymin, 0);//终止于点4

            //描第四条边
            GL.Vertex3(Xmax, Ymin, 0);//起始于点4
            GL.Vertex3(Xmin, Ymin, 0);//返回到点1

            GL.End();//画好啦&#xff01;

            GL.PopMatrix();//GL出栈
        }
    }
    Vector2 GetWorldMousePosition(){
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}