using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    private const string titleID = "7FB1B";

    public static string PlayfabId { get; private set; }

    public static void LoginPlayfab()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            /*
            Please change the titleId below to your own titleId from PlayFab Game Manager.
            If you have already set the value in the Editor Extensions, this can be skipped.
            */
            PlayFabSettings.staticSettings.TitleId = titleID;
        }

        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest();
        request.CustomId = SystemInfo.deviceUniqueIdentifier;
        request.CreateAccount = true;

        PlayFabClientAPI.LoginWithCustomID(request, OnSuccessLogin, OnErrorPlayfabAPICall);
    }

    private static void OnSuccessLogin(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");

        result.PlayFabId = PlayfabId;
        Debug.Log(result.ToJson());
    }

    private static void OnErrorPlayfabAPICall(PlayFabError error)
    {
        Debug.LogError("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    public static void HelloWorld()
    {
        ExecuteCloudScriptRequest req = new ExecuteCloudScriptRequest();
        req.FunctionName = "helloWorld";
        req.GeneratePlayStreamEvent = true;

        PlayFabClientAPI.ExecuteCloudScript(req, OnSuccessHelloWorld, OnErrorPlayfabAPICall);
    }

    private static void OnSuccessHelloWorld(ExecuteCloudScriptResult result)
    {
        Debug.Log(result.FunctionName + ":" + result.Logs[0].Level + " " + result.Logs[0].Message);
    }

    public static void SetUserData()
    {
        UpdateUserDataRequest request = new UpdateUserDataRequest();
        request.Data = new System.Collections.Generic.Dictionary<string, string>()
        {
            { "Ancestor", "Arthur" },
            { "Successor", "Fred" }
        };

        PlayFabClientAPI.UpdateUserData(request, OnSuccessSetUserData, OnErrorPlayfabAPICall);
    }

    private static void OnSuccessSetUserData(UpdateUserDataResult result)
    {
        Debug.Log("Successfully updated user data");
        Debug.Log(result.ToJson());
    }

    public static void GetUserData(string playerId)
    {
        GetUserDataRequest request = new GetUserDataRequest();
        request.PlayFabId = playerId;
        request.Keys = null;

        PlayFabClientAPI.GetUserData(request, OnSuccessGetUserData, OnErrorPlayfabAPICall);
    }

    private static void OnSuccessGetUserData(GetUserDataResult result)
    {
        Debug.Log("Successfully get user data");
        Debug.Log(result.ToJson());
    }

    public static void GetTitleData()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), OnSuccessGetTitleData, OnErrorPlayfabAPICall);
    }

    private static void OnSuccessGetTitleData(GetTitleDataResult result)
    {
        Debug.Log("Got error getting titleData:");
        Debug.Log(result.ToJson());
    }
}