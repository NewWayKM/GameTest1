using TMPro;
using UnityEngine;

public class GameTimeTracker : MonoBehaviour // отслеживает время от начал игры
{
    public TMP_Text gameTimeText; // Ссылка на текстовый элемент для отображения времени
    private float elapsedTime; // Переменная для хранения прошедшего времени

    void Start()
    {
        elapsedTime = 0f;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime; // Обновление прошедшего времени

        // Преобразование прошедшего времени в формат ЧЧ:ММ:СС
        int hours = Mathf.FloorToInt(elapsedTime / 3600);
        int minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        // Обновление текстового элемента
        gameTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
}