using UnityEngine;

public class ButtonTutorial : MonoBehaviour
{
    [SerializeField] string _progressKey;
    [SerializeField] GameObject _button;
    PlayerProgress _progress;
    bool _hasShown;
    void Awake()
    {
        Utility.SetAlphaRecursively(_button, 0f);
        _button.SetActive(true);
    }

    void Start()
    {
        _progress = FindObjectOfType<PlayerProgress>();
        _hasShown = _progress.HasPlayerProgress(_progressKey);
        if (_hasShown)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag ("Player") && !_hasShown) {
            Utility.FadeGameObjectRecursively(_button, 0f, 1f, 0.25f);
            _progress.AddPlayerProgress(_progressKey, 1);
        }
    }

    void OnTriggerExit2D (Collider2D other)
    {
        if (other.CompareTag ("Player") && !_hasShown)
            Utility.FadeGameObjectRecursively(_button, 1f, 0f, 0.25f);
    }
}