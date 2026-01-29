using TMPro;
using System.Collections;
using UnityEngine;

public class RichTextTyper : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private float charsPerSecond = 30f;

    private string fullText;
    private Coroutine typingCoroutine;

    void Start()
    {
        StartTyping("Simple <color=yellow>example</color> of text created with <#80ff80>TextMesh <#8080ff>Pro</color>!😂");
    }

    public void StartTyping(string text)
    {
        fullText = text;
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        textComponent.text = fullText;
        int visibleCount = 0;

        // TMP支持直接逐字显示，会自动处理富文本
        while (visibleCount < fullText.Length)
        {
            visibleCount++;
            textComponent.maxVisibleCharacters = visibleCount;

            yield return new WaitForSeconds(1f / charsPerSecond);
        }
    }
}