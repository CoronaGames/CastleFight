using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonActivateObject : MonoBehaviour
     , IPointerEnterHandler
        , IPointerExitHandler
{
    [SerializeField] GameObject objectToActivate;
    [SerializeField] Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void CheckForActivation()
    {
        if (objectToActivate.activeInHierarchy)
        {
            objectToActivate.SetActive(false);
        }
        else
        {
            objectToActivate.SetActive(true);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ((IPointerEnterHandler)button).OnPointerEnter(eventData);
            objectToActivate.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ((IPointerExitHandler)button).OnPointerExit(eventData);
        objectToActivate.gameObject.SetActive(false);
    }
}
