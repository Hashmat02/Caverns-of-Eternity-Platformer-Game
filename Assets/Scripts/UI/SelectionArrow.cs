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
        // Set the initial position of the arrow
		
		if (!rect) {
			ErrorHandling.throwError("No RectTransform found");
		}
		if (options.Length == 0) {
			ErrorHandling.throwError("No options configured.");
		}

        if (options.Length > 0)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, options[0].anchoredPosition.y);
        }
    }

    private void Update()
    {
        // Change position of selection arrow
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePosition(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePosition(1);
        }

        // Interact with options
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
        {
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
    }

    private void Interact()
    {
        // SoundManager.Instance.PlaySound(selectOptionSound);
        // Access the button component on each option and call its function
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
