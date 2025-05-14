using UnityEngine;

public class ProfileSwitcher : MonoBehaviour {
    [Header("Profile Canvases")]
    public GameObject jordanProfile;
    public GameObject averyProfile;

    public void OnJordanButton() {
        averyProfile.SetActive(false);
        jordanProfile.SetActive(true);
    }

    public void OnAveryButton() {
        averyProfile.SetActive(true);
        jordanProfile.SetActive(false);
    }
}
