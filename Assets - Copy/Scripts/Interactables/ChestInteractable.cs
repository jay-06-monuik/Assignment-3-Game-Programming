using UnityEngine;
using DG.Tweening;

public class ChestInteractable : MonoBehaviour, IInteractable
{ private Animator anim; private bool _hasBeenCollected = false; private int isOpenHash; private Tween _loopTween; private Tween _collectTween;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null) return;

        isOpenHash = Animator.StringToHash("IsOpen");

        _loopTween = transform.DOScale(1.6f, 0.2f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad)
            .SetDelay(Random.Range(0.5f, 2.5f));
    }

    public void OnHoverIn()
    {
        anim?.SetBool(isOpenHash, true);
        if (Toast.Instance != null)
            Toast.Instance.ShowToast("Press E to open chest");
    }

    public void OnHoverOff()
    {
        anim?.SetBool(isOpenHash, false);
        if (Toast.Instance != null)
            Toast.Instance.HideToast();
    }

    public void OnInteract()
    {
        Debug.Log("Chest collected - calling GameEvents!");
        if (_hasBeenCollected) return;
        
        _hasBeenCollected = true;
        
        GameEvents.Instance.ChestCollected();

        _collectTween = transform.DOScale(0, 0.5f)
            .SetEase(Ease.InBack)
            .OnComplete(() => Destroy(gameObject));
    }

    void OnDestroy()
    {
        _loopTween?.Kill();
        _collectTween?.Kill();
    }
}