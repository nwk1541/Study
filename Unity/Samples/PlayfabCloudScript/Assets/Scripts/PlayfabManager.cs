using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    private const string titleID = "7FB1B";

    public static void OnLoginPlayfab()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            /*
            Please change the titleId below to your own titleId from PlayFab Game Manager.
            If you have already set the value in the Editor Extensions, this can be skipped.
            */
            PlayFabSettings.staticSettings.TitleId = titleID;
        }

        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest { CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, (LoginResult loginResult) =>
        {
            Debug.Log("Congratulations, you made your first successful API call!");
        }, OnPlayfabError);
    }

    public static void OnHelloWorld()
    {
        ExecuteCloudScriptRequest req = new ExecuteCloudScriptRequest();
        req.FunctionName = "helloWorld";
        req.GeneratePlayStreamEvent = true;

        PlayFabClientAPI.ExecuteCloudScript(req, (ExecuteCloudScriptResult result) =>
        {
            Debug.Log(req.FunctionName + ":" + result.Logs[0].Level + " " + result.Logs[0].Message);
        }, OnPlayfabError);
    }

    private static void OnPlayfabError(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
}