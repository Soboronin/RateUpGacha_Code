using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class CommonButton : Button
{
    private static readonly string STATE_NAME = "Pressed";

    /// <summary>
    /// アニメーション待機設定
    /// </summary>
    /// <param name="endAction">終了時</param>
    public async UniTask SetWaitPressedAnimation(Action endAction = null)
    {
        await UniTask.WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(STATE_NAME) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        endAction?.Invoke();
    }
}
