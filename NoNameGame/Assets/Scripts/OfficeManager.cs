using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeManager : MonoBehaviour
{
    public GameObject officeHolder;

    [Space(20)]
    public int objectsToColor = 1;

    private List<GameObject> colorableObjects = new List<GameObject>();

    private List<GameObject> officeObjectSets = new List<GameObject>();

    private int officePasses = 0;

    private void Start()
    {
        colorableObjects.AddRange(GameObject.FindGameObjectsWithTag("ColorChange"));
        officeObjectSets.AddRange(GameObject.FindGameObjectsWithTag("OfficeSet"));
        //foreach (GameObject set in officeObjectSets) {
        //    Debug.Log(set);
        //}
        officeObjectSets.Sort(delegate(GameObject a, GameObject b)
        {
            return (a.name).CompareTo(b.name);
        });
        //foreach (GameObject set in officeObjectSets)
        //{
        //    Debug.Log(set);
        //}
        //Debug.Log(colorableObjects[0]);
        ChangeOffice();
    }

    public void ChangeAnObject()
    {
        for (int x = objectsToColor; x > 0; x--)
        {
            int randomPick = Random.Range(0, colorableObjects.Count - 1);

            if (colorableObjects[randomPick] != null)
            {
                colorableObjects[randomPick].GetComponent<ColorObject>().ChangeColor();

                colorableObjects.RemoveAt(randomPick);
                Debug.Log(colorableObjects[randomPick]);
            }
        }
    }

    public void HideOffice()
    {
        officeHolder.SetActive(false);
    }

    public void ShowOffice()
    {
        officeHolder.SetActive(true);
    }

    public void ChangeOffice()
    {
        for (int x = 0; x <= (officeObjectSets.Count - 1); x++)
        {
            if (x != officePasses)
            {
                officeObjectSets[x].SetActive(false);
            }
            else
            {
                officeObjectSets[x].SetActive(true);
            }
        }
        officePasses += 1;
    }
}
