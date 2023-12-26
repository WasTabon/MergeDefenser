using UnityEngine;
using TMPro;

public class RankTextController : MonoBehaviour
{
    private TextMeshProUGUI _rankText;
    private MergeUnit _mergeUnit;

    private void Start()
    {
        _rankText = GetComponentInChildren<TextMeshProUGUI>();
        _mergeUnit = GetComponent<MergeUnit>();
    }

    private void Update()
    {
        SetRankText();
    }

    private void SetRankText()
    {
        _rankText.text = _mergeUnit.Rank.ToString();
    }
}
