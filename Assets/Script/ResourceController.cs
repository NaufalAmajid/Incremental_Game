using UnityEngine;
using UnityEngine.UI;

public class ResourceController : MonoBehaviour
{
    public Button ResourceButton;
    public Image ResourceImage;
    public Text ResourceDescription;
    public Text ResourceUpgradeCost;
    public Text ResourceUnlockCost;

    //SOUND EFFECT
    public AudioSource myfx;
    public AudioClip click;
    public AudioClip click_upgrade;

    private ResourceConfig _config;

    private int _level = 1;

    public bool IsUnlocked { get; private set; }

    //PRIVATE VOID START
    private void Start ()
    {
        ResourceButton.onClick.AddListener (() =>
        {
            if (IsUnlocked)
            {
                UpgradeLevel ();
            }
            else
            {
                UnlockResource ();
            }
        });
        
    }

    //VOID SETCONFIG
    public void SetConfig (ResourceConfig config)
    {
        _config = config;

        
        ResourceDescription.text = $"{ _config.Name } Lv. { _level }\n+{ GetOutput ().ToString ("0") }";
        ResourceUnlockCost.text = $"Unlock Cost\n{ _config.UnlockCost }";
        ResourceUpgradeCost.text = $"Upgrade Cost\n{ GetUpgradeCost () }";

        SetUnlocked (_config.UnlockCost == 0);
        
        
    }

    //PUBLIC DOUBLE GETOUTPUT
    public double GetOutput ()
    {
        return _config.Output * _level;
    }

    //PUBLIC DOUBLE GETUPGRADECOST
    public double GetUpgradeCost ()
    {
        return _config.UpgradeCost * _level;
    }

    //PUBLIC DOUBLE GETUNLOCKCOST
    public double GetUnlockCost ()
    {
        return _config.UnlockCost;
        
    }

    // VOID UPGRADELEVEL
    public void UpgradeLevel ()
    {
        double upgradeCost = GetUpgradeCost ();
        if (GameManager.Instance.TotalGold < upgradeCost)
        {
            return;
        }

        GameManager.Instance.AddGold (-upgradeCost);
        _level++;

        ResourceUpgradeCost.text = $"Upgrade Cost\n{ GetUpgradeCost () }";
        ResourceDescription.text = $"{ _config.Name } Lv. { _level }\n+{ GetOutput ().ToString ("0") }";

        myfx.PlayOneShot(click_upgrade);
   }

    //VOID UNLOCKRESOURCE
    public void UnlockResource ()
    {
        double unlockCost = GetUnlockCost ();
        if (GameManager.Instance.TotalGold < unlockCost)
        {
            return;
        }

        SetUnlocked (true);
        GameManager.Instance.ShowNextResource ();
        AchievementController.Instance.UnlockAchievement (AchievementType.UnlockResource, _config.Name);
        
        myfx.PlayOneShot(click_upgrade);
    }

    //VOID SETUNLOCKED
    public void SetUnlocked (bool unlocked)
    {
        IsUnlocked = unlocked;
        ResourceImage.color = IsUnlocked ? Color.white : Color.grey;
        ResourceUnlockCost.gameObject.SetActive (!unlocked);
        ResourceUpgradeCost.gameObject.SetActive (unlocked);
        
    }
}