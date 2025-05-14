using UnityEngine;

public class TutorialManager : MonoBehaviour {
    [Header("Tutorial Steps Overlays")]  
    [Tooltip("Assign each tutorial overlay panel in order of display")]  
    public GameObject[] tutorialSteps;

    private int currentStep = 0;

    void Start() {
        HideAllSteps();
        //Begin with the first tutorial step
        ShowStep(0);
    }

    public void ShowStep(int stepIndex) {
        HideAllSteps();
        if (stepIndex >= 0 && stepIndex < tutorialSteps.Length) {
            tutorialSteps[stepIndex].SetActive(true);
            currentStep = stepIndex;
        }
    }

    public void NextStep() {
        int next = currentStep + 1;
        if (next < tutorialSteps.Length) {
            ShowStep(next);
        } else {
            HideAllSteps();
        }
    }

    public void HideAllSteps() {
        foreach (var panel in tutorialSteps) {
            if (panel != null) panel.SetActive(false);
        }
    }
}