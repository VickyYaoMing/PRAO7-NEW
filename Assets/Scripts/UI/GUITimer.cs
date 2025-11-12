using UnityEngine;
using UnityEngine.UI;
public class GUITimer : MonoBehaviour
{
    private Slider mySlider;
    [SerializeField] private Slider sliderPrefab;
    [SerializeField] public int taskStationId;
    [SerializeField] private GameObject parent;
    private Task myTask;
    private bool created;

    public void Initialize(Task task)
    {
        myTask = task;
        //if (parent == null) parent = gameObject;
        mySlider = Instantiate(sliderPrefab, transform);
        mySlider.value = task.timer.WaitTime;
        created = true;
    }

    public void OnDestroy()
    {
        created = false;
        Destroy(mySlider.gameObject);
    }

    void Update()
    {
        if (created && mySlider != null)
        {
            mySlider.value = myTask.timer.Ratio;

        }
    }
}
