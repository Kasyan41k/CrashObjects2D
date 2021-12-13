using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject _carSpawner;
    [SerializeField]
    private GameObject _carPrefab;
    [SerializeField]
    private float _zoneReaction = 2f;
    [SerializeField]
    private float _slowingDownSpeed = 15f;

    private List<GameObject> _carsInScene;
    private Dictionary<GameObject, float> _slowingDownCars;
    private float _carSize;
    private float _speed;
    private float _startSpeed;
    private float _counter = 0f;
    private bool _needToStop = false;
    private bool _needToSpeedUp = false;

    private void Start()
    {
        _carsInScene = _carSpawner.GetComponent<CarSpawner>().GetCarsInScene();
        _startSpeed = _carPrefab.GetComponent<CarController>().StartSpeed;
        _speed = _startSpeed;
        _carSize = _carPrefab.transform.localScale.x;      
    }

    private void FixedUpdate()
    {
        foreach(var car in _slowingDownCars)
        {
            if (car.Value < 0.2f && _needToStop)
            {
                _slowingDownCars[car.Key] += Time.fixedDeltaTime;
                Debug.Log($"{_counter} | {Time.fixedDeltaTime}");
            }
            else
            {
                SlowAndSpeedUp(car.Key);
            }
        }
    }

    private void Update()
    {
        int i = 0;
        bool fuck = false;
        foreach (var otherCar in _carsInScene)
        {
            if (CarNeedStopInFrontOfTheOtherCar(otherCar))
            {
                _needToStop = true;
                break;
            }
            else { if (i == _carsInScene.Count - 1) fuck = true; }
            i++;
        }
        if (_needToStop && fuck == true)
        {
            _needToStop = false;
            _counter = 0f;
            _needToSpeedUp = true;
        }
    }

    private void SlowAndSpeedUp(GameObject car)
    {
        float slowingDownSpeed = _slowingDownSpeed / (1f / Time.fixedDeltaTime);

        if (_needToStop)
        {
            if (_speed - slowingDownSpeed <= 0)
            {
                _speed = 0;
            }
            else
            {
                _speed -= slowingDownSpeed;
            }
        }
        else if (_needToSpeedUp)
        {
            if (_speed + slowingDownSpeed >= _startSpeed)
            {
                _speed = _startSpeed;

            }
            else
            {
                _speed += slowingDownSpeed;
            }
            if (_speed + slowingDownSpeed == _startSpeed)
            {
                _needToSpeedUp = false;
            }
        }
    }

    private bool CarNeedStopInFrontOfTheOtherCar(GameObject otherCar)
    {
        Vector2 carPosition = gameObject.transform.position;
        Vector2 otherCarPosition = otherCar.transform.position;

        return carPosition.x + _carSize / 2 + _zoneReaction >= otherCarPosition.x - _carSize / 2 &&
               carPosition.x + _carSize / 2 + _zoneReaction <= otherCarPosition.x + _carSize / 2;
    }
}
