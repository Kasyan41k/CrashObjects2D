using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _road;
    private List<GameObject> _carsInScene;


    void Start()
    {
        _carsInScene = gameObject.GetComponent<CarSpawner>().GetCarsInScene();
    }

    
    void Update()
    {
        foreach(var car in _carsInScene)
        {
            if(car.transform.position.x > _road.transform.position.x + _road.transform.localScale.x / 2f + 3f)
            {
                Destroy(car);
                _carsInScene.Remove(car);
            }
        }
    }
}
