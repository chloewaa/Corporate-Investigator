using UnityEngine;
using UnityEngine.UI;

public class IconButtons : MonoBehaviour {
    [Header("Message App Canvas")]
    public GameObject messageCanvas; 

    [Header("Profile App Canvas")]
    public GameObject profileCanvas; 

    [Header("Email App Canvas")]
    public GameObject emailCanvas; 

    [Header("Folder App Canvas")]
    public GameObject folderCanvas; 

    [Header("Taskbar Icons")]
    public GameObject messageIcon;
    public GameObject profileIcon;
    public GameObject emailIcon;
    public GameObject folderIcon;

    [Header("Dialogue System")]
    public DialogueSystem dialogueSystem;

    [Header("Tutorial Manager")]
    public TutorialManager tutorialManager;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip openClip;
    public AudioClip closeClip;

    void CloseAll() {
        messageCanvas.SetActive(false);
        messageIcon.SetActive(false);

        profileCanvas.SetActive(false);
        profileIcon.SetActive(false);

        emailCanvas.SetActive(false);
        emailIcon.SetActive(false);

        folderCanvas.SetActive(false);
        folderIcon.SetActive(false);
    }

    //Close other apps 
    public void OnMessageIconClicked() {
        CloseAll();
        messageCanvas.SetActive(true);
        messageIcon.SetActive(true);

        tutorialManager.NextStep();
    }

    public void OnProfileIconClicked() {
        CloseAll();
        profileCanvas.SetActive(true);
        profileIcon.SetActive(true);
    }

    public void OnEmailIconClicked() {
        CloseAll();
        emailCanvas.SetActive(true);
        emailIcon.SetActive(true);
    }

    public void OnFolderIconClicked() {
        CloseAll();
        folderCanvas.SetActive(true);
        folderIcon.SetActive(true);
    }

    public void CloseMessagingApp() {
        messageCanvas.SetActive(false);
        messageIcon.SetActive(false);
    }

    public void CloseProfileApp() {
        profileCanvas.SetActive(false);
        profileIcon.SetActive(false);
    }

    public void CloseEmailApp() {
        emailCanvas.SetActive(false);
        emailIcon.SetActive(false);
    }

    public void CloseFolderApp() {
        folderCanvas.SetActive(false);
        folderIcon.SetActive(false);
    }

    public void PlayOpenSound() {
        audioSource.PlayOneShot(openClip);
    }

    public void PlayCloseSound() {
        audioSource.PlayOneShot(closeClip);
    }
}
