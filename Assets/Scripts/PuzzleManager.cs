using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls riddle progression and unlocks rooms when solved.
/// </summary>
public class PuzzleManager : MonoBehaviour
{
    [System.Serializable]
    public class PuzzleRule
    {
        public string id;
        public string requiredItemId;
        public string unlockRoom;
    }

    [SerializeField] private RoomManager roomManager;
    [SerializeField] private List<PuzzleRule> rules = new();

    private readonly HashSet<string> solvedPuzzles = new();

    /// <summary>
    /// Attempts to solve a puzzle using a specific item.
    /// </summary>
    public bool TrySolve(string puzzleId, Item item)
    {
        if (item == null || solvedPuzzles.Contains(puzzleId)) return false;

        PuzzleRule rule = rules.Find(r => r.id == puzzleId);
        if (rule == null || item.Id != rule.requiredItemId) return false;

        solvedPuzzles.Add(puzzleId);

        if (!string.IsNullOrEmpty(rule.unlockRoom) && roomManager != null)
        {
            roomManager.LoadRoom(rule.unlockRoom);
        }

        return true;
    }
}
