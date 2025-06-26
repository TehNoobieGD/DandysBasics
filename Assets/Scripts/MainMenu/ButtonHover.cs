using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Enlarge(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Enlarge(false);
    }

    void Enlarge(bool enabled)
    {
        if (anim != null)
        {
            anim.SetBool("enlarge", enabled);
        }
    }
}
