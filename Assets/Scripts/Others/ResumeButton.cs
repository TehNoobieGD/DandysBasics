using UnityEngine;
using UnityEngine.UI;

public class UnpauseButton : MonoBehaviour
{
    public PauseMenu pauseMenu; // assigné dans l'inspecteur ou via FindObjectOfType

    void Start()
    {
        if (pauseMenu == null)
        {
            pauseMenu = FindObjectOfType<PauseMenu>();
            if (pauseMenu == null)
            {
                Debug.LogError("Aucun PauseMenu trouvé dans la scène !");
            }
        }

        Button btn = GetComponent<Button>();
        if (btn == null)
        {
            Debug.LogError("Pas de composant Button sur ce GameObject !");
            return;
        }

        btn.onClick.AddListener(UnpauseGame);
        Debug.Log("Listener ajouté au bouton Unpause");
    }

    void UnpauseGame()
    {
        Debug.Log("Bouton Unpause cliqué");
        if (pauseMenu != null)
        {
            pauseMenu.UnpauseGame();
        }
        else
        {
            Debug.LogWarning("Référence pauseMenu est null !");
        }
    }
}
