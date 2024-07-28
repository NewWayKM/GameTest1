using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public ImageTimer HarvestTimer; // ������ ������������ �������, ������������ ������ � �������� �������� ����� ���������� ����� .Tick � �.�.
    public ImageTimer EatingTimer;  // ������ ����������� ������� -//-

    public Image PeasantTimerImg;   // ����������� ������� ������������ �����������, #� ���� ��� ��������� ������ ������-������ ������-������, �������� � ���������
    public Image WarrioirTimerImg;  // ����������� ������� ������������ �����
    public Image RaidTimerImg;      // ����������� ������� �����

    public Button peasantButton;
    public Button warriorButton;

    public TMP_Text resourcesTextVillage;  // ����� �������� ��� ���� ��������� ���������� ��� �������
    public TMP_Text resourcesTextRaid;     // ����� �������� ��� ���� ��������� ���������� ��� �����

    public int peasantCount;        // ��������� �������� ���-�� ��������
    public int warriorCount;        // ��������� �������� ���-�� ������
    public int wheatCount;          // ��������� �������� ���-�� �������

    public int wheatPerPeasant;     // ������������ ������� ����� ������������
    public int wheatToWarriors;     // ����������� ������� ����� ������

    public int peasantCost;         // ��������� ������ �����������
    public int warriorCost;         // ��������� ������ �����

    public float peasantCreateTime; // ����� �������� �����������
    public float warriorCreateTime; // ����� �������� �����

    public float raidMaxTime;       // ����� ����� �����
    public int raidIncrease;      // �� ������� ������������� ����. ����
    public int nextRaid;          // ���������� ����������� � ����. �����
    public int saveWave;          // ���������� ���� ��� �������

    public GameObject GameOverScreen; // ������ �� canvas/panel

    private float peasantTimer = -2;    // ������ ������������ �����������
    private float warriorTimer = -2;    // ������ ������������ �����
    private float raidTimer;            // ������ ������������ � ����� 

    private int countingWheat;          // ������� ���� ������������� �������
    private int countingEnemy;          // ������� ���� ������ ������������� � ����
    private int currentRaidCount;       // ���������� ������ � ������� �����


    private bool isHiringPeasant = false; // ���� ��� ����������
    private bool isHiringWarrior = false; // ���� ��� ����������

    public GameObject PausePanel; // ��� ���� �����
    public GameObject GameMenu;
    void Start()
    {
        nextRaid = 0;
        UpdateTextVillage();
        UpdateTextRaid();
        raidTimer = raidMaxTime; // ������������� ������ �����
        countingWheat = wheatCount;
        GameMenu.SetActive(true);
        PausePanel.SetActive(false);
    }

    void Update()
    {
        UpdateTextVillage();
        UpdateTextRaid();

        raidTimer -= Time.deltaTime; // ��������� ������ �����
        RaidTimerImg.fillAmount = raidTimer / raidMaxTime; // ���������� ������� � UI #��������, ��� ���������� UI � ������

        if (raidTimer <= 0) // ��� ��������� ������� 
        {
            raidTimer = raidMaxTime; // ���������� �������
            if (saveWave <= 0)
            {
                warriorCount -= nextRaid; // ��������� ������
                currentRaidCount = nextRaid;
                nextRaid += raidIncrease; // ���������� �����
                countingEnemy += currentRaidCount;
            }
            else
            {
                saveWave -= 1;
            }
        }

        if (HarvestTimer.Tick) // ����� ����������� ���� try � ���������� ��� ����� script ImageTimer
        {
            wheatCount += peasantCount * wheatPerPeasant; // ������������ ������� �� ������� �����������
            HarvestTimer.Tick = false; // ���������� �����
            countingWheat += wheatCount;
        }

        if (EatingTimer.Tick) // ����� ����������� ���� try � ���������� ��� ����� script ImageTimer
        {
            wheatCount -= warriorCount * wheatToWarriors; // ����������� ������� �� ������� �����������
            EatingTimer.Tick = false; // ���������� �����
        }

        if (wheatCount < 0) // ���� ������� ������ ����, ��� ������ ����������� �������
        {
            if (warriorCount > 0) // ���� ���� �����
            {
                warriorCount -= 1; // ������� ���� ������ �������, #������������� �� �������� ���� ������ ������� ����������
                wheatCount = 0; // ���������� ������� � ���� ����� ������ �����
            }
            else
            {
                GameOver();
            }
        }

        if (peasantTimer > 0) // ���� ������������ ������ ��� ������� ������ �����
        {
            peasantTimer -= Time.deltaTime; // ������ ������� �����
            PeasantTimerImg.fillAmount = peasantTimer / peasantCreateTime; // �������� ������� �������
        }
        else if (peasantTimer > -1) // ����� ������ ����� �� 0
        {
            PeasantTimerImg.fillAmount = 1; // ������� ������
            peasantButton.interactable = true; // ������ ����� �������
            peasantCount += 1; // �������� �����������
            peasantTimer = -2; // ���������� ������� #����� ������� ����� ����� �� ��������� ����� if
            isHiringPeasant = false; // ����� ����� ����������
        }

        if (warriorTimer > 0) // ���� ������������ ������ ��� ������� ������ �����
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
            isHiringWarrior = false; // ����� ����� ����������
        }

        if (warriorCount < 0) // ��� ������ ����� � ����� ������ ����������� ��� ������ ���� � �������
        {
            GameOver();
        }
    } // Update()

    public void CreatePeasant() // ����� ��� �������� ����������� 
    {
        if (isHiringPeasant || wheatCount < peasantCost) return; // �������� ����� ���������� ������ � ������������� ��������

        isHiringPeasant = true; // ��������� ����� ����������
        wheatCount -= peasantCost; // ����� �������
        peasantTimer = peasantCreateTime; // ���������� �������, ������ if ����� ��� ����������� � ��������� ������� � �.�. �� Update()
        peasantButton.interactable = false; // ������ ���������� ��� �����, �� ��������� ����� if 

    }

    public void CreateWarrior() // ����� ��� �������� ����� 
    {
        if (isHiringWarrior || wheatCount < warriorCost) return; // �������� ����� ���������� � ������������� ��������

        isHiringWarrior = true; // ��������� ����� ����������
        wheatCount -= warriorCost;
        warriorTimer = warriorCreateTime;
        warriorButton.interactable = false;
    }

    public void UpdateTextVillage() // ���������� ������ ��� ���������� �������, ������ � ��������
    {
        resourcesTextVillage.text = peasantCount + "\n" + warriorCount + "\n\n" + wheatCount;
    }

    public void UpdateTextRaid() // ���������� ������ ��� ���������� �������, ������ � ��������
    {
        resourcesTextRaid.text = "�� ��������� ������, �������� ���� - " + saveWave.ToString() + "\n" + "�������� ������ - " + nextRaid.ToString() +
            "\n\n" + countingWheat + "  " + countingEnemy;
    }

    public void TogglePause() // ��� ���� �����
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0; // ��������� �������
            PausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1; // ������������� �������
            PausePanel.SetActive(false);
        }
    }


    private void GameOver() // ��������� ������ ��������� ����
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