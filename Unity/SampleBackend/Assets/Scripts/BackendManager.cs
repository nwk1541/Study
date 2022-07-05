using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public partial class BackendManager : MonoBehaviour
{
    public BackendManager Instance;

    private void Awake()
    {
        Instance = this;

        Backend.Initialize();

        if (!SendQueue.IsInitialize)
            SendQueue.StartSendQueue(true, SendQueueExceptionHandler);
    }

    private void Update()
    {
        if (SendQueue.IsInitialize)
            SendQueue.Poll();
    }

    private void OnApplicationQuit()
    {
        SendQueue.StopSendQueue();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SendQueue.PauseSendQueue();
        else
            SendQueue.ResumeSendQueue();
    }

    private void SendQueueExceptionHandler(Exception ex)
    {
        Debug.LogErrorFormat("SendQueueExceptionHandler : {0}, {1}", ex.Message, ex.StackTrace);
    }

    // TODO : implement T4, T5 if is needed

    private void Request<T1, T2, T3>(Func<T1, T2, T3, BackendReturnObject> req, T1 param1, T2 param2, T3 param3, Action<LitJson.JsonData> actionOnSuccess = null, Action actionOnFail = null)
    {
        SendQueue.Enqueue(req, param1, param2, param3, (resp) =>
        {
            if (resp.IsSuccess())
                OnRequestSuccess(resp, actionOnSuccess);
            else
                OnRequestFail(resp, actionOnFail);
        });
    }

    private void Request<T1, T2>(Func<T1, T2, BackendReturnObject> req, T1 param1, T2 param2, Action<LitJson.JsonData> actionOnSuccess = null, Action actionOnFail = null)
    {
        SendQueue.Enqueue(req, param1, param2, (resp) =>
        {
            if (resp.IsSuccess())
                OnRequestSuccess(resp, actionOnSuccess);
            else
                OnRequestFail(resp, actionOnFail);
        });
    }

    private void Request<T>(Func<T, BackendReturnObject> req, T param, Action<LitJson.JsonData> actionOnSuccess = null, Action actionOnFail = null)
    {
        SendQueue.Enqueue(req, param, (resp) =>
        {
            if (resp.IsSuccess())
                OnRequestSuccess(resp, actionOnSuccess);
            else
                OnRequestFail(resp, actionOnFail);
        });
    }

    private void Request(Func<BackendReturnObject> req, Action<LitJson.JsonData> actionOnSuccess = null, Action actionOnFail = null)
    {
        SendQueue.Enqueue(req, (resp) =>
        {
            if (resp.IsSuccess())
                OnRequestSuccess(resp, actionOnSuccess);
            else
                OnRequestFail(resp, actionOnFail);
        });
    }

    private void OnRequestSuccess(BackendReturnObject response, Action<LitJson.JsonData> actionOnSuccess)
    {
        LitJson.JsonData jsonData = null;
        if (response.HasReturnValue())
        {
            if(response.HasRows())
                jsonData = response.FlattenRows();
        }

        if (actionOnSuccess != null)
            actionOnSuccess(jsonData);
    }

    private void OnRequestFail(BackendReturnObject response, Action actionOnFail)
    {
        Debug.LogFormat("{0}, {1}, {2}", response.GetStatusCode(), response.GetErrorCode(), response.GetMessage());

        if (actionOnFail != null)
            actionOnFail();
    }
}
