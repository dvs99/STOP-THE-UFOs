using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    [HideInInspector] public bool SpawningEnded = false;

    [SerializeField] private Text EndText;
    [SerializeField] private GameObject EndButton;
    [SerializeField] private Transform EnemyParent;
    [SerializeField] private Image EndGameImage;
    [SerializeField] private string wonMsg;
    [SerializeField] private string lostMsg;

    public static EndGameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void End(bool won)
    {
        if (won)
            EndText.text = wonMsg;
        else
            EndText.text = lostMsg;
        EndGameImage.enabled = true;
        EndText.enabled = true;
        EndButton.SetActive(true);
    }

    private void Update()
    {
        if (SpawningEnded && EnemyParent.childCount == 0)
        {
            End(true);
        }
    }

    public bool hasEnded()
    {
        return EndText.enabled;
    }
}
