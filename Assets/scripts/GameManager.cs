using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _road;
    [SerializeField]
    private Text _endGameStatistics;
    [SerializeField]
    private GameObject _statistics;
    [SerializeField]
    private GameObject _buttonRestart;
    private List<GameObject> _carsInScene;
    private int countSuccessfulCars;

    void Start()
    {
        StartCoroutine(StopGame());
        _carsInScene = gameObject.GetComponent<CarSpawner>().GetCarsInScene();
    }

    
    private void Update()
    {
        List<GameObject> carsForDelete = new List<GameObject>();

        foreach (var car in _carsInScene)
        {            
            if ((car.transform.position.x > _road.transform.position.x + _road.transform.localScale.x / 2f + 3f))
            {
                carsForDelete.Add(car);
                countSuccessfulCars++;
            }
        }

        foreach(var car in carsForDelete)
        {
            _carsInScene.Remove(car);
            Destroy(car);
        }
    }

    private IEnumerator StopGame()
    {
        yield return new WaitForSeconds(60f);

        Time.timeScale = 0f;

        _statistics.SetActive(true);
        _buttonRestart.SetActive(true);
        _endGameStatistics.text += countSuccessfulCars + "cars\nsuccessful\ndrove";
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
