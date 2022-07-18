using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadingPanel : MonoBehaviour
{
    public Image ProgressImage;

    [SerializeField] private GameObject _progressBar;
    [SerializeField] private GameObject _reloadText;
    [SerializeField] private float fillAmount;

    public void DisplayReloadUI(float givenfillAmount)
    {
        fillAmount = Mathf.Min(1, givenfillAmount);

        if (fillAmount < 1)
        {
            ProgressImage.fillAmount = fillAmount;

            _progressBar.SetActive(true);
            _reloadText.SetActive(true);
            
        }
        else
        {
            _progressBar.SetActive(false);
            _reloadText.SetActive(false);
        }
    }
}
