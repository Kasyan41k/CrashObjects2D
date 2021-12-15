using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _road;
    [SerializeField]
    private GameObject _carPrefab;
    [SerializeField]
    private float _beforeRoadDistance;
    [SerializeField]
    private float _latencyForSpawnCars;

    public List<GameObject> _carsInScene = new List<GameObject>();
    private GameObject _barrierInScene;

    private Vector3 _zoneSpawn;
    private int _countCarsForSpawn = 0;

    private void Start()
    {
        _zoneSpawn = new Vector3(_road.transform.position.x - _road.transform.localScale.x / 2 - _beforeRoadDistance,
                     _carPrefab.transform.position.y, _carPrefab.transform.position.z);
        _barrierInScene = gameObject.GetComponent<BarrierSpawner>().GetBarrierInScene();
    }

    private void Update()
    {
        if (_countCarsForSpawn == 0 && _carsInScene.Count == 0)
        {
            _countCarsForSpawn = Random.Range(3, 6);
            StartCoroutine(SpawnCar());
        }
    }

    private IEnumerator SpawnCar()
    {
        if (_countCarsForSpawn != 0)
        {
            GameObject currentCar = Instantiate(_carPrefab, _zoneSpawn, _carPrefab.transform.rotation);
            currentCar.GetComponent<CarController>().TakeCarsInScene(_carsInScene);
            _carsInScene.Add(currentCar);

            _countCarsForSpawn--;

            yield return new WaitForSeconds(_latencyForSpawnCars);

            StartCoroutine(SpawnCar());
        }
    }

    public List<GameObject> GetCarsInScene()
    {
        return _carsInScene;
    }
}
