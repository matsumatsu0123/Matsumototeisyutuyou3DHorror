using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者:松本

public class PhoneScript : MonoBehaviour
{
    [SerializeField] private GameObject MenuPhone;
    [SerializeField] private GameObject PlayerPhone;

    [SerializeField] private GameObject Settings;
    [SerializeField] private GameObject Map;
    [SerializeField] private GameObject Memo;
    [SerializeField] private GameObject Item;



    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        MenuPhone.SetActive(false);
    }

    private void PhoneActives()
    {
        switch (MenuPhone.activeSelf)
        {
            //メニューを開いている時
            case true:
                Debug.Log("ケータイはアクティブだよ");
                PlayerPhone.SetActive(false);
                if (Settings.activeSelf == false && Map.activeSelf == false && Memo.activeSelf == false && Item.activeSelf == false)
                {
                    if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1))
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        Debug.Log("アクティブ中にボタンが押されたよ");
                        MenuPhone.SetActive(false);
                        Time.timeScale = 1.0f;
                    }
                }
                break;

            //メニュー画面を閉じている時
            case false:
                Debug.Log("ケータイはアクティブじゃないよ！");
                PlayerPhone.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1))
                {
                    Cursor.lockState = CursorLockMode.None;
                    Debug.Log("非アクティブ中にボタンが押されたよ");
                    MenuPhone.SetActive(true);
                    Time.timeScale = 0.0f;
                }

                break;

        }


    }

    void Update()
    {
        PhoneActives();
    }
}
