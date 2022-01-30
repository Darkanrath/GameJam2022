using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindTasks : MonoBehaviour
{
    bool taskCompleted;
    public Text[] taskList;
    public GameObject taskObject;
    // Start is called before the first frame update
    void Start()
    {
        taskCompleted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (taskCompleted)
        {
            for(int i = 0; i < taskList.Length; i++)
            {
                taskList[i].text.ToString().ToLower().Equals(taskObject.name);
                taskList[i].text = "Task Completed";
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKey(KeyCode.E))
        {
            taskCompleted = true;
        }
    }
}
