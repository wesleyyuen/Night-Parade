using UnityEngine;

public class ButtonTutorial : MonoBehaviour
{
    [SerializeField] string _progressKey;
    [SerializeField] Animator _prompt;
    PlayerProgress _progress;
    bool _hasShown;
    void Awake()
    {
        _prompt.gameObject.SetActive(true);
    }

    void Start()
    {
        _progress = FindObjectOfType<PlayerProgress>();
        _hasShown = _progress.HasPlayerProgress(_progressKey);
        if (_hasShown) {
            Destroy(_prompt.gameObject);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag ("Player") && !_hasShown) {
            _prompt.SetTrigger("Open");
            _progress.AddPlayerProgress(_progressKey, 1);
        }
    }

    void OnTriggerExit2D (Collider2D other)
    {
        if (other.CompareTag ("Player") && !_hasShown)
            _prompt.SetTrigger("Close");
    }
}