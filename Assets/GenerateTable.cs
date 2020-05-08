using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;


public class GenerateTable : MonoBehaviour
{
    [SerializeField] private GameObject _prefabLetter;
    [SerializeField] private GameObject _symbolsGroup;
    [SerializeField] private InputField _columnsCountText;
    [SerializeField] private InputField _stringsCountText;

    private int ColumnsCount;
    private int StringCount;

    public GameObject[] allLetters;


    public void GenerateGridElements()
    {
        DeleateGridElements();
        ChangingSizeLetterForGrid();
        SpawnGridElements();
    }

    private static void DeleateGridElements()
    {
        GameObject[] LetterList = GameObject.FindGameObjectsWithTag("Letter");
        int letterListLength = LetterList.Length;

        if (LetterList != null)
        {
            while (--letterListLength >= 0)
            {
                GameObject gameObject = LetterList[letterListLength];
                Destroy(gameObject);
            }
        }

    }

    private void ChangingSizeLetterForGrid()
    {
        const int maxSizeOfGrid = 12;
        
        int columnsCount;
        int stringCount;
        bool successColumnParse = Int32.TryParse(_columnsCountText.text, out columnsCount);
        bool successStringParse = Int32.TryParse(_stringsCountText.text, out stringCount);

        if (successColumnParse)
        {
            ColumnsCount = columnsCount;
        }

        if (successStringParse)
        {
            StringCount = stringCount;
        }
        
        if (ColumnsCount > maxSizeOfGrid || StringCount > maxSizeOfGrid)
        {
            NormalizeGridToAverage(maxSizeOfGrid);
        }


        float width = _symbolsGroup.GetComponent<RectTransform>().rect.width;
        Vector2 newSize = new Vector2(width / ColumnsCount, width / ColumnsCount);
        var obj = _symbolsGroup.GetComponent<GridLayoutGroup>();
        obj.cellSize = newSize;
    }

    private void NormalizeGridToAverage(int maxSizeOfGrid)
    {
        _stringsCountText.text = (maxSizeOfGrid.ToString());
        _columnsCountText.text = (maxSizeOfGrid.ToString());
        StringCount = maxSizeOfGrid;
        ColumnsCount = maxSizeOfGrid;
    }

    private void SpawnGridElements()
    {
        var grid = TransformLetter(out var str);

        for (int i = 0; i < allLetters.Length; i++)
        {
            allLetters[i] = Instantiate(_prefabLetter, grid);
            string SortedLetter = str[UnityEngine.Random.Range(0, 26)].ToString();
            allLetters[i].GetComponent<Text>().text = SortedLetter; 

        }
    }

    private Transform TransformLetter(out char[] str)
    {
        int CountOfElements = ColumnsCount * StringCount;

        allLetters = new GameObject [CountOfElements];
        var grid = _symbolsGroup.transform;
        Random random = new Random();
        char[] letters = Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char) c).ToArray();
        str = letters.OrderBy(x => random.Next()).ToArray();
        return grid;
    }


    public void ShaffleLetters()
    {
        int CountOfElements = ColumnsCount * StringCount;

        for (int i = 0; i < allLetters.Length; i++)
        {
            allLetters[i].transform.SetSiblingIndex(UnityEngine.Random.Range(0, CountOfElements-1));
        }
    }


}
