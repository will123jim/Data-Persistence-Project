using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static string playerName;
    public InputField nameInputField;
    // Start is called before the first frame update
   public void StartNew() 
    {
      
         playerName = nameInputField.text;

         if (string.IsNullOrEmpty(playerName))
         {
            playerName = "DefaultName";
         }
  SceneManager.LoadScene(1);
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

