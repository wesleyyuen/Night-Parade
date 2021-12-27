using UnityEngine;

public class ButtonTutorial : MonoBehaviour
{
    [SerializeField] private string _progressKey;
    [SerializeField] private Animator _prompt;
    private bool _hasShown;
    
    private void Awake()
    {
        _prompt.gameObject.SetActive(true);
    }

    private void Start()
    {
        _hasShown = SaveManager.Instance.HasOverallProgress(_progressKey);
        if (_hasShown) {
            Destroy(_prompt.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_hasShown) {
            _prompt.SetTrigger("Open");
            SaveManager.Instance.AddOverallProgress(_progressKey, 1);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag ("Player") && !_hasShown)
            _prompt.SetTrigger("Close");
    }
}