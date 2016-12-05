using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

public class Labirint : MonoBehaviour {

    private  List<List<int>> labirint = new List<List<int>>();
    public string text;
    public GameObject wallPrefab;
    public GameObject finishPrefab;

	// Use this for initialization
	void Start () {
        GetLabirint();
        InitWall();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Получение лабиринта из файла
    /// </summary>
    void GetLabirint()
    {
        List<string> tmp = File.ReadAllLines(text).ToList();
        foreach (var s in tmp)
        {
            labirint.Add(new List<int>());
            for (int i = 0; i < s.Length; i++)
            {
                labirint[labirint.Count - 1].Add(Int32.Parse(s[i].ToString()));
            }
        }
    }

    /// <summary>
    /// Инициализации стен
    /// </summary>
    void InitWall()
    {
        for (int i = 0; i < labirint.Count; i++)
        {
            for (int j = 0; j < labirint[i].Count; j++)
            {
                if (labirint[i][j] == 1)
                {
                    GameObject init = (GameObject)Instantiate(wallPrefab, new Vector2(i,labirint[i].Count-1- j), transform.rotation);
                    init.transform.parent = transform;
                }

                if (labirint[i][j] == 3)
                {
                    GameObject init = (GameObject)Instantiate(finishPrefab, new Vector2(i, labirint[i].Count - 1 - j), transform.rotation);
                    init.transform.parent = transform;
                }
            }
        }
    }
}
