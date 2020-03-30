using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PanelBase : MonoBehaviour
{
    /// <summary> 패널 최초 로드시 한번 실행</summary>
    public virtual void Init() { }

    /// <summary> 패널 활성화시 동작</summary>
    public virtual void OpenPanel() { }

    /// <summary> 패널 비활성화시 동작</summary>
    public virtual void ClosePanel() { }

    /// <summary> 백버튼 눌렸을시  </summary>
    public virtual void OnClickBack() { }
}
