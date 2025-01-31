using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class mainMenu : MonoBehaviour
{
    [SerializeField] GameObject Settings;

    [SerializeField] Button[] buttonList;
    [SerializeField] Button backButton;
    [SerializeField] Color notSelectedColor;
    [SerializeField] Color selectedColor;
    private int selectedButton = 0;
    private bool isInSettings = false;
    public void moving(CallbackContext cb) {
        if (!cb.started) return;
        Vector2 mvValFloat = cb.ReadValue<Vector2>();
        selectedButton -= (int)mvValFloat.y;
        selectedButton %= 4;
        if(selectedButton < 0) selectedButton = 3;
        ColorBlock colorB = buttonList[0].colors;

        colorB.normalColor = notSelectedColor;
        foreach (var button in buttonList) {
            button.colors = colorB;
        }
        colorB.normalColor = selectedColor;
        buttonList[selectedButton].colors = colorB; 
    }

    public void action(CallbackContext cb) {
        if (!cb.started) return;

        if (!isInSettings)
        {
            switch (selectedButton)
            {
                case 0:
                    PlayGame();
                    break;
                case 1:
                    PlaySandbox();
                    break;
                case 2:
                    foreach (var button in buttonList)
                    {
                        button.gameObject.SetActive(false);
                    }
                    Settings.SetActive(true);
                    isInSettings = true;
                    break;
                case 3:
                    QuitGame();
                    break;
            }
        }
        else {
            foreach (var button in buttonList)
            {
                button.gameObject.SetActive(true);
            }
            Settings.SetActive(false);
            isInSettings = false;
        }


    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    
    public void PlaySandbox()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    Console.WriteLine("Beendet");
    }
}
