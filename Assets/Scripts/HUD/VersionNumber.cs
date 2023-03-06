using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class VersionNumber : MonoBehaviour
{
    private TextMeshProUGUI versionText;
    
    void Start()
    {
        versionText = GetComponent<TextMeshProUGUI>();
        versionText.text = Application.version;
    }
}
