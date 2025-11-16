using UnityEngine;
using UnityEngine.UI;
public class GUITimer : MonoBehaviour
{
    private Slider mySlider;
    [SerializeField] private Slider sliderPrefab;
    [SerializeField] public taskEnum taskStationId;
    [SerializeField] private GameObject parent;
    private Task myTask;
    private bool created;

    private void Start()
    {
        mySlider = Instantiate(sliderPrefab, transform);
        mySlider.gameObject.SetActive(false);

    }
    public void Initialize(Task task)
    {
        myTask = task;
        //if (parent == null) parent = gameObject;
        //mySlider = Instantiate(sliderPrefab, transform);
        mySlider.gameObject.SetActive(true);
        mySlider.value = 1;
        mySlider.minValue = 0;
        mySlider.maxValue = 1;

        created = true;
    }

    public void OnRemove()
    {
        created = false;
        mySlider.gameObject.SetActive(false);
        //Destroy(mySlider.gameObject);
    }

    void Update()
    {
        if (created && mySlider != null)
        {
            //mySlider.value = myTask.timer.Ratio;

        }
    }
}
