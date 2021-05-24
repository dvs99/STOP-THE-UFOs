using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggableUITower : MonoBehaviour
{
    [SerializeField] private float dragRescaling;
    [SerializeField] private Color dragUnplazableColor;
    [SerializeField] private Material dragUnplazableMaterial;
    [SerializeField] private Material plazableMaterial;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float towerSize;
    [SerializeField] private int groundLayerNumber;
    [SerializeField] private int obstacleLayerNumber;

    private RectTransform draggingUICopy;
    private Transform draggingModelCopy;
    private Transform draggingModelRange;
    private bool validPosition = false;
    private int price;

    private Material[] towerPlazableMaterials;
    private Material[] towerUnplazableMaterials;
    private bool setChildMaterials;
    private Material[] towerChildPlazableMaterials;
    private Material[] towerChildUnplazableMaterials;

    private void Start()
    {
        generateImage();
        generateRange();
        draggingModelCopy = Instantiate(prefab, Vector3.zero, prefab.transform.rotation).transform;

        price = prefab.GetComponent<TowerShooting>().Price;

        draggingUICopy.gameObject.SetActive(false);
        draggingModelCopy.gameObject.SetActive(false);
        draggingModelRange.gameObject.SetActive(false);

        towerPlazableMaterials = prefab.GetComponent<MeshRenderer>().sharedMaterials;
        towerUnplazableMaterials = new Material[towerPlazableMaterials.Length];
        for (int i = 0; i < towerUnplazableMaterials.Length; i++)
            towerUnplazableMaterials[i] = dragUnplazableMaterial;

        if (prefab.transform.childCount > 0 && prefab.transform.GetChild(0).GetComponent<MeshRenderer>() != null)
        {
            setChildMaterials = true;
            towerChildPlazableMaterials = prefab.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials;
            towerChildUnplazableMaterials = new Material[towerChildPlazableMaterials.Length];
            for (int i = 0; i < towerChildUnplazableMaterials.Length; i++)
                towerChildUnplazableMaterials[i] = dragUnplazableMaterial;
        }
        else
            setChildMaterials = false;
    }

    private void generateRange()
    {
        draggingModelRange = GameObject.CreatePrimitive(PrimitiveType.Cylinder).transform;
        Destroy(draggingModelRange.GetComponent<Collider>());
        draggingModelRange.GetComponent<MeshRenderer>().material = dragUnplazableMaterial;
        float diameter = prefab.GetComponent<TowerShooting>().Range * 2;
        draggingModelRange.localScale = new Vector3(diameter, 0.01f, diameter);
    }

    private void generateImage()
    {
        Image newImage = new GameObject().AddComponent<Image>();
        newImage.sprite = GetComponent<Image>().sprite;
        newImage.color = dragUnplazableColor;
        draggingUICopy = newImage.GetComponent<RectTransform>();
        draggingUICopy.SetParent(transform.parent);
        draggingUICopy.sizeDelta = GetComponent<RectTransform>().sizeDelta;
        draggingUICopy.localScale = GetComponent<RectTransform>().localScale * dragRescaling;
    }


    public void startDrag()
    {
        draggingUICopy.position = Input.mousePosition;
        draggingUICopy.gameObject.SetActive(true);
    }

    public void endDrag()
    {
        if (draggingModelCopy.gameObject.activeSelf && validPosition)
        {
            moneyManager.Pay(price);
            SphereCollider towerObstacle = new GameObject().AddComponent<SphereCollider>();
            towerObstacle.radius = towerSize;
            towerObstacle.gameObject.layer = obstacleLayerNumber;
            towerObstacle.transform.position = draggingModelCopy.transform.position;
            towerObstacle.transform.parent = draggingModelCopy;
            draggingModelCopy.GetComponent<TowerShooting>().StartRunning(enemySpawner, draggingModelRange.gameObject);
            draggingModelCopy = Instantiate(prefab, Vector3.zero, prefab.transform.rotation).transform;
            generateRange();
        }

        draggingUICopy.gameObject.SetActive(false);
        draggingModelCopy.gameObject.SetActive(false);
        draggingModelRange.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (draggingUICopy.gameObject.activeSelf || draggingModelCopy.gameObject.activeSelf)
        {
            draggingUICopy.position = Input.mousePosition;

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(draggingUICopy.position) - Camera.main.transform.forward * 100f;
            RaycastHit hit;
            if (Physics.Raycast(worldPos, Camera.main.transform.forward, out hit, Mathf.Infinity, 1 << groundLayerNumber))
            {
                Debug.DrawRay(worldPos, Camera.main.transform.forward * hit.distance, Color.yellow);
                draggingUICopy.gameObject.SetActive(false);

                Vector3 finalPos = worldPos + Camera.main.transform.forward * hit.distance;
                draggingModelCopy.position = finalPos;
                draggingModelRange.position = finalPos;
                draggingModelCopy.gameObject.SetActive(true);

                if (moneyManager.CanAfford(price) && Physics.OverlapSphere(finalPos, towerSize, 1 << obstacleLayerNumber).Length == 0)
                {
                    validPosition = true;
                    draggingModelRange.GetComponent<MeshRenderer>().material = plazableMaterial;
                    draggingModelCopy.GetComponent<MeshRenderer>().materials = towerPlazableMaterials;
                    if (setChildMaterials)
                        draggingModelCopy.GetChild(0).GetComponent<MeshRenderer>().materials = towerChildPlazableMaterials;
                }
                else
                {
                    validPosition = false;
                    draggingModelRange.GetComponent<MeshRenderer>().material = dragUnplazableMaterial;
                    draggingModelCopy.GetComponent<MeshRenderer>().materials = towerUnplazableMaterials;
                    if (setChildMaterials)
                        draggingModelCopy.GetChild(0).GetComponent<MeshRenderer>().materials = towerChildUnplazableMaterials;
                }
                draggingModelRange.gameObject.SetActive(true);
            }
            else
            {
                draggingModelCopy.gameObject.SetActive(false);
                draggingModelRange.gameObject.SetActive(false);
                draggingUICopy.gameObject.SetActive(true);
            }
        }
    }
}
