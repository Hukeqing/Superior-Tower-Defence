using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScrollControl : MonoBehaviour
{
    public Text T;
    public Scrollbar Sb;

    public GameControl Main;
    private const int V = 60;
    
    public void SetString(string s)
    {
        T.text = s.Replace("\\n", "\n");
        T.GetComponent<RectTransform>().rect.Set(0, 0, T.GetComponent<RectTransform>().rect.width, T.GetComponent<RectTransform>().rect.height);
        // Debug.Log(T.GetComponent<RectTransform>().rect.xMin);
        int l = V;
        for (int i = 0; i < T.text.Length; i++)
        {
            if (T.text[i] == '\n')
            {
                l += V;
            }
        }
        T.GetComponent<RectTransform>().sizeDelta = new Vector2(T.GetComponent<RectTransform>().sizeDelta.x, l);
        Sb.value = 1;
    }
    public void OpenURL(string s) => Application.OpenURL(s);
    public void Back()
    {
        Time.timeScale = 1;
        Main.Help();
    }
}
