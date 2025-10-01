// InventoryUI.cs
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    public GameObject panel;
    void Start() {
        if (panel) panel.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            if (panel) panel.SetActive(!panel.activeSelf);
            Cursor.lockState = panel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
