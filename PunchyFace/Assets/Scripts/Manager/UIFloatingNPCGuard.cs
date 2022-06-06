using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFloatingNPCGuard : MonoBehaviour
{
    [SerializeField] private GameObject floatingUIPrefab;
    [SerializeField] private Vector3 offset;

    private TextMesh currentPowerLevelText;
    private NPCGuard npcGuard;
    private GameObject floatingUI;

    private bool isDisable;

    public bool IsDisable { get => isDisable; set => isDisable = value; }

    // Start is called before the first frame update
    void OnEnable()
    {
        floatingUI = Instantiate(floatingUIPrefab, transform);
        currentPowerLevelText = floatingUI.GetComponent<TextMesh>();
        npcGuard = GetComponent<NPCGuard>();
        currentPowerLevelText.text = npcGuard.CurrentPowerLevel.ToString();
        floatingUI.transform.localPosition += offset;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDisable)
            currentPowerLevelText.text = "";
        else
            currentPowerLevelText.text = npcGuard.CurrentPowerLevel.ToString();
    }
}
