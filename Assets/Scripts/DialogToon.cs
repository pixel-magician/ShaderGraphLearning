using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogToon : MonoBehaviour
{
    [SerializeField]
    Text _text;
    [SerializeField]
    RectTransform LU, RU, LD, RD;
    [SerializeField]
    private DialogCornerMarkDir _default = DialogCornerMarkDir.RD;
    private DialogCornerMarkDir _cornerMarkDir = DialogCornerMarkDir.RD;
    /// <summary>
    /// 角标尖到角标中心点的向量
    /// 手动测量出来的数值
    /// </summary>
    //[SerializeField]
    Vector2[] _cornerMarkPoint = new Vector2[]
    {
        new Vector2(-26, 56),
        new Vector2(26, 56),
        new Vector2(-26, -56),
        new Vector2(26, -56),
    };


    public UnityEvent OnClick;
    /// <summary>
    /// 记录当前角标的方向值
    /// </summary>
    public DialogCornerMarkDir CornerMarkDir
    {
        get => _cornerMarkDir;
        set
        {
            GetCornerMarkRect().gameObject.SetActive(false);
            _cornerMarkDir = value;
            GetCornerMarkRect().gameObject.SetActive(true);
        }
    }

    private void Awake()
    {
        _cornerMarkDir = _default;
        LU.gameObject.SetActive(false);
        RU.gameObject.SetActive(false);
        LD.gameObject.SetActive(false);
        RD.gameObject.SetActive(false);
        switch (CornerMarkDir)
        {
            case DialogCornerMarkDir.LU:
                LU.gameObject.SetActive(true);
                break;
            case DialogCornerMarkDir.RU:
                RU.gameObject.SetActive(true);
                break;
            case DialogCornerMarkDir.LD:
                LD.gameObject.SetActive(true);
                break;
            case DialogCornerMarkDir.RD:
                RD.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }



    public void SetData(Vector2 pos, string message)
    {
        //调整数值和锚点
        _text.text = message;
        _text.GetComponent<ContentSizeFitter>().SetLayoutHorizontal();//双层自动布局组件，需要强制刷新一次
        Vector2 sizeDelta = _text.GetComponent<RectTransform>().sizeDelta;


        //自适角标位置
        int defaultBinValue = (int)_default;
        if (pos.y > Screen.height - sizeDelta.y - 100) defaultBinValue |= 0B0100;//0100，使第二位为1
        else if (pos.y < sizeDelta.y + 100) defaultBinValue &= 0B1011;//1011，使第二位为0
        if (pos.x > Screen.width - sizeDelta.x - 100) defaultBinValue &= 0B1101;//0010，使第三位为1
        else if (pos.x < sizeDelta.x + 100) defaultBinValue |= 0B0010;//1101，使第三位为0

        CornerMarkDir = (DialogCornerMarkDir)(defaultBinValue);




        RectTransform cornerMark = GetCornerMarkRect();
        if (_text.GetComponent<RectTransform>().sizeDelta.x < cornerMark.sizeDelta.x / 2)
        {
            //计算Text宽度过小时的锚点
            cornerMark.anchorMax = new Vector2(0.5f, 0.5f - cornerMark.localScale.y * 0.5f);//上方的角标ScaleY都是-1，下方则是1
            cornerMark.anchorMin = new Vector2(0.5f, 0.5f - cornerMark.localScale.y * 0.5f);
            cornerMark.anchoredPosition = new Vector2(0, cornerMark.anchoredPosition.y);
        }
        else
        {
            //计算锚点
            var anchor = new Vector2(0.5f + cornerMark.localScale.x * 0.5f, 0.5f - cornerMark.localScale.y * 0.5f);
            cornerMark.anchorMax = anchor;
            cornerMark.anchorMin = anchor;
            cornerMark.anchoredPosition = new Vector2(-35 * cornerMark.localScale.x, cornerMark.anchoredPosition.y);
        }

        //调整坐标
        int pointIndex = -1;
        Vector2 posAdd = Vector2.zero;
        switch (CornerMarkDir)
        {
            case DialogCornerMarkDir.LU:
                posAdd = new Vector2(-sizeDelta.x, sizeDelta.y);
                pointIndex = 0;
                break;
            case DialogCornerMarkDir.RU:
                posAdd = new Vector2(sizeDelta.x, sizeDelta.y);
                pointIndex = 1;
                break;
            case DialogCornerMarkDir.LD:
                posAdd = new Vector2(-sizeDelta.x, -sizeDelta.y);
                pointIndex = 2;
                break;
            case DialogCornerMarkDir.RD:
                posAdd = new Vector2(sizeDelta.x, -sizeDelta.y);
                pointIndex = 3;
                break;
            default:
                break;
        }
        var uiPosAdd = new Vector2(Screen.width / 2, Screen.height / 2)
            + posAdd / 2
            + cornerMark.anchoredPosition
            + _cornerMarkPoint[pointIndex]; ;
        GetComponent<RectTransform>().anchoredPosition = pos - uiPosAdd;



    }

    private void Update()
    {
        #region 测试代码

        if (Input.GetMouseButtonDown(0))
        {
            SetData(Input.mousePosition, "我是猪虎，一只可爱的小猪！");
        }
        if (Input.GetMouseButtonDown(1))
        {
            SetData(Input.mousePosition, "1");
        }
        #endregion
    }







    /// <summary>
    /// 获取默认角标的 RectTransform
    /// </summary>
    /// <returns></returns>
    RectTransform GetCornerMarkRect()
    {
        switch (CornerMarkDir)
        {
            case DialogCornerMarkDir.LU:
                return LU;
            case DialogCornerMarkDir.RU:
                return RU;
            case DialogCornerMarkDir.LD:
                return LD;
            case DialogCornerMarkDir.RD:
                return RD;
            default:
                return null;
        }
    }
    ///// <summary>
    ///// 将十六进制数转换为枚举类型
    ///// 值为0时，保持默认不变
    ///// </summary>
    ///// <param name="value"></param>
    ///// <returns></returns>
    //DialogCornerMarkDir GetDialogCornerMarkDir(int value)
    //{


    //    if (value == 0)
    //        return _default;
    //    return (DialogCornerMarkDir)value;
    //}

    /// <summary>
    /// 对话框角标方向
    /// </summary>
    public enum DialogCornerMarkDir
    {
        LU = 0x7,//0111
        RU = 0x5,//0101
        LD = 0x3,//0011
        RD = 0x1,//0001
    }
}
