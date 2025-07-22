using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class ArtifactUI : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text artifactNameText;
    public TMP_Text artifactDescriptionText;
    public Image artifactImage; 

    public void ShowArtifact(string name, string description, Sprite image) 
    {
        artifactNameText.text = name;
        artifactDescriptionText.text = description;
        artifactImage.sprite = image; 
        panel.SetActive(true);
    }

    public void HideArtifact()
    {
        panel.SetActive(false);
    }
}
