using TMPro;
using UnityEngine.SceneManagement; // ��� ����������� �����
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
    public TMP_Text winText;
    public TMP_Text gameOverText;
    public TMP_Text peasantCostforButton; // ��� ��������� �� ������ �����
    public TMP_Text warriorCostforButton; // 

    public int peasantCount;        // ��������� �������� ���-�� ��������
    public int warriorCount;        // ��������� �������� ���-�� ������
    public int wheatCount;          // ��������� �������� ���-�� �������
    public int wheatGoal;           // ���������� ������� ��� ������

    public int wheatPerPeasant;     // ������������ ������� ����� ������������
    public int wheatToWarriors;     // ����������� ������� ����� ������

    public int peasantCost;         // ��������� ������ �����������
    public int warriorCost;         // ��������� ������ �����

    public float peasantCreateTime; // ����� �������� �����������
    public float warriorCreateTime; // ����� �������� �����

    private float timeToNextWave;  // ����� �� ��������� �����
    public float raidMaxTime;      // ����� ����� �����
    public int raidIncrease;       // �� ������� ������������� ����. ����
    public int nextRaid;           // ���������� ����������� � ����. �����
    public int saveWave;           // ���������� ���� ��� �������

    public GameObject GameOverScreen;   // ������ �� canvas/panel

    private float peasantTimer = -2;    // ������ ������������ �����������
    private float warriorTimer = -2;    // ������ ������������ �����
    private float raidTimer;            // ������ ������������ � ����� 

    private int countingWheat;          // ������� ���� ������������� �������
    private int countingEnemy;          // ������� ���� ������ ������������� � ����
    private int currentRaidCount;       // ���������� ������ � ������� �����


    private bool isHiringPeasant = false; // ���� ��� ����������
    private bool isHiringWarrior = false; // ���� ��� ����������

    public GameObject PausePanel; // ��� ���� �����
    public GameObject GameMenuPanel;
    public GameObject GameOverPanel;
    public GameObject WinPanel;

    public VolumeControl volumeControl; // ������ �� ��������� ������ 
    void Start()
    {
        Time.timeScale = 1;
        UpdateTextVillage();
        UpdateTextRaid();
        raidTimer = 0;
        timeToNextWave = (saveWave + 1) * raidMaxTime; // ������������� ������� �� ��������� ����� � ������ saveWave
        countingWheat = wheatCount;
        GameMenuPanel.SetActive(true);
        GameOverPanel.SetActive(false);
        PausePanel.SetActive(false);
        WinPanel.SetActive(false);
        peasantCostforButton.text = "��� - " + peasantCost.ToString(); // ��� ��������� �� ������ �����
        warriorCostforButton.text = "��� - " + warriorCost.ToString();
}

    void Update()
    {
        UpdateTextVillage();
        UpdateTextRaid();

        raidTimer += Time.deltaTime; // ��������� ������ �����
        RaidTimerImg.fillAmount = raidTimer / raidMaxTime; // ���������� ������� � UI #��������, ��� ���������� UI � ������

          if (raidTimer >= raidMaxTime)
          {
                raidTimer = 0;
                if (saveWave <= 0)
                {
                    warriorCount -= nextRaid; // ��������� ������
                    currentRaidCount = nextRaid;
                    nextRaid += raidIncrease; // ���������� �����
                    timeToNextWave = raidMaxTime;
                    countingEnemy += currentRaidCount;
                    volumeControl.EnemySound();
                }
                else
                {
                    saveWave -= 1;
                
                }
          timeToNextWave = (saveWave + 1) * raidMaxTime; // ���������� ������� �� ��������� �����

          }

        timeToNextWave -= Time.deltaTime;

        if (HarvestTimer.Tick) // ����� ����������� ���� try � ���������� ��� ����� script ImageTimer
        {
            wheatCount += peasantCount * wheatPerPeasant; // ������������ ������� �� ������� �����������
            HarvestTimer.Tick = false; // ���������� �����
            countingWheat += wheatCount;
            volumeControl.HarvestSound();
        }

        if (EatingTimer.Tick) // ����� ����������� ���� try � ���������� ��� ����� script ImageTimer
        {
            wheatCount -= warriorCount * wheatToWarriors; // ����������� ������� �� ������� �����������
            EatingTimer.Tick = false; // ���������� �����
            volumeControl.EatingSound();
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
            PeasantTimerImg.fillAmount = 0; // ������� ������
            peasantButton.interactable = true; // ������ ����� �������
            peasantCount += 1; // �������� �����������
            peasantTimer = -2; // ���������� ������� #����� ������� ����� ����� �� ��������� ����� if
            isHiringPeasant = false; // ����� ����� ����������
            volumeControl.PeasantSound();
        }

        if (warriorTimer > 0) // ���� ������������ ������ ��� ������� ������ �����
        {
            warriorTimer -= Time.deltaTime;
            WarrioirTimerImg.fillAmount = warriorTimer / warriorCreateTime;
        }
        else if (warriorTimer > -1)
        {
            WarrioirTimerImg.fillAmount = 0;
            warriorButton.interactable = true;
            warriorCount += 1;
            warriorTimer = -2;
            isHiringWarrior = false; // ����� ����� ����������
            volumeControl.WarriorSound();
        }

        if (warriorCount < 0) // ��� ������ ����� � ����� ������ ����������� ��� ������ ���� � �������
        {
            GameOver();
        }

        if (wheatCount > wheatGoal) // ��� ������ ����� � ����� ������ ����������� ��� ������ ���� � �������
        {
            WinGame();
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
        resourcesTextVillage.text = peasantCount + "\n" + warriorCount + "\n\n" + wheatCount + "\n\n" + wheatGoal;
    }

    public void UpdateTextRaid() // ���������� ������ ��� ���������� �������, ������ � ��������
    {
        resourcesTextRaid.text = "����� " + Mathf.CeilToInt(timeToNextWave).ToString() + " ��� \n" + "�������� - " + nextRaid.ToString();
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
        StopGame();
        GameOverScreen.SetActive(true);
        gameOverText.text ="�� ������� ������� - " + countingWheat.ToString() + "\n" + "������ ���� - " + countingEnemy.ToString();

    }
    private void WinGame() // ��������� ������ ��������� ����
    {
        StopGame();
        WinPanel.SetActive(true);
        // ������ ����������, �� ����� �� ���������
        winText.text = "�� ������� ������� - " + countingWheat.ToString() + "\n" + "������ ���� - " + countingEnemy.ToString();
    }

    public void StopGame()
    {
        Time.timeScale = 0; // ����� ���� �������� ������ � WinGame() � GameOver(), �� ��� ������������� � ����� ���� �����, ��� ����� ���� ������� ���������� � �.�.
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        UnityEditor.EditorApplication.isPlaying = false;
    }

}