using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public ImageTimer HarvestTimer; // таймер производства пшеницы, собственного класса к значеним которого можно обращаться через .Tick и т.д.
    public ImageTimer EatingTimer;  // таймер потребления пшеницы -//-

    public Image PeasantTimerImg;   // Изображение таймера производства крестьянина, #в след раз сохранять логику таймер-таймер тулбар-тулбар, путаница в названиях
    public Image WarrioirTimerImg;  // Изображение таймера производства воина
    public Image RaidTimerImg;      // Изображение таймера рейда

    public Button peasantButton;
    public Button warriorButton;

    public TMP_Text resourcesTextVillage;  // текст ресурсов где есть несколько параметров для деревни
    public TMP_Text resourcesTextRaid;     // текст ресурсов где есть несколько параметров для рейда

    public int peasantCount;        // Стартовые значения кол-ва крестьян
    public int warriorCount;        // Стартовые значения кол-ва воинов
    public int wheatCount;          // Стартовые значения кол-ва пшеницы

    public int wheatPerPeasant;     // Производство пшеницы одним крестьянином
    public int wheatToWarriors;     // Потребление пшеницы одним воином

    public int peasantCost;         // стоимость одного крестьянина
    public int warriorCost;         // стоимость одного воина

    public float peasantCreateTime; // время создания крестьянина
    public float warriorCreateTime; // время создания воина

    public float raidMaxTime;       // Время цикла рейда
    public int raidIncrease;      // На сколько увеличивается след. рейд
    public int nextRaid;          // Количество противником в след. волне
    public int saveWave;          // Количество волн без набегов

    public GameObject GameOverScreen; // ссылка на canvas/panel

    private float peasantTimer = -2;    // таймер производства крестьянина
    private float warriorTimer = -2;    // таймер производства война
    private float raidTimer;            // таймер производства у рейда 

    private int countingWheat;          // Подсчет всей произведенной пщеницы
    private int countingEnemy;          // Подсчет всех врагов участвовавщих в игре
    private int currentRaidCount;       // количество врагов в текущей волне


    private bool isHiringPeasant = false; // Флаг для блокировки
    private bool isHiringWarrior = false; // Флаг для блокировки

    public GameObject PausePanel; // для меню паузы
    public GameObject GameMenu;
    void Start()
    {
        nextRaid = 0;
        UpdateTextVillage();
        UpdateTextRaid();
        raidTimer = raidMaxTime; // устанавливаем таймер рейда
        countingWheat = wheatCount;
        GameMenu.SetActive(true);
        PausePanel.SetActive(false);
    }

    void Update()
    {
        UpdateTextVillage();
        UpdateTextRaid();

        raidTimer -= Time.deltaTime; // запускаем таймер рейда
        RaidTimerImg.fillAmount = raidTimer / raidMaxTime; // заполнение таймера в UI #уточнить, про разделение UI и логики

        if (raidTimer <= 0) // при обнулении таймера 
        {
            raidTimer = raidMaxTime; // обновление таймера
            if (saveWave <= 0)
            {
                warriorCount -= nextRaid; // вычитание воинов
                currentRaidCount = nextRaid;
                nextRaid += raidIncrease; // увеличение волны
                countingEnemy += currentRaidCount;
            }
            else
            {
                saveWave -= 1;
            }
        }

        if (HarvestTimer.Tick) // когда срабатывает флаг try в компоненте где висит script ImageTimer
        {
            wheatCount += peasantCount * wheatPerPeasant; // Производство пшеницы за каждого крестьянина
            HarvestTimer.Tick = false; // обновление флага
            countingWheat += wheatCount;
        }

        if (EatingTimer.Tick) // когда срабатывает флаг try в компоненте где висит script ImageTimer
        {
            wheatCount -= warriorCount * wheatToWarriors; // Потребление пшеницы за каждого крестьянина
            EatingTimer.Tick = false; // обновление флага
        }

        if (wheatCount < 0) // Если пшеницы меньше нуля, для случая потребления пшеницы
        {
            if (warriorCount > 0) // если есть воины
            {
                warriorCount -= 1; // условно воин кормит деревню, #идеологически не подходит если войнов большое количество
                wheatCount = 0; // Сбрасываем пшеницу к нулю после вычета воина
            }
            else
            {
                GameOver();
            }
        }

        if (peasantTimer > 0) // если активировали таймер при нажатии кнопки найма
        {
            peasantTimer -= Time.deltaTime; // запуск таймера найма
            PeasantTimerImg.fillAmount = peasantTimer / peasantCreateTime; // анимация заливки тулбара
        }
        else if (peasantTimer > -1) // когда таймер дошел до 0
        {
            PeasantTimerImg.fillAmount = 1; // заливка полная
            peasantButton.interactable = true; // кнопка снова активна
            peasantCount += 1; // добавили крестьянина
            peasantTimer = -2; // обновление таймера #можно выбрать любое число за пределами цикла if
            isHiringPeasant = false; // Сброс флага блокировки
        }

        if (warriorTimer > 0) // если активировали таймер при нажатии кнопки найма
        {
            warriorTimer -= Time.deltaTime;
            WarrioirTimerImg.fillAmount = warriorTimer / warriorCreateTime;
        }
        else if (warriorTimer > -1)
        {
            WarrioirTimerImg.fillAmount = 1;
            warriorButton.interactable = true;
            warriorCount += 1;
            warriorTimer = -2;
            isHiringWarrior = false; // Сброс флага блокировки
        }

        if (warriorCount < 0) // для случая когда в рейде больше противников чем воинов было в деревне
        {
            GameOver();
        }
    } // Update()

    public void CreatePeasant() // метод для создания крестьянина 
    {
        if (isHiringPeasant || wheatCount < peasantCost) return; // Проверка флага блокировки кнопки и достаточности ресурсов

        isHiringPeasant = true; // Установка флага блокировки
        wheatCount -= peasantCost; // трата пшеницы
        peasantTimer = peasantCreateTime; // обновление таймера, запуск if найма для крестьянина с анимацией тулбара и т.д. из Update()
        peasantButton.interactable = false; // кнопка недоступна для найма, до окончания цикла if 

    }

    public void CreateWarrior() // метод для создания воина 
    {
        if (isHiringWarrior || wheatCount < warriorCost) return; // Проверка флага блокировки и достаточности ресурсов

        isHiringWarrior = true; // Установка флага блокировки
        wheatCount -= warriorCost;
        warriorTimer = warriorCreateTime;
        warriorButton.interactable = false;
    }

    public void UpdateTextVillage() // обновление текста для количества пшеницы, воинов и крестьян
    {
        resourcesTextVillage.text = peasantCount + "\n" + warriorCount + "\n\n" + wheatCount;
    }

    public void UpdateTextRaid() // обновление текста для количества пшеницы, воинов и крестьян
    {
        resourcesTextRaid.text = "До появления врагов, осталось волн - " + saveWave.ToString() + "\n" + "Прибудет врагов - " + nextRaid.ToString() +
            "\n\n" + countingWheat + "  " + countingEnemy;
    }

    public void TogglePause() // для меню паузы
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0; // Остановка времени
            PausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1; // Возобновление времени
            PausePanel.SetActive(false);
        }
    }


    private void GameOver() // активация экрана окончания игры
    {
        Time.timeScale = 0;
        GameOverScreen.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        UnityEditor.EditorApplication.isPlaying = false;
    }

}