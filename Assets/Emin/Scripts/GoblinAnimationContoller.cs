using UnityEngine;

public class GoblinAnimationContoller : MonoBehaviour
{
   private static readonly int IsWalk = Animator.StringToHash("isWalk");
   private static readonly int IsIdle = Animator.StringToHash("isIdle");
   private static readonly int IsRun = Animator.StringToHash("isRun");
   private static readonly int IsInjured = Animator.StringToHash("isInjured");
   private static readonly int IsPunch = Animator.StringToHash("isPunch");
   private static readonly int IsSlash = Animator.StringToHash("isSlash");
   private static readonly int IsHit = Animator.StringToHash("isHit");
   private static readonly int IsDead = Animator.StringToHash("isDead");
   private static readonly int IsRoll = Animator.StringToHash("isRoll");
   private Animator _animator;

   private void Awake()
   {
      _animator = GetComponent<Animator>();
   }
   
   public void GetIdleAnimation()
   {
      _animator.SetBool(IsWalk, false);
      _animator.SetBool(IsRun, false);
      _animator.SetBool(IsIdle, true);
   }
   public void GetWalkAnimation()
   {
      _animator.SetBool(IsWalk, true);
      _animator.SetBool(IsInjured, false);
      _animator.SetBool(IsRun, false);
      _animator.SetBool(IsIdle, false);
   }

   public void GetInjuredWalkAnimation()
   {
      _animator.SetBool(IsInjured, true);
      _animator.SetBool(IsWalk, false);
      _animator.SetBool(IsRun, false);
      _animator.SetBool(IsIdle, false);
   }
   public void GetRunAnimation()
   {
      _animator.SetBool(IsRun, true);
      _animator.SetBool(IsWalk, false);
      _animator.SetBool(IsIdle, false);
   }
   
   public void GetSlashAnimation()
   {
      _animator.SetTrigger(IsSlash);
   }
   public void GetPunchAnimation()
   {
      _animator.SetTrigger(IsPunch);
   }
   public void GetHitAnimation()
   {
      _animator.SetTrigger(IsHit);
   }
   public void GetRollAnimation()
   {
      _animator.SetTrigger(IsRoll);
   }
   public void GetDeathAnimation()
   {
      _animator.Play("Dying Backwards");
   }

   public AnimatorStateInfo GetStateInfo(int layerIndex)
   {
      return _animator.GetCurrentAnimatorStateInfo(layerIndex);
   }
   
}
