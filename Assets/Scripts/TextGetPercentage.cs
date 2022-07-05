using UnityEngine;
using UnityEngine.UI;

public class TextGetPercentage : MonoBehaviour
{
    public GameObject agent;
    private MowerAgent _mowerAgent;

    private Text _timerText;

    private void Start()
    {
        _timerText = gameObject.GetComponent<Text>();
        _mowerAgent = agent.GetComponent<MowerAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        _timerText.text = "Round: " + _mowerAgent.currentEpisode + " | " + (int)(_mowerAgent.cumsumReward * 100f) + "% completed";
    }
}
