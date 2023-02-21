using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] private GameObject[] _softWallLines;
    [SerializeField] private List<GameObject> _softWallList = new List<GameObject>();
    [SerializeField] private List<GameObject> _softWallInactives = new List<GameObject>();
    [SerializeField] private GameObject[] _p3Walls;
    [SerializeField] private GameObject[] _p4Walls;

    [SerializeField] private GameObject[] _bombItems;
    private int _bombCount = 0;
    private const int MAX_BOMB_COUNT = 10;
    [SerializeField] private GameObject[] _fireItems;
    private int _fireCount = 0;
    private const int MAX_FIRE_COUNT = 10;

    private void Start()
    {
        for (int i = 0; i < _softWallLines.Length; i++)
        {
            for (int j = 0; j < _softWallLines[i].transform.childCount; j++)
            {
                _softWallList.Add(_softWallLines[i].transform.GetChild(j).gameObject);
            }
        }


        if (GameplayManager.I_GameplayManager.PlayerCount > 2)
        {
            for (int i = 0; i < _p3Walls.Length; i++)
            {
                _p3Walls[i].SetActive(false);
                _softWallList.Remove(_p3Walls[i]);
                _softWallInactives.Add(_p3Walls[i]);
            }
        }

        if (GameplayManager.I_GameplayManager.PlayerCount > 3)
        {
            for (int i = 0; i < _p4Walls.Length; i++)
            {
                _p4Walls[i].SetActive(false);
                _softWallList.Remove(_p4Walls[i]);
                _softWallInactives.Add(_p4Walls[i]);
            }
        }

        GenerateObstacles();
    }

    private void GenerateObstacles()
    {
        for (int i = 0; i < _softWallLines.Length; i++)
        {
            int holes = 0;
            if (i == 2 || i == 4 || i == 6)
            {
                int holeRnd = UnityEngine.Random.Range(0, 100);
                if (holeRnd < 15)
                    holes = 1;
                else if (holeRnd < 30)
                    holes = 4;
                else if (holeRnd < 65)
                    holes = 2;
                else
                    holes = 3;
            }
            else
            {
                int holeRnd = UnityEngine.Random.Range(0, 100);
                if (holeRnd < 50)
                    holes = 2;
                else
                    holes = 3;
            }

            List<int> holePos = new List<int>();
            for (int j = 0; j < holes; j++)
            {
                int index = UnityEngine.Random.Range(0, _softWallLines[i].transform.childCount);
                if (!holePos.Contains(index))
                    holePos.Add(index);
                else
                    j--;
            }

            for (int k = 0; k < _softWallLines[i].transform.childCount; k++)
            {
                if (holePos.Contains(k))
                {
                    _softWallLines[i].transform.GetChild(k).gameObject.SetActive(false);
                    _softWallInactives.Add(_softWallLines[i].transform.GetChild(k).gameObject);
                    _softWallList.Remove(_softWallLines[i].transform.GetChild(k).gameObject);
                }
            }
        }

        List<Transform> occupiedSpaces = new List<Transform>();
        while (_bombCount + _fireCount < MAX_BOMB_COUNT + MAX_FIRE_COUNT)
        {
            for (int i = 0; i < _softWallLines.Length; i++)
            {
                int rnd = UnityEngine.Random.Range(0, 100);

                int rndSpace = UnityEngine.Random.Range(0, _softWallLines[i].transform.childCount);

                if (rnd < 33 && _bombCount < MAX_BOMB_COUNT && 
                    !occupiedSpaces.Contains(_softWallLines[i].transform.GetChild(rndSpace)) &&
                    !_softWallInactives.Contains(_softWallLines[i].transform.GetChild(rndSpace).gameObject))
                {
                    _bombItems[_bombCount].transform.position = _softWallLines[i].transform.GetChild(rndSpace).position;
                    _softWallLines[i].transform.GetChild(rndSpace).GetComponent<SoftWall>().AddItem(_bombItems[_bombCount]);
                    occupiedSpaces.Add(_softWallLines[i].transform.GetChild(rndSpace));
                    _bombCount++;
                }
                else if (rnd < 66 && _fireCount < MAX_FIRE_COUNT &&
                    !occupiedSpaces.Contains(_softWallLines[i].transform.GetChild(rndSpace)) &&
                    !_softWallInactives.Contains(_softWallLines[i].transform.GetChild(rndSpace).gameObject))
                {
                    _fireItems[_fireCount].transform.position = _softWallLines[i].transform.GetChild(rndSpace).position;
                    _softWallLines[i].transform.GetChild(rndSpace).GetComponent<SoftWall>().AddItem(_fireItems[_fireCount]);
                    occupiedSpaces.Add(_softWallLines[i].transform.GetChild(rndSpace));
                    _fireCount++;
                }
                else
                {
                    //Nothing happens
                }
            }
        }
    }
}