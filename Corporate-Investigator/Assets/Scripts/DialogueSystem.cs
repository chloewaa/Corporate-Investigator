using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour {
    [Header("UI References")]
    public Button sendButton; 
    public TMP_Dropdown intentDropdown;
    public TMP_Dropdown topicDropdown;
    public Transform messageLogContent;
    public GameObject playerMessagePrefab;
    public GameObject npcMessagePrefab;
    public ScrollRect scrollRect;

    [Header("Icon Buttons")]
    public Button vivianIcon;
    public Button averyIcon;
    public Button jordanIcon;
    public Button folderIcon;
    public Button emailsIcon;
    public Button profilesIcon;

    [Header("NPC Dialogue Trees")]
    public DialogueNodeSO vivianTree;
    public DialogueNodeSO averyTree;
    public DialogueNodeSO jordanTree;

    [Header("Typing Delay")]
    public float npcInitialPause = 0.5f;
    public float npcTypingDuration = 2f;

    [Header("Layout")]
    public float verticalSpacingY = 10f;
    public float horizontalMargin = 20f;

    [Header("Trust UI")]
    public Slider trustSlider;
    public TextMeshProUGUI trustValueText;
    public int maxTrust = 100;

    [Header("Tutorial")]
    public TutorialManager tutorialManager;

    [Header("Final Decision Popup")]
    public GameObject finalDecisionPopup; 

    [Header("Final Decision Node")]
    public DialogueNodeSO vivianFinalNode;

    public enum GamePhase {     
        Tutorial,
        ExploreDesktop,
        FinalDecision
    }
    private GamePhase phase = GamePhase.Tutorial;

    //Conversation state
    private DialogueNodeSO currentNode;
    [SerializeField] private DialogueNodeSO startingNode;
    private Dictionary<string, DialogueNodeSO> currentNodePerPartner = new Dictionary<string, DialogueNodeSO>();
    private string currentPartnerId;

    //Trust per NPC
    private Dictionary<string,int> partnerTrust = new Dictionary<string,int>();

    //Chat history
    private struct ChatMessage { public string text; public bool isPlayer; public ChatMessage(string t,bool p){text=t;isPlayer=p;} }
    private Dictionary<string,List<ChatMessage>> chatHistory = new Dictionary<string,List<ChatMessage>>();

    //UI layout
    private RectTransform contentRect;
    private float nextMessageY;

    //Talked to booleans
    private bool talkedToAvery;
    private bool talkedToJordan; 

    void Awake() {
        contentRect = messageLogContent.GetComponent<RectTransform>();
        nextMessageY = 0f;

        //Initialize chat history
        chatHistory["Vivian Hayes"] = new List<ChatMessage>();
        chatHistory["Avery Kane"]   = new List<ChatMessage>();
        chatHistory["Jordan Cole"]  = new List<ChatMessage>();

        //Map dialogue trees
        currentNodePerPartner["Vivian Hayes"] = vivianTree;
        currentNodePerPartner["Avery Kane"]   = averyTree;
        currentNodePerPartner["Jordan Cole"]  = jordanTree;

        //Initialize trust
        partnerTrust["Vivian Hayes"] = maxTrust;
        partnerTrust["Avery Kane"]   = 50;
        partnerTrust["Jordan Cole"]  = 50;
    }

    void Start() {
        DisableAllIcons();
        StartConversation("Vivian Hayes");
        tutorialManager.ShowStep(0);
    }

    public void StartConversation(string partnerId) {
        //Clear existing messages
        foreach (Transform c in messageLogContent) Destroy(c.gameObject);

        nextMessageY = 0f;

        //Replay history 
        foreach (var msg in chatHistory[partnerId]) {
            AddMessage(msg.text, msg.isPlayer);
        }

        currentPartnerId = partnerId;
        currentNode = currentNodePerPartner[partnerId];
        LoadNode(currentNode);
        UpdateTrustUI();

        //Tutorial progression
        if (phase == GamePhase.Tutorial) {
            tutorialManager.NextStep(); 
        }

        //Icon interactivity
        vivianIcon.interactable = (phase == GamePhase.ExploreDesktop || phase == GamePhase.FinalDecision);
        averyIcon.interactable  = (phase == GamePhase.ExploreDesktop);
        jordanIcon.interactable = (phase == GamePhase.ExploreDesktop);
        folderIcon.interactable = (phase == GamePhase.ExploreDesktop);
        emailsIcon.interactable = (phase == GamePhase.ExploreDesktop);
        profilesIcon.interactable = (phase == GamePhase.ExploreDesktop);
    }

    public void SwitchPartner(string partnerId) {
        if (!chatHistory.ContainsKey(partnerId)) return;
        //Save current scroll position 
        StartConversation(partnerId);
    }

    public void OnSendPressed() {
        //Turn send button off 
        sendButton.interactable = false;

        string intent = intentDropdown.options[intentDropdown.value].text;
        string topic = topicDropdown.options[topicDropdown.value].text;
        string partner = currentPartnerId;
        
        //Find the transition
        var transition = currentNode.nextNodes
            .FirstOrDefault(t => t.intent == intent && t.topic == topic);

        //Trust gate 
        if (transition.requiredTrust > partnerTrust[currentPartnerId]) {
            AddMessage("I think this conversation is over.", false);
            return;
        }

        //If there's no next node, terminate here
        if (transition.nextNode == null) {
            OnConversationFinished(currentPartnerId);
            return;
        }

        tutorialManager.NextStep(); 

        //Player message
        string playerLine = GetLine(currentNode.playerLines, intent, topic);
        chatHistory[currentPartnerId].Add(new ChatMessage(playerLine, true));
        AddMessage(playerLine, true);

        //Update trust
        partnerTrust[currentPartnerId] = Mathf.Clamp(
            partnerTrust[currentPartnerId] + transition.trustDelta,
            0, maxTrust
        );
        UpdateTrustUI();

        //NPC reply
        string npcLine = GetLine(currentNode.npcResponses, intent, topic);
        StartCoroutine(NPCReplySequence(npcLine, intent, topic));

        //into the new node
        currentNodePerPartner[currentPartnerId] = transition.nextNode;
        currentNode = transition.nextNode;
        LoadNode(currentNode);

        //Double‐check for terminal node as a fallback
        if (currentNode.nextNodes.Count == 0) {
            OnConversationFinished(currentPartnerId);
        }

            if (intent == "Accuse") {
            if (partner == "Avery Kane") {
                SceneManager.LoadScene("WinScene");
            }
            else {
                SceneManager.LoadScene("LoseScene");
            }
            return;  
        }
    }

    private IEnumerator NPCReplySequence(string npcLine, string intent, string topic) {
        yield return new WaitForSeconds(npcInitialPause);

        GameObject typingBubble = AddMessage("…", false);
        var rt = typingBubble.GetComponent<RectTransform>(); float h = rt.rect.height;
        yield return new WaitForSeconds(npcTypingDuration);

        Destroy(typingBubble);

        nextMessageY -= h + verticalSpacingY;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, nextMessageY);
        chatHistory[currentPartnerId].Add(new ChatMessage(npcLine, false));

        AddMessage(npcLine, false);

        //re-enable send button
        sendButton.interactable = true;
    }

    void LoadNode(DialogueNodeSO node) {
        intentDropdown.ClearOptions();
        topicDropdown.ClearOptions();
        intentDropdown.AddOptions(node.intentOptions);
        topicDropdown.AddOptions(node.topicOptions);
    }

    void OnConversationFinished(string partnerId) {
        //Mark each side-chat done
        if (partnerId == "Avery Kane")  talkedToAvery  = true;
        if (partnerId == "Jordan Cole") talkedToJordan = true;

        //Only when both are done, show the pop-up
        if (phase == GamePhase.ExploreDesktop && talkedToAvery && talkedToJordan) {
            finalDecisionPopup.SetActive(true);
        }
    }

    void EnterExplorePhase() {
        folderIcon.interactable = true;
        emailsIcon.interactable = true;
        averyIcon.interactable  = true;
        jordanIcon.interactable = true;
        vivianIcon.interactable = true;
        profilesIcon.interactable = true;
    }

    public void ShowEpilogue() {
        Debug.Log("Epilogue reached");
    }

    string GetLine(List<DialogueNodeSO.DialogueLine> lines, string intent, string topic) {
        foreach (var line in lines) if (line.intent == intent && line.topic == topic) return line.lineText;
        return "I'm not sure how to reply to that.";
    }

    GameObject AddMessage(string text, bool isPlayer) {
        var prefab = isPlayer ? playerMessagePrefab : npcMessagePrefab;

        GameObject msg = Instantiate(prefab, messageLogContent, false);
        var tmp = msg.GetComponentInChildren<TMP_Text>(); tmp.text = text;

        Canvas.ForceUpdateCanvases();

        var rt = msg.GetComponent<RectTransform>(); float h = rt.rect.height, w = rt.rect.width;
        rt.anchorMin = new Vector2(0, 1); rt.anchorMax = new Vector2(0, 1); rt.pivot = new Vector2(0, 1);
        float x = isPlayer ? horizontalMargin : (contentRect.rect.width - w - horizontalMargin);
        
        rt.anchoredPosition = new Vector2(x, -nextMessageY);
        nextMessageY += h + verticalSpacingY;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, nextMessageY);
        scrollRect.verticalNormalizedPosition = 0f;

        return msg;
    }

    private void UpdateTrustUI() {
        bool show = currentPartnerId == "Avery Kane" || currentPartnerId == "Jordan Cole";
        trustSlider.gameObject.SetActive(show);
        trustValueText.gameObject.SetActive(show);

        if (show) {
            int t = partnerTrust[currentPartnerId];
            trustSlider.value = t;
            trustValueText.text = $"{t}/{maxTrust}";
        }
    }

    void DisableAllIcons() {
        averyIcon.interactable = false;
        jordanIcon.interactable = false;
        folderIcon.interactable = false;
        emailsIcon.interactable = false;
        profilesIcon.interactable = false;
    }

    public void OnOKButton() {
        if (phase == GamePhase.Tutorial) {
            tutorialManager.NextStep();     
            phase = GamePhase.ExploreDesktop;
            EnterExplorePhase();       
            return;   
        }
    }

    public void OnVivianClicked() {
        //If player has talked to both
        if (phase == GamePhase.ExploreDesktop && talkedToAvery && talkedToJordan) {
            finalDecisionPopup.SetActive(true);
        }
        else {
            StartConversation("Vivian Hayes");
        }
    }

    public void ConfirmFinalDecision() {
        finalDecisionPopup.SetActive(false);

        //flip into the final phase
        phase = GamePhase.FinalDecision;

        //prime Vivian’s final node
        currentNodePerPartner["Vivian Hayes"] = vivianFinalNode;

        //actually load it
        StartConversation("Vivian Hayes");
    }

    public void CancelFinalDecision() {
        finalDecisionPopup.SetActive(false);
    }
}
