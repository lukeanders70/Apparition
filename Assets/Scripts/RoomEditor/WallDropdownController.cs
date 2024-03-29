using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class WallDropdownController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    UnityEngine.UI.Dropdown dropDown;
    void Start()
    {
#if UNITY_EDITOR
        var folders = AssetDatabase.GetSubFolders("Assets/Resources/images/Room");
        var options = new List<UnityEngine.UI.Dropdown.OptionData>() { };
        foreach (var folderPath in folders)
        {
            var pathNames = folderPath.Split('/');
            var wallName = pathNames[pathNames.Length - 1];
            var option = new UnityEngine.UI.Dropdown.OptionData(wallName);
            options.Add(option);
        }
        dropDown.AddOptions(options);
#endif
    }
}
