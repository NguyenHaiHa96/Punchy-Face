using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIFloatingPlayer : MonoBehaviour
{
    [SerializeField] private GameObject floatingUIPrefab;
    [SerializeField] private Vector3 offset;

    private TextMesh currentPowerLevelText;
    private Player player;
    private GameObject floatingUI;

    private bool isDisable;

    public bool IsDisable { get => isDisable; set => isDisable = value; }

    // Start is called before the first frame update
    void OnEnable()
    {
        isDisable = false;
        floatingUI = Instantiate(floatingUIPrefab, transform);
        currentPowerLevelText = floatingUI.GetComponent<TextMesh>();
        player = GetComponent<Player>();
        currentPowerLevelText.text = player.CurrentPowerLevel.ToString();
        floatingUI.transform.localPosition += offset;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDisable)
            currentPowerLevelText.text = "";
        else
            currentPowerLevelText.text = player.CurrentPowerLevel.ToString();
    }
}
