using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;

public class CalendarSystem : MonoBehaviour
{

    GameObject[] m_Days;
    public float dayTimer;
    public int currentDay;
    public int seasonIndex;
    public Sprite unselectedDay;
    public Sprite selectedDay;
    public TMP_Text timeText;
    public TMP_Text dateText;
    public GameObject rainPrefabEffect;
    public GameObject snowPrefabEffect;
    public GameObject GlobalLight;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_Days = GameObject.FindGameObjectsWithTag("Day");
        Debug.Log("Amount of days in array: " + m_Days.Length);

        seasonIndex = 1;
        dayTimer = 0;
        currentDay = 0;
        m_Days[currentDay].GetComponent<Image>().sprite = selectedDay;
        dateText.text = ("Date: Spring " + (currentDay + 1));
    }

    // Update is called once per frame
    void Update()
    {
        //update day timer (1 sec = 1 hour)
        dayTimer += Time.deltaTime;
        timeText.text = ("Time: " + dayTimer.ToString("F0") + ":00");

        //set up sunset and sunrise
        if (dayTimer > 18 && dayTimer <24)
        {
            if (GlobalLight.GetComponent<Light2D>().intensity > 0)
            {
                GlobalLight.GetComponent<Light2D>().intensity -= 1 * Time.deltaTime / 4;
            }
        }
        else if (dayTimer > 4 && dayTimer < 8)
        {
            //keep it dark on fall 7th/ halloween
            if (currentDay != 20)
            {
                if (GlobalLight.GetComponent<Light2D>().intensity < 1)
                {
                    GlobalLight.GetComponent<Light2D>().intensity += 1 * Time.deltaTime / 4;
                }
            }
        }
        //next day if 24 hours passed
        if(dayTimer >= 24)
        {
            if (currentDay < 27)
            {
                currentDay += 1;
                NextDay();
                dayTimer = 0;
            }
            else
            {
                currentDay = 0;
                NextDay();
                dayTimer = 0;
            }
        }

        // press N to progress the day for debugging
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (currentDay < 27)
            {
                currentDay += 1;
                NextDay();
                dayTimer = 0;
            }
            else
            {
                currentDay = 0;
                NextDay();
                dayTimer = 0;
            }
        }
    }

    void NextDay()
    {
        //update calender selected day
        if (currentDay >0 )
        {
            m_Days[currentDay].GetComponent<Image>().sprite = selectedDay;
            m_Days[currentDay - 1].GetComponent<Image>().sprite = unselectedDay;
        }
        else
        {
            m_Days[currentDay].GetComponent<Image>().sprite = selectedDay;
            m_Days[27].GetComponent<Image>().sprite = unselectedDay;
            audioSource.Play();
        }

        //delete last day weather effect
        GameObject[] oldWeather = GameObject.FindGameObjectsWithTag("Weather");
        for (int i = 0; i < oldWeather.Length; i++)
        {
            Destroy(oldWeather[i]);
        }

        //create chance for new weather effect
        int x = Random.Range(0, 2);
        Debug.Log("Random Num = " + x);

        //season specific events & update ui text
        if (currentDay < 7)
        {
            GlobalLight.GetComponent<Light2D>().color = new Color(0.9921f, 0.8196f, 0.8823f);
            Debug.Log(GlobalLight.GetComponent<Light2D>().color + "");
            seasonIndex = 1;
            dateText.text = ("Date: Spring " + (currentDay + 1));
            if (x ==0)
            {
                Instantiate(rainPrefabEffect, new Vector3(0, 0, 0), Quaternion.identity);
            }
        }
        else if (currentDay >= 7 && currentDay <14)
        {
            seasonIndex = 2;
            dateText.text = ("Date: Summer " + (currentDay + 1));
            GlobalLight.GetComponent<Light2D>().color = new Color(0.9885f, 1f, 0.5330f);
        }
        else if (currentDay >= 14 && currentDay < 21)
        {
            seasonIndex = 3;
            dateText.text = ("Date: Fall " + (currentDay + 1));
            GlobalLight.GetComponent<Light2D>().color = new Color(1f, 0.7988f, 0.3254f);
        }
        else if (currentDay >= 21 && currentDay < 28)
        {
            seasonIndex = 4;
            dateText.text = ("Date: Winter " + (currentDay + 1));
            if (x == 0)
            {
                Instantiate(snowPrefabEffect, new Vector3(0, 0, 0), Quaternion.identity);
            }
            GlobalLight.GetComponent<Light2D>().color = new Color(0.5613f, 0.9117f, 1f);
        }
    }

}
