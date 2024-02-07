using StaserSDK.Upgrades;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class StackCheatUpgrade : MonoBehaviour
{
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Upgrade _upgrade;
    
    [Inject] private UpgradesController _upgradesController;
    [Inject] public Player Player { get; }
    private void OnEnable()
    {
        _upgradeButton.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _upgradeButton.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        var model = _upgradesController.GetModel(_upgrade);
        if (model.CanLevelUp())
            model.LevelUp();
    }
}
