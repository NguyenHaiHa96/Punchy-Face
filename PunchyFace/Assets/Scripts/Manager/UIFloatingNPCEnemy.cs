using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFloatingNPCEnemy : MonoBehaviour
{
    [SerializeField] private GameObject floatingUIPrefab;
    [SerializeField] private Vector3 offset;

    private TextMesh currentPowerLevelText;
    private NPCEnemy npcEnemy;
    private GameObject floatingUI;

    private bool isDisable;

    public bool IsDisable { get => isDisable; set => isDisable = value; }

    // Start is called before the first frame update
    void OnEnable()
    {
        floatingUI = Instantiate(floatingUIPrefab, transform);
        currentPowerLevelText = floatingUI.GetComponent<TextMesh>();
        npcEnemy = GetComponent<NPCEnemy>();
        currentPowerLevelText.text = npcEnemy.CurrentPowerLevel.ToString();
        floatingUI.transform.localPosition += offset;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDisable)
            currentPowerLevelText.text = "";
        else
            currentPowerLevelText.text = npcEnemy.CurrentPowerLevel.ToString();
    }
}
