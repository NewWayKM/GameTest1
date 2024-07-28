using UnityEngine;
using UnityEngine.UI; 

public class ImageTimer : MonoBehaviour // Используется для отслеживания тиков и выдача try,false от Tick в scripts GameManager
{
    public float MaxTime; // устанавливаю время для таймеров #здесь неудобно, в след раз перенести к остальным настройкам
    public bool Tick; // отслеживание прохождения таймера

    private float currentTime; // отслеживание времени во внутренем цикле
    private Image img;

    void Start()
    {
        img = GetComponent<Image>();
        currentTime = MaxTime;
    }

    void Update()
    {
        Tick = false;
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            Tick = true;
            currentTime = MaxTime;
        }

        if (img != null) // проверяет назначили ли в инспекторе sprite
        {
            img.fillAmount = currentTime / MaxTime;
        }

    }


}