using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

//担当者:松本

public class PlayerDefault : PlayerPointer
{
    // プレイヤーの状態
    public enum PlayerState { Nomal, InLocker, Locker, OutLocker, GameOver }

    public PlayerState _state;


    //指定した状態に変更する
    public void ChangeState(PlayerState next) => _state = next;


    [SerializeField] private Transform XAxis;
    [SerializeField] private Transform YAxis;
    [SerializeField] private Rigidbody Ribo;
    
    public float sensitivity;

    [SerializeField] private float LimitXAxizAngle;

    [SerializeField] private float speed;

    private Vector3 mXAxiz;

    //追加
    //ゲームオーバーの際に再生するTimeLineの再生に使用
    public PlayableDirector playableDirector;   
    //TimeLineのobj
    public GameObject gameOverMovie;

    private void Start()
    {
        //タイトル画面の設定画面で変更した感度を反映
        sensitivity = GameManager.Instance.playerSensitivity;

        if (sensitivity == 0)
            sensitivity = 2;
    }
    //視点移動
    private void CameraMove()
    {
        //首の縦の動きを計算
        Vector3 newRotation = XAxis.localEulerAngles;

        var x = mXAxiz.x + Input.GetAxis("Mouse Y");
        newRotation.y += Input.GetAxis("Mouse X") * sensitivity;

        //角度検証
        if (x >= -LimitXAxizAngle && x <= LimitXAxizAngle)
        {
            //問題無ければ反映
            mXAxiz.x = x;

            newRotation.x = -x;
        }

        XAxis.localEulerAngles = newRotation;
        
    }

    //ロッカー内での視点操作
    private void LockerCamera()
    {
        
        float LockerXLimit = 20f;   //カメラの縦方向
        float LockerYLimit = 10f;   //カメラの横方向

        //首の縦の動きを計算
        Vector3 newRotation = XAxis.localEulerAngles;

        var x = mXAxiz.x + Input.GetAxis("Mouse Y");
        newRotation.y += Input.GetAxis("Mouse X") * sensitivity;

        var y = mXAxiz.y + Input.GetAxis("Mouse X");
        //newRotation.x += Input.GetAxis("Mouse Y") * sensitivity;


        //角度検証
        if (x >= -LockerXLimit && x <= LockerXLimit && y >= -LockerYLimit && y <= LockerYLimit)
        {
            //問題無ければ反映
            mXAxiz.x = Mathf.Clamp(x, -LockerXLimit, LockerXLimit);            
            mXAxiz.y = Mathf.Clamp(y, -LockerYLimit, LockerYLimit);

            newRotation.x = -x;
            newRotation.y = mXAxiz.y;

        }

        XAxis.localEulerAngles = newRotation;

    }

    //プレイヤーの移動
    private void Move()
    {
        float HAxis = Input.GetAxisRaw("Horizontal");
        float VAxis = Input.GetAxisRaw("Vertical");

        Vector3 velocity = XAxis.forward * VAxis * speed * Time.deltaTime
                         + XAxis.right   * HAxis * speed * Time.deltaTime;
        velocity.y = 0.0f;
        velocity.Normalize();

        Ribo.velocity = velocity * speed;
    }

    
    private void FixedUpdate()
    {
        switch (_state)
        {
            case PlayerState.Nomal:
                //通常時の処理を記載
                Move();
                CameraMove();
                
                _Player.GetComponent<CapsuleCollider>().enabled = true;
                Debug.Log("現在のプレイヤーのステートは「Nomal」です。");
                break;

            case PlayerState.InLocker:
                //ロッカーに入るまでの処理を記載
                _Player.GetComponent<CapsuleCollider>().enabled = false;
                Debug.Log("現在のプレイヤーのステートは「InLocker」です。");
                break;
            case PlayerState.Locker:
                //ロッカー内の処理を記載
                LockerCamera();
                Debug.Log("現在のプレイヤーのステートは「Locker」です。");
                break;
            case PlayerState.OutLocker:
                //ロッカーから出る際の処理を記載
                Debug.Log("現在のプレイヤーのステートは「OutLocker」です。");
                break;
            case PlayerState.GameOver:
                //ゲームオーバー時の処理を記載
                Debug.Log("現在のプレイヤーのステートは「GameOver」です。");
                break;
        }
    }

    private void Update()
    {
        switch (_state)
        {
            case PlayerState.Nomal:
                //通常時の処理を記載
                Pointer();
                break;

            case PlayerState.InLocker:
                //ロッカーに入るまでの処理を記載
                //敵の行動を止めたい
                break;
            case PlayerState.Locker:
                //ロッカー内の処理を記載
                Pointer();
                break;
            case PlayerState.OutLocker:
                //ロッカーから出る際の処理を記載
                break;
            case PlayerState.GameOver:
                //ゲームオーバー時の処理を記載
                break;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.tag == "Enemy")
    //    {
    //        //以下の処理をゲームオーバーの際に加える
    //        gameOverMovie.SetActive(!gameOverMovie.activeSelf);
    //        PlayTimeline();
    //    }
    //}


    //TimeLine再生する
    void PlayTimeline()
    {
        playableDirector.Play();
    }
}
