using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManager : MonoBehaviour
{
    NetClient m_NetClient;
    Queue<KeyValuePair<int, string>> m_MessageQueue = new Queue<KeyValuePair<int, string>>();
    XLua.LuaFunction ReceiveMessage;

    public void Init()
    {
        m_NetClient = new NetClient();
        ReceiveMessage = Manager.Lua.LuaEnv.Global.Get<XLua.LuaFunction>("ReceiveMessage");
    }

    //������Ϣ
    public void SendMessage(int messageId, string message)
    {
        m_NetClient.SendMessage(messageId, message);
    }

    //���ӵ�������
    public void ConnectedServer(string post, int port)
    {
        m_NetClient.OnConnectServer(post, port);
    }

    //��������
    public void OnNetConnected()
    {

    }

    //���������Ͽ�����
    public void OnDisConnected()
    {

    }

    //���ܵ�����
    public void Receive(int msgId, string message)
    {
        m_MessageQueue.Enqueue(new KeyValuePair<int, string>(msgId, message));
    }

    private void Update()
    {
        if(m_MessageQueue.Count > 0)
        {
            KeyValuePair<int, string> msg = m_MessageQueue.Dequeue();
            ReceiveMessage?.Call(msg.Key, msg.Value);
        }
    }
}
