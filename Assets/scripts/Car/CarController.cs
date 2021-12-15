using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    public float StartSpeed = 75f;
    [SerializeField]
    private float _zoneReaction = 2f;
    [SerializeField]
    private float _slowingDownSpeed = 15f;
    [SerializeField]
    private float _speed;

    private List<GameObject> _carsInScene = new List<GameObject>();

    private GameObject _barrier;
    private float _carSize;
    private float _counter = 0f;
    private bool _needToStop = false;
    private bool _needToSpeedUp = false;

    private void Start()
    {
        _carSize = gameObject.transform.localScale.x;
        _speed = StartSpeed;
    }

    private void FixedUpdate()
    {
        if (_counter < 0.2f && _needToStop)
        {
            _counter += Time.fixedDeltaTime;           
        }
        else
        {
            SlowAndSpeedUp();
        }

        Vector2 currentPosition = gameObject.transform.position;
        currentPosition.x += _speed * 0.05f / (1f / Time.fixedDeltaTime);
        gameObject.transform.position = currentPosition;
    }

    private void Update()
    {
        foreach (var otherCar in _carsInScene)
        {
            if (CarNeedStopInFrontOfTheOtherCar(otherCar) || CarNeedStopInFrontOfTheBarrier(_barrier))
            {
                _needToStop = true;
                break;
            }
            else
            {
                if (_speed == 0)
                {
                    _needToStop = false;
                    Debug.Log($"{_counter}");
                    _counter = 0f;
                    _needToSpeedUp = true;
                }
            }
        }
    }

    private void SlowAndSpeedUp()
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
            if (_speed + slowingDownSpeed >= StartSpeed)
            {
                _speed = StartSpeed;

            }
            else
            {
                _speed += slowingDownSpeed;
            }
            if (_speed + slowingDownSpeed == StartSpeed)
            {
                _needToSpeedUp = false;
            }
        }
    }

    private bool CarNeedStopInFrontOfTheOtherCar(GameObject otherCar)
    {
        Vector2 carPosition = gameObject.transform.position;
        Vector2 otherCarPosition = otherCar.transform.position;

        return carPosition.x + _carSize / 2 <= otherCarPosition.x - _carSize / 2 &&
               carPosition.x + _carSize / 2 + _zoneReaction >= otherCarPosition.x - _carSize / 2;
    }

    private bool CarNeedStopInFrontOfTheBarrier(GameObject barrier)
    {
        if (barrier == null)
            return false;

        Vector2 carPosition = gameObject.transform.position;
        Vector2 barrierPosition = barrier.transform.position;
        float barrierSize = barrier.transform.localScale.x;

        return carPosition.x + _carSize / 2 <= barrierPosition.x - barrierSize / 2 &&
               carPosition.x + _carSize / 2 + _zoneReaction >= barrierPosition.x - barrierSize / 2;
    }

    public void TakeCarsInScene(List<GameObject> carsInScene)
    {
        _carsInScene = carsInScene;
    }

    public void TakeBarrierInScene(GameObject barrier)
    {
        _barrier = barrier;
    }
}
