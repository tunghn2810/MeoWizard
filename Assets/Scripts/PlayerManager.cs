using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerManager : MonoBehaviour
{   
    [SerializeField] private GameObject playerPrefab;

    void Start()
    {
        PlayerInput.Instantiate(playerPrefab, controlScheme: "Keyboard_1", pairWithDevice: Keyboard.current);
        PlayerInput.Instantiate(playerPrefab, controlScheme: "Keyboard_2", pairWithDevice: Keyboard.current);
    }
}
