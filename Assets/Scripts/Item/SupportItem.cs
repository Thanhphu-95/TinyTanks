using UnityEngine;

public abstract class SupportItem : MonoBehaviour
{
    public string itemName;
    public float duration = 3f;  // mặc định 3 giây
    public abstract void Activate(GameObject target);
}
