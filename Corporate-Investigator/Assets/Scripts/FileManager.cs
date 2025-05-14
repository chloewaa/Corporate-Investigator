using UnityEngine;

public class FileManager : MonoBehaviour
{
    [Header("File Canvases")]
    public GameObject averyFiles;
    public GameObject jordanFiles;

    [Header("Prompt Image")]
    public GameObject promptImage;

    [Header("Avery Kane Files")]
    public GameObject[] averyFileCanvases;

    [Header("Jordan Cole Files")]
    public GameObject[] jordanFileCanvases;

    public void OnAveryFileButton() {
        averyFiles.SetActive(true);
        promptImage.SetActive(false);
    }

    public void OnJordanFileButton() {
        jordanFiles.SetActive(true);
        promptImage.SetActive(false);
    }

    public void OnExitFileButton() {
        jordanFiles.SetActive(false);
        averyFiles.SetActive(false);
        promptImage.SetActive(true);
    }

        public void ShowAveryFile(int fileIndex) {
        CloseAllJordanFiles();
        for (int i = 0; i < averyFileCanvases.Length; i++) {
            averyFileCanvases[i].SetActive(i == fileIndex);
        }
    }

    public void ShowJordanFile(int fileIndex) {
        CloseAllAveryFiles();
        for (int i = 0; i < jordanFileCanvases.Length; i++) {
            jordanFileCanvases[i].SetActive(i == fileIndex);
        }
    }

    public void CloseAllAveryFiles() {
        foreach (var canvas in averyFileCanvases)
            canvas.SetActive(false);
    }

    public void CloseAllJordanFiles() {
        foreach (var canvas in jordanFileCanvases)
            canvas.SetActive(false);
    }

    public void CloseAllFiles() {
        CloseAllAveryFiles();
        CloseAllJordanFiles();
    }
}
