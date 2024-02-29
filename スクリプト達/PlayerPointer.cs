using System.Collections;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UIElements;

//�S����:���{

public class PlayerPointer : MonoBehaviour
{

    [SerializeField] private float armLength = 3;               //�v���C���[�̎�̒���

    [SerializeField] private Camera PlayerCam;

    [SerializeField] private RaycastHit hit;                    //���O�̊ȗ���

    [SerializeField] private float hitDistance;                 //���C�̔򋗗�

    [SerializeField] private GameObject handIcon;               //�A�N�e�B�u�ȃI�u�W�F�N�g�ɓ����������Ɏ�̃A�C�R����\��

    [SerializeField] private Animator hitAnimator;              //�q�b�g�����I�u�W�F�N�g��Animator�R���|�[�l���g

    [SerializeField] public GameObject _Player;

    [SerializeField] private PlayerDefault playerDefault;

    [SerializeField] private GameObject inLockerPos;            //���b�J�[�ɓ���ۂ̃v���C���[�̃|�W�V����

    [SerializeField] PlayProgressManager playProgress; 

    [SerializeField] GameObject pcScreen;

    /// <summary>
    /// �h�A�̃^�C�v�𐮐��Ŕ���(0=�J��,1=�����������Ă���,2=���̃h�A�͊J���Ȃ�)
    /// </summary>
    private int doorTypeNumber;


    //�I�u�W�F�N�g�̏����擾
    public void Pointer()
    {
        Collider hitCollider;                    //�q�b�g�����R���C�_�[���擾
        GameObject hitObj;                         //���������I�u�W�F�N�g���i�[
        string hitTag;                         //���������I�u�W�F�N�g�����i�[

        //switch���ŏ�Ԃ��Ǘ��@string�^���`���āA


        // Ray�̓J�����̈ʒu����Ƃ΂�
        var rayStartPosition = PlayerCam.transform.position;
        // Ray�̓J�����������Ă�����ɂƂ΂�
        var rayDirection = PlayerCam.transform.forward.normalized;



        // Ray���΂��iout raycastHit ��Hit�����I�u�W�F�N�g���擾����j
        if (Physics.Raycast(rayStartPosition, rayDirection, out hit))
        {
            hitCollider = hit.collider;             //���C�����������u�R���C�_�[�v���yhitCollider�z�Ɋi�[
            hitObj = hitCollider.gameObject;        //���C�����������R���C�_�[�������Ă���u�Q�[���I�u�W�F�N�g�v���yhitObj�z�Ɋi�[
            hitTag = hitCollider.tag;               //���C�����������R���C�_�[�������Ă���u�^�O�v���yhitTag�z�Ɋi�[
            hitDistance = hit.distance;             //���C�̔򋗗�
            Transform hitTransform;                 //���C�����������I�u�W�F�N�g�̐e�I�u�W�F�N�g���擾


            // Debug.DrawRay (Vector3 start(ray���J�n����ʒu), Vector3 dir(ray�̕����ƒ���), Color color(���C���̐F));
            Debug.DrawRay(rayStartPosition, rayDirection * armLength, Color.red);

            if (armLength > hitDistance)               //�͂��ʒu�ɂ���Ƃ�
            {
                //���ɓ������Ă��邩�𔻕�
                switch (hitTag)
                {
                    //�^�O���uLocker�v
                    case "Locker":
                        {
                            handIcon.SetActive(true);

                            if (Input.GetMouseButtonDown(0))
                            {
                                // ���̂Ƃ�
                                if (hit.transform.parent)
                                {
                                    hitTransform = hit.transform.parent;
                                    Debug.Log("���b�J�[�̃h�A����");

                                    //����ق��Əo��ق��𔻒f����֐�
                                    LockerState(hitTransform);                        

                                }
                                
                            }
                            break;
                        }

                    //�^�O���uSlideDoor�v
                    case "SlideDoor":
                        {
                            handIcon.SetActive(true);
                            if (Input.GetMouseButtonDown(0))
                            {
                                
                                doorTypeNumber = (int)hit.collider.gameObject.GetComponent<SlideDoor>().type;

                                if (doorTypeNumber == 0)
                                {
                                    //���������h�A�̊֐������s
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

                    //��L�ȊO�̃^�O��
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
        //���b�J�[���J��
        hitAnimator = _hitTransform.GetComponent<Animator>();

        //�v���C���[�����b�J�[�̑O�܂Ń��[�v������
        GameObject LockerPlayerPos = _hitTransform.GetChild(1).gameObject;   //Locker�̎q�I�u�W�F�N�g�̖��O���ȗ���
        Animator LockerAnim = LockerPlayerPos.GetComponent<Animator>();   //Locker�̎q�I�u�W�F�N�g�̃A�j���[�^�[���擾

        //����Ƃ��̏������L�q
        if (playerDefault._state == PlayerDefault.PlayerState.Nomal)
        {
            playerDefault.ChangeState(PlayerDefault.PlayerState.InLocker);

            Debug.Log("���b�J�[�̃h�A�ɐG��Ă����");

            
            _Player.transform.position = LockerPlayerPos.transform.position;
            _Player.transform.rotation = LockerPlayerPos.transform.rotation;
            _Player.transform.parent = LockerPlayerPos.transform;   //PlayerPos�̎q���ɂ���
            _Player.GetComponent<CapsuleCollider>().enabled = false;
            LockerAnim.SetTrigger("InLockerAnimTrigger");
            //�����J���A�j���[�V����
            hitAnimator.SetTrigger("LockerAnimTrigger");
        }

        //�o�鎞�̏������L��
        else
        {
            playerDefault.ChangeState(PlayerDefault.PlayerState.OutLocker);

            LockerAnim.SetTrigger("OutLockerAnimTrigger");
            hitAnimator.SetTrigger("LockerAnimTrigger");

        }

    

        StartCoroutine(LockerWaitTimeChangeState(1.5f));

    }

    //���b�J�[�̃A�j���[�V������̃v���C���[�X�e�[�g��ύX����֐�
    IEnumerator LockerWaitTimeChangeState(float hoge)
    {
        GameObject PlayerCam = _Player.transform.GetChild(0).gameObject;     //�v���C���[�̎q���i�J�������擾�j
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