using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public static GroundManager Instance { get; private set; }

    [SerializeField]
    private bool _isPlayerInGroundWithinity;
    /// Indicates, whether the player in a small enough withinity to the ground (defined by
    /// the trigger on the ground object), which in turn indicates that it's ok to jump.
    public bool IsPlayerInGroundWithinity =>_isPlayerInGroundWithinity;

    // List of all ground that the player is in withinity of.
    private readonly List<Ground> _activeGrounds = new();

    private void Start()
    {
        Instance = this;
    }

    public void AddActiveGround(Ground ground)
    {
        _activeGrounds.Add(ground);
        UpdateIsInWithinityValue();
    }

    public void RemoveActiveGround(Ground ground)
    {
        _activeGrounds.Remove(ground);
        UpdateIsInWithinityValue();
    }

    private void UpdateIsInWithinityValue()
    {
        _isPlayerInGroundWithinity = _activeGrounds.Count > 0;
    }
}