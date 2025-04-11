using UnityEngine;
using TMPro;

public class BoundaryWarning : MonoBehaviour
{
    public TextMeshProUGUI warningText;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            warningText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            warningText.gameObject.SetActive(false);
        }
    }
}
