using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SkillMarket : MonoBehaviour
{
    public static SkillMarket instance { get; set;}

    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI TitleText;
    [SerializeField] Image IconImg;
    [SerializeField] TextMeshProUGUI DetailText;
    [SerializeField] Slider UnlockBar;
    [SerializeField] TextMeshProUGUI InputText;
    [SerializeField] TextMeshProUGUI RequienmentText;
    [SerializeField] TextMeshProUGUI AdditionalEffectText;
    [SerializeField] TextMeshProUGUI AdditionalRequienmentText;
    [SerializeField] Transform SkillSelector;

    bool CanUnlock;
    bool IsMarketActive;
    SkillButton _button;
    Skill skillinfo;

    float UnlockTime = 0;

    #region Market Initialization
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }
    #endregion

    #region Public Methods
    public void OpenMarket(Skill skillinfo)
    {
        if (_button != skillinfo.button)
        {
            this.skillinfo = skillinfo;
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOLocalMoveX(1220, .2f).SetEase(Ease.InBack));
            s.Join(SkillSelector.DOMove(skillinfo.button.transform.position, .2f).SetEase(Ease.OutBack));
            s.AppendCallback(() => SetMarket());
            s.Append(transform.DOLocalMoveX(710, .2f).SetEase(Ease.OutBack));
        }               
    }

    public void CloseMarket()
    {
        IsMarketActive = false;
        _button = null;
        transform.DOLocalMoveX(1220, .2f).SetEase(Ease.InBack);
        SkillSelector.DOLocalMove(new Vector3(-1100 , 115 , 0), .2f).SetEase(Ease.OutBack);
    }

    void SetMarket()
    {
        TitleText.text = skillinfo.Name;
        IconImg.sprite = skillinfo.Icon;
        DetailText.text = skillinfo.Info;
        AdditionalEffectText.text = skillinfo.AdditionalEffect;

        RequienmentText.SetText($"{EconomyManager.instance.XP} / {skillinfo.SkillPointToUnlock}");
        AdditionalRequienmentText.SetText($"{EconomyManager.instance.coin} / {skillinfo.StatForAE}");

        _button = skillinfo.button;
        if (CanUnlock = !_button.IsUnlocked)
        {            
            UnlockBar.value = UnlockTime = 0;
            if (_button.CanUnlock() && EconomyManager.instance.XP >= skillinfo.SkillPointToUnlock)
            {
                InputText.transform.parent.localPosition = new Vector3(-75, 0, 0);
                RequienmentText.transform.parent.gameObject.SetActive(true);
                UnlockBar.gameObject.SetActive(true);
                InputText.color = Color.white;
                InputText.text = "HOLD ( H )";
                IsMarketActive = true;
            }
            else
            {
                UnlockBar.gameObject.SetActive(false);
                RequienmentText.transform.parent.gameObject.SetActive(false);
                InputText.transform.parent.localPosition = new Vector3(0, 0, 0);
                InputText.color = Color.red;
                InputText.text = (!_button.CanUnlock()) ? "LOCKED" : "NOT ENOUGH XP";
                IsMarketActive = false;
            }
        }
        else
        {
            UnlockBar.gameObject.SetActive(false);
            RequienmentText.transform.parent.gameObject.SetActive(false);
            InputText.transform.parent.localPosition = new Vector3(0, 0, 0);
            InputText.color = Color.white;
            InputText.text = "PURCHASED";
            IsMarketActive = false;
        }
    }
    #endregion

    #region Markiet Updation
    void FixedUpdate()
    {
        if(IsMarketActive && !_button.IsUnlocked && Input.GetKey(KeyCode.H))
        {

            if(UnlockTime < 1)
            {
                UnlockTime += Time.deltaTime;
                UnlockBar.value = UnlockTime;
            }
            else
            {
                EconomyManager.instance.AddXP(-skillinfo.SkillPointToUnlock);
                _button.UnlockSkill();
                SetMarket();
            }
            
        }
        else if(UnlockTime > 0)
        {
            UnlockTime -= Time.deltaTime;
            UnlockBar.value = UnlockTime;
        }
    }


    #endregion
}
