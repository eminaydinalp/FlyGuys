using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : RMonoBehaviour,IPlayerStateObserver
{
    public Animator Animator;
    
    private PlayerState playerState => currentPlayerState;
    private Dictionary<string, string> animationNames;

    private void Awake()
    {
        AddToPlayerStateManager();
    }

    void Start()
    {
        GetComponentAndAssign(ref Animator);
        animationNames = ReflectionHelpers<object>.GetPropertyNamesWithValues(typeof(Animations));
    }
    
    public void OnPlayerStateChanged()
    {
        if (Animator == null)
            return;

        switch (playerState)
        {
            case PlayerState.Idle:
                SetAnimationTrigger(Animations.IDLE, false);
                break;
            case PlayerState.Running:
                SetAnimationTrigger(Animations.RUN, false);
                break;
            case PlayerState.Jumping:
                SetAnimationTrigger(Animations.JUMP, false);
                break;
            case PlayerState.Win:
                SetAnimationTrigger(Animations.VICTORY);
                break;
            default:
                break;
        } 
    }

    void SetAnimationTrigger(string animationProp, bool resetOther = false)
    {
        if (!resetOther)
        {
            Animator.SetTrigger(animationProp);
        }
        else
        {
            foreach (var item in animationNames)
            {
                if (item.Key.Equals(animationProp))
                {
                    Animator.SetTrigger(animationProp);
                }
                else
                {
                    Animator.ResetTrigger(item.Value);
                }
            }
        }
    }


    public void AddToPlayerStateManager()
    {
        player.PlayerStateManager.AddListener(this);
    }


}
