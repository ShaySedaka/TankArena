using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RespawnPopup : MonoBehaviour
{
    [SerializeField] private int _respawnTime;
    [SerializeField] private int _timeUntilRespawn;
    [SerializeField] private TextMeshProUGUI _countdownText;

    public void Present()
    {
        gameObject.SetActive(true);
        StartCoroutine(CountdownToRespawn());
    }

    private IEnumerator CountdownToRespawn()
    {
        _timeUntilRespawn = _respawnTime;
        yield return new WaitForSeconds(0.2f);

        while (_timeUntilRespawn > 0)
        {
            _countdownText.text = _timeUntilRespawn.ToString();
            yield return new WaitForSeconds(1);
            _timeUntilRespawn--;
        }

        gameObject.SetActive(false);
    }
}
