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

    private List<GameObject> _carsInScene;
    private float _carSize;
    [SerializeField]
    private float Speed;
    private float _counter = 0f;
    private bool _needToStop = false;
    private bool _needToSpeedUp = false;


    private void Start()
    {
        _carSize = gameObject.transform.localScale.x;
        Speed = StartSpeed;
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
        currentPosition.x += Speed * 0.05f / (1f / Time.fixedDeltaTime);
        gameObject.transform.position = currentPosition;
    }

    private void Update()
    {
        foreach (var otherCar in _carsInScene)
        {
            if (CarNeedStopInFrontOfTheOtherCar(otherCar))
            {
                _needToStop = true;
                break;
            }
            else
            {
                if (Speed == 0)
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
            if (Speed - slowingDownSpeed <= 0)
            {
                Speed = 0;
            }
            else
            {
                Speed -= slowingDownSpeed;
            }
        }
        else if (_needToSpeedUp)
        {
            if (Speed + slowingDownSpeed >= StartSpeed)
            {
                Speed = StartSpeed;

            }
            else
            {
                Speed += slowingDownSpeed;
            }
            if (Speed + slowingDownSpeed == StartSpeed)
            {
                _needToSpeedUp = false;
            }
        }
    }

    private bool CarNeedStopInFrontOfTheOtherCar(GameObject otherCar)
    {
        Vector2 carPosition = gameObject.transform.position;
        Vector2 otherCarPosition = otherCar.transform.position;

        float positionCarWithZoneReaction = carPosition.x + _carSize + _zoneReaction;

        //return positionCarWithZoneReaction >= otherCarPosition.x - _carSize / 2 &&
        //        positionCarWithZoneReaction <= otherCarPosition.x + _carSize;
        //return carPosition.x + _carSize / 2 + _zoneReaction >= otherCarPosition.x - _carSize / 2 &&
        //       carPosition.x + _carSize / 2 + _zoneReaction <= otherCarPosition.x + _carSize / 2;

        return carPosition.x + _carSize / 2 <= otherCarPosition.x - _carSize / 2 &&
               carPosition.x + _carSize / 2 + _zoneReaction >= otherCarPosition.x - _carSize / 2;
    }

    //
    private bool CarNeedStopInFrontOfTheBarrier(GameObject otherCar)
    {
        return true;
    }

    public void TakeCarsInScene(List<GameObject> carsInScene)
    {
        _carsInScene = carsInScene;
    }

    // дальше треш который не используется
    private IEnumerator WaitReaction()
    {
        yield return new WaitForSeconds(0.2f);
    }

    private IEnumerator Slow()
    {
        if (_needToStop)
        {
            if (Speed - _slowingDownSpeed <= 0)
            {
                Speed = 0;
            }
            else
            {
                Speed -= _slowingDownSpeed;
                yield return new WaitForSeconds(1f);
                StartCoroutine(Slow());
            }
        }
    }


    private IEnumerator SpeedUp()
    {
        if (_needToSpeedUp)
        {
            if (Speed + _slowingDownSpeed >= StartSpeed)
            {
                Speed = StartSpeed;
                _needToSpeedUp = false;
            }
            else
            {
                Speed += StartSpeed;
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(SpeedUp());
            }
        }
    }
}
