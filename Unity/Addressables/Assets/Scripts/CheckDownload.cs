using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;
using UnityEngine.UI;

public class CheckDownload : MonoBehaviour
{
    public TextMeshProUGUI size;
    public Button downloadButton;

    private void Start()
    {
        Manager.Instance.GetDownloadSize((res) =>
        {
            size.text = res.ToString();
        });

        downloadButton.onClick.AddListener(OnClickButton);
    }

    public void OnClickButton()
    {
        AsyncOperationHandle handle = Addressables.DownloadDependenciesAsync("Common");
        handle.Completed += (op) =>
        {
            if (op.IsDone)
                downloadButton.gameObject.SetActive(false);
        };
    }
}
