using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    // [SerializeField] private AudioClip changeOptionSound;
    // [SerializeField] private AudioClip selectOptionSound;

    private RectTransform rect;
    private int currentPosition;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        Debug.Log("SelectionArrow Awake: RectTransform component assigned.");
        // Set the initial position of the arrow
        if (options.Length > 0)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, options[0].anchoredPosition.y);
            Debug.Log("Initial arrow position set.");
        }
    }

    private void Update()
    {
        // Change position of selection arrow
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Up key pressed. Changing position.");
            ChangePosition(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("Down key pressed. Changing position.");
            ChangePosition(1);
        }

        // Interact with options
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interaction key pressed. Interacting with option.");
            Interact();
        }
    }

    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (currentPosition < 0)
        {
            currentPosition = options.Length - 1;
        }
        else if (currentPosition >= options.Length)
        {
            currentPosition = 0;
        }

        // Assign the Y position of the current option to the arrow (basically moving up and down)
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, options[currentPosition].anchoredPosition.y);
        Debug.Log("Changed position to: " + currentPosition + ". Arrow moved to: " + rect.anchoredPosition);
    }

    private void Interact()
    {
        // SoundManager.Instance.PlaySound(selectOptionSound);
        // Access the button component on each option and call its function
        Debug.Log("Interacting with option at position: " + currentPosition);
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
