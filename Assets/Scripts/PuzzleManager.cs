using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls riddle progression and unlocks rooms when solved.
/// </summary>
public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private RoomManager roomManager;

    private readonly HashSet<string> solvedPuzzles = new();

    /// <summary>
    /// Attempts to solve a puzzle using a specific item.
    /// </summary>
    public bool TrySolve(string puzzleId, Item item)
    {
        // Placeholder for puzzle resolution rules.
        if (item == null || solvedPuzzles.Contains(puzzleId)) return false;
        solvedPuzzles.Add(puzzleId);
        return true;
    }
}
