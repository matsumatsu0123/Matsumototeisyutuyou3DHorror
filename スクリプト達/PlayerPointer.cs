using System.Collections;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UIElements;

//担当者:松本

public class PlayerPointer : MonoBehaviour
{

    [SerializeField] private float armLength = 3;               //プレイヤーの手の長さ

    [SerializeField] private Camera PlayerCam;

    [SerializeField] private RaycastHit hit;                    //名前の簡略化

    [SerializeField] private float hitDistance;                 //レイの飛距離

    [SerializeField] private GameObject handIcon;               //アクティブなオブジェクトに当たった時に手のアイコンを表示

    [SerializeField] private Animator hitAnimator;              //ヒットしたオブジェクトのAnimatorコンポーネント

    [SerializeField] public GameObject _Player;

    [SerializeField] private PlayerDefault playerDefault;

    [SerializeField] private GameObject inLockerPos;            //ロッカーに入る際のプレイヤーのポジション

    [SerializeField] PlayProgressManager playProgress; 

    [SerializeField] GameObject pcScreen;

    /// <summary>
    /// ドアのタイプを整数で判別(0=開く,1=鍵がかかっている,2=このドアは開かない)
    /// </summary>
    private int doorTypeNumber;


    //オブジェクトの情報を取得
    public void Pointer()
    {
        Collider hitCollider;                    //ヒットしたコライダーを取得
        GameObject hitObj;                         //当たったオブジェクトを格納
        string hitTag;                         //当たったオブジェクト名を格納

        //switch文で状態を管理　string型を定義して、


        // Rayはカメラの位置からとばす
        var rayStartPosition = PlayerCam.transform.position;
        // Rayはカメラが向いてる方向にとばす
        var rayDirection = PlayerCam.transform.forward.normalized;



        // Rayを飛ばす（out raycastHit でHitしたオブジェクトを取得する）
        if (Physics.Raycast(rayStartPosition, rayDirection, out hit))
        {
            hitCollider = hit.collider;             //レイが当たった「コライダー」を【hitCollider】に格納
            hitObj = hitCollider.gameObject;        //レイが当たったコライダーを持っている「ゲームオブジェクト」を【hitObj】に格納
            hitTag = hitCollider.tag;               //レイが当たったコライダーを持っている「タグ」を【hitTag】に格納
            hitDistance = hit.distance;             //レイの飛距離
            Transform hitTransform;                 //レイが当たったオブジェクトの親オブジェクトを取得


            // Debug.DrawRay (Vector3 start(rayを開始する位置), Vector3 dir(rayの方向と長さ), Color color(ラインの色));
            Debug.DrawRay(rayStartPosition, rayDirection * armLength, Color.red);

            if (armLength > hitDistance)               //届く位置にあるとき
            {
                //何に当たっているかを判別
                switch (hitTag)
                {
                    //タグ名「Locker」
                    case "Locker":
                        {
                            handIcon.SetActive(true);

                            if (Input.GetMouseButtonDown(0))
                            {
                                // 扉のとき
                                if (hit.transform.parent)
                                {
                                    hitTransform = hit.transform.parent;
                                    Debug.Log("ロッカーのドアだよ");

                                    //入るほうと出るほうを判断する関数
                                    LockerState(hitTransform);                        

                                }
                                
                            }
                            break;
                        }

                    //タグ名「SlideDoor」
                    case "SlideDoor":
                        {
                            handIcon.SetActive(true);
                            if (Input.GetMouseButtonDown(0))
                            {
                                
                                doorTypeNumber = (int)hit.collider.gameObject.GetComponent<SlideDoor>().type;

                                if (doorTypeNumber == 0)
                                {
                                    //当たったドアの関数を実行
                                    hit.collider.gameObject.GetComponent<SlideDoor>().OpeningAndClosing();
                                }
                                else if(doorTypeNumber == 1)
                                {
                                    playProgress.TextOnDoorTouch(false);
                                }
                                else if(doorTypeNumber == 2)
                                {
                                    playProgress.TextOnDoorTouch(true);
                                }
                                else
                                {
                                    Debug.Log("error");
                                }
                            }
                            break;
                        }

                    case "Pc":
                        {
                            handIcon.SetActive(true);
                            if (Input.GetMouseButtonDown(0))
                            {
                                //Time.timeScale = 0;
                                
                                pcScreen.SetActive(true);
                            }
                        }
                        break;

                    //上記以外のタグ名
                    default:
                        {
                            handIcon.SetActive(false);
                            break;
                        }
                        
                }

            }
            else
            {
                handIcon.SetActive(false);
            }
        }
    }

    private void LockerState(Transform _hitTransform)
    {
        //ロッカーを開く
        hitAnimator = _hitTransform.GetComponent<Animator>();

        //プレイヤーをロッカーの前までワープさせる
        GameObject LockerPlayerPos = _hitTransform.GetChild(1).gameObject;   //Lockerの子オブジェクトの名前を簡略化
        Animator LockerAnim = LockerPlayerPos.GetComponent<Animator>();   //Lockerの子オブジェクトのアニメーターを取得

        //入るときの処理を記述
        if (playerDefault._state == PlayerDefault.PlayerState.Nomal)
        {
            playerDefault.ChangeState(PlayerDefault.PlayerState.InLocker);

            Debug.Log("ロッカーのドアに触れているよ");

            
            _Player.transform.position = LockerPlayerPos.transform.position;
            _Player.transform.rotation = LockerPlayerPos.transform.rotation;
            _Player.transform.parent = LockerPlayerPos.transform;   //PlayerPosの子供にする
            _Player.GetComponent<CapsuleCollider>().enabled = false;
            LockerAnim.SetTrigger("InLockerAnimTrigger");
            //扉を開くアニメーション
            hitAnimator.SetTrigger("LockerAnimTrigger");
        }

        //出る時の処理を記載
        else
        {
            playerDefault.ChangeState(PlayerDefault.PlayerState.OutLocker);

            LockerAnim.SetTrigger("OutLockerAnimTrigger");
            hitAnimator.SetTrigger("LockerAnimTrigger");

        }

    

        StartCoroutine(LockerWaitTimeChangeState(1.5f));

    }

    //ロッカーのアニメーション後のプレイヤーステートを変更する関数
    IEnumerator LockerWaitTimeChangeState(float hoge)
    {
        GameObject PlayerCam = _Player.transform.GetChild(0).gameObject;     //プレイヤーの子供（カメラを取得）
        yield return new WaitForSeconds(hoge);
        if(playerDefault._state== PlayerDefault.PlayerState.InLocker)
        {
            playerDefault.ChangeState(PlayerDefault.PlayerState.Locker);
        }
        else
        {
            playerDefault.ChangeState(PlayerDefault.PlayerState.Nomal);
            _Player.transform.parent = null;           
            _Player.GetComponent<CapsuleCollider>().enabled = true;
            _Player.transform.Rotate(0, 180, 0);
            PlayerCam.transform.Rotate(0,180, 0);
        }

    }




}