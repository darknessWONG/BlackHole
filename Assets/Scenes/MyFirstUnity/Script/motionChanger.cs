using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*!
 *	----------------------------------------------------------------------
 *	@brief	モーション切り替えスクリプト
 *	
 *	@note	←→キーかOnGUIのボタンでモーションを切り替える
 *	
*/
public class motionChanger : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;

    private int m_AnimationIndex = 0;
    private int m_AnimationMax = 0;
    private AnimatorStateInfo m_PrevState;
    private bool m_ChangingMotion = false;

    private enum E_MotionType
    {
        index,
        toIdle,
        toMove,
        toGree,
        toJump,
        max,
    }

    private void Start()
    {
        AnimationClip[] AnimationClips = m_Animator.runtimeAnimatorController.animationClips;

        m_AnimationIndex = 0;
        m_AnimationMax = AnimationClips.Length;
        m_PrevState = m_Animator.GetCurrentAnimatorStateInfo(0);

        // index
        for (int i = 0; i < m_AnimationMax; i++)
        {
            if (m_PrevState.IsName(AnimationClips[i].name))
            {
                m_AnimationIndex = i;
                break;
            }
        }
    }

    private void Update()
    {

        // モーション遷移中
        AnimatorStateInfo animState = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (animState.fullPathHash != m_PrevState.fullPathHash)
        {
            m_PrevState = m_Animator.GetCurrentAnimatorStateInfo(0);
            //Debug.Log(m_ChangingMotion.ToString());
        }



        // 走るから待機に戻る
        //if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        //{
        //    ChangerAnimation(E_MotionType.toIdle);
        //}
        /*===== コントローラー =====*/
        if (Input.GetAxis("Horizontal") > -0.1f && Input.GetAxis("Horizontal") < 0.1f)
        {
            ChangerAnimation(E_MotionType.toIdle);
        }

        playerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>();
        // ジャンプ中　アニメーション終わったら
        if (playerController.E_JumpStatus.E_raise == pc.js
            && animState.IsName("mid_acq_twn_jump"))
        {
            // 走るに戻る
            //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            //{
            //    m_Animator.SetBool("Jump", false);
            //    ChangerAnimation(E_MotionType.toMove);
            //}
            //// 待機に戻る
            //else
            //{
            //    ChangerAnimation(E_MotionType.toIdle);
            //}


            /*===== コントローラー =====*/
            if (Input.GetAxis("Horizontal") < -0.1f || Input.GetAxis("Horizontal") > 0.1f)
            {
                m_Animator.SetBool("Jump", false);
                ChangerAnimation(E_MotionType.toMove);
            }
            else
            {
                ChangerAnimation(E_MotionType.toIdle);
            }

        }
    }

    private void FixedUpdate()
    {
        playerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>();
        // モーション遷移中
        AnimatorStateInfo animState = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (animState.fullPathHash != m_PrevState.fullPathHash)
        {
            m_PrevState = m_Animator.GetCurrentAnimatorStateInfo(0);
            //Debug.Log(m_ChangingMotion.ToString());
        }
        else
        {
            if (Input.GetButton("Jump") && playerController.E_JumpStatus.E_normal == pc.js)
            {

                ChangerAnimation(E_MotionType.toJump);
            }

            /*===== コントローラー =====*/
            if (Input.GetAxis("Horizontal") < -0.1f)
            {
                ChangerAnimation(E_MotionType.toMove);
            }
            else if (Input.GetAxis("Horizontal") > 0.1f)
            {
                ChangerAnimation(E_MotionType.toMove);
            }

            if (Input.GetButton("Jump") && playerController.E_JumpStatus.E_normal == pc.js)
            {
                ChangerAnimation(E_MotionType.toJump);
            }

        }
    }

    /*!
*	----------------------------------------------------------------------
*	@brief	move
*/
    public bool IsMoveAnimation()
    {
        AnimatorStateInfo animState = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (animState.IsName("mid_acq_twn_run"))
        {
            return true;
        }
        else if (animState.IsName("mid_acq_twn_jump"))
        {
            return true;
        }
        else if (animState.IsName("mid_acq_twn_idl"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /*!
 *	----------------------------------------------------------------------
 *	@brief	指定するアニメーションへ
*/
    private void ChangerAnimation(E_MotionType type)
    {
        switch(type)
        {
            case E_MotionType.toIdle:
                m_PrevState = m_Animator.GetCurrentAnimatorStateInfo(0);
                m_Animator.SetBool("Jump", false);
                m_Animator.SetBool("Gree", false);
                m_Animator.SetBool("Move", false);
                break;
            case E_MotionType.toMove:
                m_PrevState = m_Animator.GetCurrentAnimatorStateInfo(0);
                m_Animator.SetBool("Move", true);
                break;
            //case E_MotionType.toGree:
            //    m_PrevState = m_Animator.GetCurrentAnimatorStateInfo(0);
            //    m_Animator.SetBool("Move", false);
            //    m_Animator.SetBool("Gree", true);
            //    break;
            case E_MotionType.toJump:
                m_PrevState = m_Animator.GetCurrentAnimatorStateInfo(0);
                m_Animator.SetBool("Move", false);
                m_Animator.SetBool("Jump", true);
                break;
        }

    }



    /*!
	 *	----------------------------------------------------------------------
	 *	@brief	ボタン表示
	*/
    //private void OnGUI()
    //{
    //    GUIStyle tempStyle = GUI.skin.box;
    //    tempStyle.fontSize = 24;

    //    Vector2 boxSize = new Vector2(350f, 100f);
    //    Vector2 pos = new Vector2((Screen.width - boxSize.x - 50f), (Screen.height - boxSize.y - 50f));

    //    // モーション名
    //    string animName = m_Animator.runtimeAnimatorController.animationClips[m_AnimationIndex].name;
    //    string text = string.Format("{0} [{1}/{2}]", animName, (m_AnimationIndex + 1), m_AnimationMax);
    //    GUI.Box(new Rect(pos, boxSize), text, tempStyle);

    //    // ボタン
    //    pos.x += 60f;
    //    pos.y += 50f;
    //    if (GUI.Button(new Rect(pos, new Vector2(100f, 40f)), "<<", tempStyle))
    //    {
    //        PrevAnimation();
    //    }
    //    pos.x += (100f + 30f);
    //    if (GUI.Button(new Rect(pos, new Vector2(100f, 40f)), ">>", tempStyle))
    //    {
    //        NextAnimation();
    //    }
    //}
}
