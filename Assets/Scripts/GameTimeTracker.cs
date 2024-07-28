using TMPro;
using UnityEngine;

public class GameTimeTracker : MonoBehaviour // ����������� ����� �� ����� ����
{
    public TMP_Text gameTimeText; // ������ �� ��������� ������� ��� ����������� �������
    private float elapsedTime; // ���������� ��� �������� ���������� �������

    void Start()
    {
        elapsedTime = 0f;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime; // ���������� ���������� �������

        // �������������� ���������� ������� � ������ ��:��:��
        int hours = Mathf.FloorToInt(elapsedTime / 3600);
        int minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        // ���������� ���������� ��������
        gameTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
}