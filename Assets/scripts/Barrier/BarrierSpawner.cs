using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _barrierPrefab;
    [SerializeField]
    private float _distanceAfterCar;

    private List<GameObject> _carsInScene;
    private GameObject _barrierInScene = null;

    public GameObject GetBarrierInScene()
    {
        return _barrierInScene;
    }

    private void Start()
    {
        _carsInScene = gameObject.GetComponent<CarSpawner>().GetCarsInScene();
    }

    private void Update()
    {
        Spawn();
    }

    public void Spawn()
    {
        if (_carsInScene == null || _barrierInScene != null)
            return;

        foreach (var car in _carsInScene)
        {
            if (Random.Range(1,11) > 5)
            {
                Vector3 spawnPosition = new Vector3(car.transform.position.x + _distanceAfterCar,
                                                    car.transform.position.y,
                                                    car.transform.position.z);

                _barrierInScene = Instantiate(_barrierPrefab, spawnPosition, _barrierPrefab.transform.rotation);

                UpdateCarsInfoByBarrier();

                StartCoroutine(StartRemoveBarrier());
                break;
            }
        }
    }

    private IEnumerator StartRemoveBarrier()
    {
        yield return new WaitForSeconds(5f);

        Destroy(_barrierInScene);

        _barrierInScene = null;
    }

    private void UpdateCarsInfoByBarrier()
    {
        foreach(var car in _carsInScene)
        {
            car.GetComponent<CarController>().TakeBarrierInScene(_barrierInScene);
        }
    }
}
