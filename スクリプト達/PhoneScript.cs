using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S����:���{

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
            //���j���[���J���Ă��鎞
            case true:
                Debug.Log("�P�[�^�C�̓A�N�e�B�u����");
                PlayerPhone.SetActive(false);
                if (Settings.activeSelf == false && Map.activeSelf == false && Memo.activeSelf == false && Item.activeSelf == false)
                {
                    if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1))
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        Debug.Log("�A�N�e�B�u���Ƀ{�^���������ꂽ��");
                        MenuPhone.SetActive(false);
                        Time.timeScale = 1.0f;
                    }
                }
                break;

            //���j���[��ʂ���Ă��鎞
            case false:
                Debug.Log("�P�[�^�C�̓A�N�e�B�u����Ȃ���I");
                PlayerPhone.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1))
                {
                    Cursor.lockState = CursorLockMode.None;
                    Debug.Log("��A�N�e�B�u���Ƀ{�^���������ꂽ��");
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
