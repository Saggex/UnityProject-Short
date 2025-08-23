using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks overall game progression and quest states.
/// </summary>
public class QuestManager : MonoBehaviour
{
    private readonly HashSet<string> completedQuests = new();

    /// <summary>
    /// Marks a quest as complete.
    /// </summary>
    public void CompleteQuest(string questId)
    {
        if (!completedQuests.Contains(questId))
        {
            completedQuests.Add(questId);
        }
    }

    /// <summary>
    /// Checks if a quest is already completed.
    /// </summary>
    public bool IsCompleted(string questId) => completedQuests.Contains(questId);
}
