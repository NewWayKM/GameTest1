using UnityEngine;
using UnityEngine.UI; 

public class ImageTimer : MonoBehaviour // ������������ ��� ������������ ����� � ������ try,false �� Tick � scripts GameManager
{
    public float MaxTime; // ������������ ����� ��� �������� #����� ��������, � ���� ��� ��������� � ��������� ����������
    public bool Tick; // ������������ ����������� �������

    private float currentTime; // ������������ ������� �� ��������� �����
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

        if (img != null) // ��������� ��������� �� � ���������� sprite
        {
            img.fillAmount = currentTime / MaxTime;
        }

    }


}