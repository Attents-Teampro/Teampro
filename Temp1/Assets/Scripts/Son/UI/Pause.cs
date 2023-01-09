using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class Pause : MonoBehaviour
{
    Button btn;
    bool isPause = false;
    [SerializeField]
    Pause_Window window;
    public bool IsPaused 
    {
        get => isPause;
        private set => isPause = value;
    }
    
    PlayerInputAction inputAction;
    private void OnEnable()
    {
        inputAction.Enable();
        inputAction.UI.Esc.canceled += UI_Pause_Esc;
    }
    private void OnDisable()
    {
        inputAction.UI.Esc.canceled -= UI_Pause_Esc;
        inputAction.Disable();
    }
    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(UI_Pause);
        inputAction = new PlayerInputAction();
    }
    private void Start()
    {
        window.gameObject.SetActive(false);
    }
    private void UI_Pause_Esc(InputAction.CallbackContext obj)
    {
        PauseSet();
    }
    public void UI_Pause()
    {
        PauseSet();
    }
    void PauseSet()
    {
        
        if (isPause)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
        isPause = !isPause;
        window.gameObject.SetActive(isPause);

    }
}
