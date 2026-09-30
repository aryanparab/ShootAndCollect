using UnityEngine;

public class PlayerStealth : MonoBehaviour
{
    public bool IsHidden { get; private set; }

    private int bushesInside = 0;

    public void EnterBush()
    {
        bushesInside++;
        IsHidden = bushesInside > 0;

        Debug.Log("PLAYER HIDDEN");
    }

    public void ExitBush()
    {
        bushesInside = Mathf.Max(0, bushesInside - 1);
        IsHidden = bushesInside > 0;

        Debug.Log("PLAYER VISIBLE");
    }
}