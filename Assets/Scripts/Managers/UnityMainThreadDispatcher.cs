using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    public static UnityMainThreadDispatcher _instance;
    private static Queue<System.Action> _actionQueue = new
       Queue<System.Action>();

    private void Awake()
    {
        if (!Application.isBatchMode)
        {
            Destroy(this.gameObject);
        }

        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        lock (_actionQueue)
        {
            while (_actionQueue.Count > 0)
            {
                _actionQueue.Dequeue().Invoke();
            }
        }
    }

    public void ExecuteOnMainThread(System.Action action)
    {
        lock (_actionQueue)
        {
            _actionQueue.Enqueue(action);
        }
    }
}
