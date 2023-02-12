using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alphabet : MonoBehaviour
{
    [SerializeField] private Sprite[] _bigNumbers;
    public Sprite[] BigNumbers { get => _bigNumbers; }

    [SerializeField] private Sprite[] _smallNumbers;
    public Sprite[] SmallNumbers { get => _smallNumbers; }

    public static Alphabet Instance { get; set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
