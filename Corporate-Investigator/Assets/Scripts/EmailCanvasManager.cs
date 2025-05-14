using UnityEngine;

public class EmailCanvasManager : MonoBehaviour {
    [Header("Avery Kane Emails")]
    public GameObject[] averyEmailCanvases;

    [Header("Jordan Cole Emails")]
    public GameObject[] jordanEmailCanvases;

    public void ShowAveryEmail(int emailIndex) {
        CloseAllJordanEmails();
        for (int i = 0; i < averyEmailCanvases.Length; i++) {
            averyEmailCanvases[i].SetActive(i == emailIndex);
        }
    }

    public void ShowJordanEmail(int emailIndex) {
        CloseAllAveryEmails();
        for (int i = 0; i < jordanEmailCanvases.Length; i++) {
            jordanEmailCanvases[i].SetActive(i == emailIndex);
        }
    }

    public void CloseAllAveryEmails() {
        foreach (var canvas in averyEmailCanvases)
            canvas.SetActive(false);
    }

    public void CloseAllJordanEmails() {
        foreach (var canvas in jordanEmailCanvases)
            canvas.SetActive(false);
    }

    public void CloseAllEmails() {
        CloseAllAveryEmails();
        CloseAllJordanEmails();
    }
}
