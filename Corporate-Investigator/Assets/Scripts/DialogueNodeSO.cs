using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDialogueNode", menuName = "Dialogue/Node")]
public class DialogueNodeSO : ScriptableObject {
    [Header("Node ID")]
    public string nodeID;

    [Header("Intent Options")]
    public List<string> intentOptions;

    [Header("Topic Options")]
    public List<string> topicOptions;

    [Header("Player Dialogue Lines")]
    public List<DialogueLine> playerLines;

    [Header("NPC Dialogue Lines")]
    public List<DialogueLine> npcResponses;

    [Header("Trasition Node")]
    public List<DialogueTransition> nextNodes;

    [System.Serializable]
    public struct DialogueLine {
        public string intent;
        public string topic;
        [TextArea(2, 5)]
        public string lineText;
    }

    [System.Serializable]
    public struct DialogueTransition {
        public string intent;
        public string topic;
        public DialogueNodeSO nextNode;

        [Tooltip("Player must have ≥ this trust to pick this option")]
        public int requiredTrust;
    
        [Tooltip("How much to add or subtract when this transition happens")]
        public int trustDelta;
    }
}

