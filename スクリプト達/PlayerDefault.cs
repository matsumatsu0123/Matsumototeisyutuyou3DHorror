using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

//�S����:���{

public class PlayerDefault : PlayerPointer
{
    // �v���C���[�̏��
    public enum PlayerState { Nomal, InLocker, Locker, OutLocker, GameOver }

    public PlayerState _state;


    //�w�肵����ԂɕύX����
    public void ChangeState(PlayerState next) => _state = next;


    [SerializeField] private Transform XAxis;
    [SerializeField] private Transform YAxis;
    [SerializeField] private Rigidbody Ribo;
    
    public float sensitivity;

    [SerializeField] private float LimitXAxizAngle;

    [SerializeField] private float speed;

    private Vector3 mXAxiz;

    //�ǉ�
    //�Q�[���I�[�o�[�̍ۂɍĐ�����TimeLine�̍Đ��Ɏg�p
    public PlayableDirector playableDirector;   
    //TimeLine��obj
    public GameObject gameOverMovie;

    private void Start()
    {
        //�^�C�g����ʂ̐ݒ��ʂŕύX�������x�𔽉f
        sensitivity = GameManager.Instance.playerSensitivity;

        if (sensitivity == 0)
            sensitivity = 2;
    }
    //���_�ړ�
    private void CameraMove()
    {
        //��̏c�̓������v�Z
        Vector3 newRotation = XAxis.localEulerAngles;

        var x = mXAxiz.x + Input.GetAxis("Mouse Y");
        newRotation.y += Input.GetAxis("Mouse X") * sensitivity;

        //�p�x����
        if (x >= -LimitXAxizAngle && x <= LimitXAxizAngle)
        {
            //��薳����Δ��f
            mXAxiz.x = x;

            newRotation.x = -x;
        }

        XAxis.localEulerAngles = newRotation;
        
    }

    //���b�J�[���ł̎��_����
    private void LockerCamera()
    {
        
        float LockerXLimit = 20f;   //�J�����̏c����
        float LockerYLimit = 10f;   //�J�����̉�����

        //��̏c�̓������v�Z
        Vector3 newRotation = XAxis.localEulerAngles;

        var x = mXAxiz.x + Input.GetAxis("Mouse Y");
        newRotation.y += Input.GetAxis("Mouse X") * sensitivity;

        var y = mXAxiz.y + Input.GetAxis("Mouse X");
        //newRotation.x += Input.GetAxis("Mouse Y") * sensitivity;


        //�p�x����
        if (x >= -LockerXLimit && x <= LockerXLimit && y >= -LockerYLimit && y <= LockerYLimit)
        {
            //��薳����Δ��f
            mXAxiz.x = Mathf.Clamp(x, -LockerXLimit, LockerXLimit);            
            mXAxiz.y = Mathf.Clamp(y, -LockerYLimit, LockerYLimit);

            newRotation.x = -x;
            newRotation.y = mXAxiz.y;

        }

        XAxis.localEulerAngles = newRotation;

    }

    //�v���C���[�̈ړ�
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
                //�ʏ펞�̏������L��
                Move();
                CameraMove();
                
                _Player.GetComponent<CapsuleCollider>().enabled = true;
                Debug.Log("���݂̃v���C���[�̃X�e�[�g�́uNomal�v�ł��B");
                break;

            case PlayerState.InLocker:
                //���b�J�[�ɓ���܂ł̏������L��
                _Player.GetComponent<CapsuleCollider>().enabled = false;
                Debug.Log("���݂̃v���C���[�̃X�e�[�g�́uInLocker�v�ł��B");
                break;
            case PlayerState.Locker:
                //���b�J�[���̏������L��
                LockerCamera();
                Debug.Log("���݂̃v���C���[�̃X�e�[�g�́uLocker�v�ł��B");
                break;
            case PlayerState.OutLocker:
                //���b�J�[����o��ۂ̏������L��
                Debug.Log("���݂̃v���C���[�̃X�e�[�g�́uOutLocker�v�ł��B");
                break;
            case PlayerState.GameOver:
                //�Q�[���I�[�o�[���̏������L��
                Debug.Log("���݂̃v���C���[�̃X�e�[�g�́uGameOver�v�ł��B");
                break;
        }
    }

    private void Update()
    {
        switch (_state)
        {
            case PlayerState.Nomal:
                //�ʏ펞�̏������L��
                Pointer();
                break;

            case PlayerState.InLocker:
                //���b�J�[�ɓ���܂ł̏������L��
                //�G�̍s�����~�߂���
                break;
            case PlayerState.Locker:
                //���b�J�[���̏������L��
                Pointer();
                break;
            case PlayerState.OutLocker:
                //���b�J�[����o��ۂ̏������L��
                break;
            case PlayerState.GameOver:
                //�Q�[���I�[�o�[���̏������L��
                break;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.tag == "Enemy")
    //    {
    //        //�ȉ��̏������Q�[���I�[�o�[�̍ۂɉ�����
    //        gameOverMovie.SetActive(!gameOverMovie.activeSelf);
    //        PlayTimeline();
    //    }
    //}


    //TimeLine�Đ�����
    void PlayTimeline()
    {
        playableDirector.Play();
    }
}
