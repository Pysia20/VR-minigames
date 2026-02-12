using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FlasksManager : MonoBehaviour
{
    private int colorAmount;
    private int emptyFlasksAmount;
    private int fullFlasksAmount;
    public GameObject flaskPrefab;
    public GameObject flaskSpawner;
    List<Transform> flaskSpawners = new();

    public List<Color> colors = new();

    private List<GameObject> flasks = new();

    private Dictionary<Color, int> colorCounters = new();

    public UnityEvent handEnableCollider;
    public UnityEvent handDisableCollider;

    //TODO fizyczne rece

    void Start()
    {
        colorAmount = colors.Count();
        fullFlasksAmount = colorAmount;
        emptyFlasksAmount = 2;

        resetColorsCounter();

        if (flaskSpawner)
        { 
            bool skipFirst = true;
            foreach(Transform child in flaskSpawner.GetComponentsInChildren<Transform>())
            {
                if (skipFirst)
                {
                    skipFirst = false;
                    continue;
                }
                flaskSpawners.Add(child);
            }

            generateFlasksSpawners();
        }
        else
        {
            generateFlasks();   
        }
    }

    void resetColorsCounter()
    {
        for(int i = 0; i < colors.Count; i++){
            colorCounters[colors[i]] = 4;
        }
    }

    void clearFlasks()
    {
        foreach(GameObject f in flasks)
        {
            Destroy(f);
        }

        resetColorsCounter();
    }

    void generateFlasks()
    {
        clearFlasks();

        int flaskOffset = 3;
        for(int i = 0 ; i < emptyFlasksAmount; i++)
        {
            GameObject a = Instantiate(flaskPrefab);
            a.transform.position = new Vector3(0, 2, flaskOffset * i + 1);

            a.GetComponent<CustomOnGrab>().grabEvent = handDisableCollider;
            a.GetComponent<CustomOnGrab>().releaseEvent = handEnableCollider;

            flasks.Add(a);
        }

        for(int i = 0 ; i < fullFlasksAmount; i++)
        {
            GameObject a = Instantiate(flaskPrefab);
            a.transform.position = new Vector3(2, 2, flaskOffset * i + 1);
            a.GetComponent<Flask>().generateRandomColors(new(colors), colorCounters);

            a.GetComponent<CustomOnGrab>().grabEvent = handDisableCollider;
            a.GetComponent<CustomOnGrab>().releaseEvent = handEnableCollider;

            flasks.Add(a);
        }
    }

    public void generateFlasksSpawners()
    {
        clearFlasks();

        if(flaskSpawners.Count < fullFlasksAmount + emptyFlasksAmount)
        {
            Debug.LogError($"Too little spawners for this amount of flasks (S: {flaskSpawners.Count}, F: {fullFlasksAmount}, E: {emptyFlasksAmount})");
            return;
        }

        int spawnerIndex = 0;

        for(int i = 0 ; i < emptyFlasksAmount; i++)
        {
            GameObject a = Instantiate(flaskPrefab);
            a.transform.position = flaskSpawners[spawnerIndex].position;

            a.GetComponent<CustomOnGrab>().grabEvent = handDisableCollider;
            a.GetComponent<CustomOnGrab>().releaseEvent = handEnableCollider;

            flasks.Add(a);
            spawnerIndex++;
        }

        for(int i = 0 ; i < fullFlasksAmount; i++)
        {
            GameObject a = Instantiate(flaskPrefab);
            a.transform.position = flaskSpawners[spawnerIndex].position;
            a.GetComponent<Flask>().generateRandomColors(new(colors), colorCounters);

            a.GetComponent<CustomOnGrab>().grabEvent = handDisableCollider;
            a.GetComponent<CustomOnGrab>().releaseEvent = handEnableCollider;

            flasks.Add(a);
            spawnerIndex++;
        }
    }
}
