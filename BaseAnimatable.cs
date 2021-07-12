using System;
using System.Collections;
using Spine.Unity;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseAnimatable : MonoBehaviour
{
    [SerializeField] private AnimationReferenceAsset[] _idleAnimation;
    [SerializeField] private AnimationReferenceAsset _runAnimation;
    [SerializeField] private AnimationReferenceAsset _shootAnimation;
    
    private SkeletonAnimation _skeletonAnimation;
    
    private AnimationReferenceAsset _currentAnim;
    private int _randomAnimIndex;

    private bool _isShooting;
    
    private void Awake()
    {
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    private void SetAnimation(AnimationReferenceAsset referenceAsset, bool loop = true, float timeScale = 1)
    {
        if (_skeletonAnimation.AnimationName != null && _skeletonAnimation.AnimationName == referenceAsset.name)
            return;
        
        _skeletonAnimation.state.SetAnimation(0, referenceAsset, loop).TimeScale = timeScale;
    }
    
    protected void SetIdle()
    {
        if(_isShooting)
            return;
        if (_currentAnim == _idleAnimation[_randomAnimIndex])
            return;
        
        _randomAnimIndex = Random.Range(0, _idleAnimation.Length);
        _currentAnim = _idleAnimation[_randomAnimIndex];
        SetAnimation(_currentAnim);
    }
    
    protected void SetRun()
    {
        if (_currentAnim == _runAnimation)
            return;
        
        _currentAnim = _runAnimation;
        SetAnimation(_currentAnim);
    }
    
    protected void SetShoot()
    {
        if(_isShooting)
            return;
        if (_currentAnim == _shootAnimation)
            return;

        _isShooting = true;
        _currentAnim = _shootAnimation;
        SetAnimation(_currentAnim, false);
        
        StartCoroutine(ShootingEnd());
    }

    private IEnumerator ShootingEnd()
    {
        yield return new WaitForSeconds(GetShootingAnimLength());
        _isShooting = false;
    }

    protected float GetShootingAnimLength()
    {
        return _shootAnimation.Animation.Duration;
    }
}
