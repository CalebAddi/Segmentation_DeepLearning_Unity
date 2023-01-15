using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    public ImageSynthesis synthesis;
    public GameObject[] prefabs;
    private GameObject[] capacity;

    [Header("Object Spawn Settings")]
    public int minObj;
    public int maxObj;
    public int nextActTime;
    public int timePeriod;

    [Header("Image Capture Settings")]
    public int trainingImages;
    public int validationImages;
    public bool grayscale;  //Logic not setup yet for the initial commit
    

    private void Start() 
    {
        capacity = new GameObject[maxObj];
    }

    private void Update() 
    {
        SynthesisContainer();
        TimesToGenerate();
    }

    private void SynthesisContainer()
    {
        if (nextActTime < trainingImages + validationImages)
        {
            Debug.Log($"Image Captured: {nextActTime}");

            if (nextActTime < trainingImages)
            {
                string filename = $"image_{nextActTime.ToString().PadLeft(5, '0')}";
                synthesis.Save(filename, 600, 600, "ImageContainer/train", 2);
            }
            else if (nextActTime < trainingImages + validationImages)
            {
                int valFrameCount = nextActTime - trainingImages;
                string filename = $"image_{valFrameCount.ToString().PadLeft(5, '0')}";
                synthesis.Save(filename, 600, 600, "ImageContainer/valImage", 2);
            }
        }
    }

    private void GenerateRandom() 
    {
        int objMinToMax = Random.Range(minObj, maxObj);

        for (int i = 0; i < capacity.Length; i++)
        {
            if (capacity[i] != null)
            {
                Destroy(capacity[i]);
            }
        }

        for (int i = 0; i < objMinToMax; i++)
        {
            // Pick a random Element from prefabs array
            int prefabIdx = Random.Range(0, prefabs.Length);
            GameObject prefab = prefabs[prefabIdx];

            // Pick random position
            float posX, posY, posZ;
            posX = Random.Range(-10f, 10f);
            posY = Random.Range(4f, 15f);
            posZ = Random.Range(-8f, 10f);

            Vector3 newPos = new Vector3(posX, posY, posZ);

            // Add the new Rotation
            var newRotation = Random.rotation;
            var newObj = Instantiate(prefab, newPos, newRotation);
            capacity[i] = newObj;

            // Scale of objects
            float scaleFact = Random.Range(0.5f, 2f);
            Vector3 newScale = new Vector3(scaleFact, scaleFact, scaleFact);
            newObj.transform.localScale = newScale;

            //Color of objects
            float colorR, colorG, colorB;
            colorR = Random.Range(0f, 1f);
            colorG = Random.Range(0f, 1f);
            colorB = Random.Range(0f, 1f);
            var newColor = new Color(colorR, colorB, colorG);
            newObj.GetComponent<Renderer>().material.color = newColor;
        }
    }

    private void TimesToGenerate()
    {
        if (Time.time > nextActTime)
        {
            nextActTime += timePeriod;
            GenerateRandom();
        }
        synthesis.OnSceneChange();
    }
}
