using UnityEngine;
using UnityEngine.UI; // Pour Button
using TMPro;          // Pour TMP support

public class QuitButton : MonoBehaviour
{
    public Button quitButton; // Assigne le bouton dans l'inspecteur

    void Start()
    {
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
        else
        {
            Debug.LogWarning("QuitButton: aucun bouton assigné !");
        }
    }

    void QuitGame()
    {
        Debug.Log("Quitter le jeu...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stoppe le jeu dans l'éditeur
#else
        Application.Quit(); // Quitte l'application buildée
#endif
    }
}
