using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeMenu;

    [Header("Buttons")]
    public Button weaponButton;
    public Button sprintButton;
    public Button healButton;

    public TMP_Text weaponText;
    public TMP_Text sprintText;
    public TMP_Text healText;

    private PlayerController player;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();

        weaponButton.onClick.AddListener(ChooseWeaponUpgrade);
        sprintButton.onClick.AddListener(ChooseSprintUpgrade);
        healButton.onClick.AddListener(ChooseSelfHeal);

        upgradeMenu.SetActive(false);
    }

    public void ShowUpgradeMenu()
    {
        upgradeMenu.SetActive(true);
        UpdateUpgradeButtons();
        player.canMove = false;
    }

    public void HideUpgradeMenu()
    {
        upgradeMenu.SetActive(false);
        player.canMove = true;
    }

    void ChooseWeaponUpgrade()
    {
        player.UpgradeWeapon();
        UpdateUpgradeButtons();
        HideUpgradeMenu();
    }

    void ChooseSprintUpgrade()
    {
        player.UpgradeSprint();
        UpdateUpgradeButtons();
        HideUpgradeMenu();
    }

    void ChooseSelfHeal()
    {
        player.UpgradeSelfHeal();
        UpdateUpgradeButtons();
        HideUpgradeMenu();
    }

    public void UpdateUpgradeButtons()
    {
        weaponText.text = player.weaponLevel >= player.maxWeaponLevel ? "Weapon (Max)" : $"Weapon (Lv. {player.weaponLevel})";
        weaponButton.interactable = player.weaponLevel < player.maxWeaponLevel;

        healText.text = player.selfHealLevel >= player.maxSelfHealLevel ? "Self Heal (Max)" : $"Self Heal (Lv. {player.selfHealLevel})";
        healButton.interactable = player.selfHealLevel < player.maxSelfHealLevel;

        sprintText.text = player.sprintLevel >= player.maxSprintLevel ? "Sprint (Max)" : $"Sprint (Lv. {player.sprintLevel})";
        sprintButton.interactable = player.sprintLevel < player.maxSprintLevel;
    }
}
