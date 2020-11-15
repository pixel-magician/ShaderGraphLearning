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
    /// <summary>
    /// 角标尖到角标中心点的向量
    /// </summary>
    [SerializeField]
    Vector2[] _cornerMarkPoint = new Vector2[]
    {
        new Vector2(26, -56),
        new Vector2(26, -56),
        new Vector2(26, -56),
        new Vector2(26, -56),
    };


    public UnityEvent OnClick;

    public DialogCornerMarkDir Default
    {
        get => _default;
        set
        {
            GetCornerMarkRect().gameObject.SetActive(false);
            _default = value;
            GetCornerMarkRect().gameObject.SetActive(true);
        }
    }

    private void Awake()
    {
        LU.gameObject.SetActive(false);
        RU.gameObject.SetActive(false);
        LD.gameObject.SetActive(false);
        RD.gameObject.SetActive(false);
        switch (_default)
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
        Vector2 sizeDelta = _text.GetComponent<RectTransform>().sizeDelta;
        switch (_default)
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
        var uiPos = pos
            - new Vector2(Screen.width / 2, Screen.height / 2)
            - posAdd / 2
            - cornerMark.anchoredPosition
            - _cornerMarkPoint[pointIndex]; ;
        GetComponent<RectTransform>().anchoredPosition = uiPos;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetData(Input.mousePosition, "我是猪虎，一只可爱的小猪！");
        }
        if (Input.GetMouseButtonDown(1))
        {
            SetData(Input.mousePosition, "1");
        }
    }







    /// <summary>
    /// 获取默认角标的 RectTransform
    /// </summary>
    /// <returns></returns>
    RectTransform GetCornerMarkRect()
    {
        switch (_default)
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
    /// <summary>
    /// 对话框角标方向
    /// </summary>
    public enum DialogCornerMarkDir
    {
        LU,
        RU,
        LD,
        RD,
    }
}
