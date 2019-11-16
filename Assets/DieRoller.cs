using TMPro;
using UnityEngine;

public class DieRoller : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _resultText;

    private void Start()
    {
        _resultText.text = "Roll some dice!";
    }

    public void RollDie(int d)
    {
        int random = Random.Range(1, d + 1);
        _resultText.text = $"Result: {random.ToString()}";
    }
}