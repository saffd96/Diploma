using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEventSystem : MonoBehaviour
{
    [SerializeField] private EventTrigger eventTrigger;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Button button;
    

    private void Awake()
    {
        eventTrigger = GetComponent<EventTrigger>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data); });
        eventTrigger.triggers.Add(entry);
        
        button.onClick.AddListener(audioManager.PlayButtonOnClickSfx);
    }
    
    private void OnPointerEnterDelegate(PointerEventData data)
    {
        AudioManager.Instance.PlayButtonOnHoverSfx();
    }
}
