using UnityEngine;
using System.Collections.Generic;
public class EventCenter
{
    private EventCenter() { }
    private static EventCenter eventCenter= null;
    public static EventCenter GetInstance()
    {
        if (eventCenter == null)
            eventCenter = new EventCenter();
        return eventCenter;
    }
    public delegate void processEvent(Object obj, int param1, int param2, Vector3 vector3); 
    //把委托当成一个指针来理解
    private Dictionary<string, processEvent> eventMap = new Dictionary<string, processEvent>(); 
    //那么这里存贮的就是一个字符串，对应一个函数地址
    //声明一个自定义委托，使用字典键值对存贮，

    //注册
    public void Regist(string name, processEvent func)
    {
        if (eventMap.ContainsKey(name))
            eventMap[name] += func;
        else
            eventMap[name] = func;
    }
    //注销
    public void UnRegist(string name, processEvent func)
    {
        if (eventMap.ContainsKey(name))
            eventMap[name] -= func;
    }
    //触发
    public void Trigger(string name, Object obj, int param1, int param2, Vector3 vector3)
    {
        if (eventMap.ContainsKey(name))
            eventMap[name].Invoke(obj, param1, param2, vector3);
    }

    //+++运行逻辑：当触发 触发函数时，判断字典内是否有对应注册好的字符串，若有，则执行字符串对应的函数地址的函数，则完成一次触发
}

/*
    //注册方面
    private void Start()
    {
        EventCenter.GetInstance().Regist("LookItem", OnLookItem);
    }
    private void OnLookItem(Object obj, int param1, int param2)
    {
        Debug.Log(obj.name + "被发现了");
    }


    //触发方面：
    EventCenter.GetInstance().Trigger("LookItem", hit.collider.gameObject, 0, 0);

*/

