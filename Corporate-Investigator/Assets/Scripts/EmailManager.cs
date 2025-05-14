using UnityEngine;

public class EmailManager : MonoBehaviour{
    [Header("Email Canvases")]
    public GameObject averyEmail;
    public GameObject jordanEmail;

    [Header("Prompt Image")]
    public GameObject promptImage;

    public void OnAveryEmailButton() {
        averyEmail.SetActive(true);
        promptImage.SetActive(false);
    }

    public void OnJordanEmailButton() {
        jordanEmail.SetActive(true);
        promptImage.SetActive(false);
    }

    public void OnExitButton() {
        jordanEmail.SetActive(false);
        averyEmail.SetActive(false);
        promptImage.SetActive(true);
    }
}
