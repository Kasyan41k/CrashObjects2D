using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    public float _startSpeed = 75f;
    [SerializeField]
    private float _zoneReaction = 2f;
    [SerializeField]
    private float _slowingDownSpeed = 15f;
    [SerializeField]
    private float _speed;

    private List<GameObject> _carsInScene = new List<GameObject>();

    private GameObject _barrier;
    private float _carSize;
    private bool _needToStop = false;
    private bool _needToSpeedUp = false;
    private bool _needDeleteCar = false;

    private void Start()
    {
        _carSize = gameObject.transform.localScale.x;
        _speed = _startSpeed;
    }

    private void FixedUpdate()
    {
        Vector2 currentPosition = gameObject.transform.position;
        currentPosition.x += _speed * 0.05f / (1f / Time.fixedDeltaTime);
        gameObject.transform.position = currentPosition;
    }

    private void Update()
    {
        if (_needDeleteCar)
            return;

        foreach (var otherCar in _carsInScene)
        {
            if (gameObject != otherCar)
            {
                if (CarIsCrashedInOtherCar(otherCar))
                {
                    otherCar.GetComponent<CarController>().SetDeleteState();
                    SetDeleteState();
                    break;
                }
                if (CarNeedStopInFrontOfTheOtherCar(otherCar) || CarNeedStopInFrontOfTheBarrier())
                {
                    if (!_needToStop) StartCoroutine(Slowing());
                    break;
                }
            }            
            if (CarIsCrashedInBarrier())
            {
                SetDeleteState();
                break;
            }          
            else
            {
                if (_speed == 0)
                {
                    _needToStop = false;
                    if (!_needToSpeedUp) StartCoroutine(SpeedUp());
                }
            }
        }
    }
    // ===> ===> ===>
    private bool CarIsCrashedInOtherCar(GameObject nextCar)
    {
        Vector2 prevoiusCar = gameObject.transform.position;
        Vector2 nextCarPosition = nextCar.transform.position;

        return prevoiusCar.x + _carSize / 2 >= nextCarPosition.x - _carSize / 2 &&
               prevoiusCar.x + _carSize / 2 <= nextCarPosition.x + _carSize / 2;
    }

    private bool CarIsCrashedInBarrier()
    {
        if (_barrier == null)
            return false;

        Vector2 carPosition = gameObject.transform.position;
        Vector2 barrierPosition = _barrier.transform.position;
        float barrierSize = _barrier.transform.localScale.x;

        return carPosition.x + _carSize / 2 >= barrierPosition.x - barrierSize / 2 &&
               carPosition.x + _carSize / 2 <= barrierPosition.x + barrierSize / 2;
    }

    private bool CarNeedStopInFrontOfTheOtherCar(GameObject otherCar)
    {
        Vector2 carPosition = gameObject.transform.position;
        Vector2 otherCarPosition = otherCar.transform.position;

        return carPosition.x + _carSize / 2 <= otherCarPosition.x - _carSize / 2 &&
               carPosition.x + _carSize / 2 + _zoneReaction >= otherCarPosition.x - _carSize / 2;
    }

    private bool CarNeedStopInFrontOfTheBarrier()
    {
        if (_barrier == null)
            return false;

        Vector2 carPosition = gameObject.transform.position;
        Vector2 barrierPosition = _barrier.transform.position;
        float barrierSize = _barrier.transform.localScale.x;

        return carPosition.x + _carSize / 2 <= barrierPosition.x - barrierSize / 2 &&
               carPosition.x + _carSize / 2 + _zoneReaction >= barrierPosition.x - barrierSize / 2;
    }

    public void SetDeleteState()
    {
        _startSpeed = 0;
        _speed = 0;
        _needDeleteCar = true;
        StartCoroutine(DeleteCar());
    }

    private IEnumerator DeleteCar()
    {
        yield return new WaitForSeconds(3f);

        _carsInScene.Remove(gameObject);
        Destroy(gameObject);
    }

    public IEnumerator Slowing()
    {
        _needToStop = true;
        yield return new WaitForSeconds(0.2f);

        while (_needToStop)
        {
            SlowAndSpeedUp();
            yield return new WaitForSeconds(0.02f);
        }
    }

    public IEnumerator SpeedUp()
    {
        _needToSpeedUp = true;
        yield return new WaitForSeconds(0.2f);

        while (_needToSpeedUp)
        {
            SlowAndSpeedUp();
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void SlowAndSpeedUp()
    {
        float slowingDownSpeed = _slowingDownSpeed / (1f / 0.02f);

        if (_needToStop)
        {
            if (_speed - slowingDownSpeed <= 0)
            {
                _speed = 0;
                _needToStop = false;
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
                _needToSpeedUp = false;
            }
            else
            {
                _speed += slowingDownSpeed;
            }
        }
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
