using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BountyUI : MonoBehaviour
{

    public Text displayText;

    public AnimationCurve fadeOutAnimation;
    public float fadeSpeed = 1f;

    IEnumerator FadeOut()
    {
        float time = 0f;
        Color c = displayText.color;
        while (time <= 1f)
        {
            time += Time.deltaTime * fadeSpeed;
            float a = 1f - fadeOutAnimation.Evaluate(time);
            displayText.color = new Color(c.r, c.g, c.b, a);
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
            yield return 0;
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartBountyAnimation(string text)
    {
        SetText(text);
        StartCoroutine(FadeOut());
    }

    public void SetText(string text)
    {
        displayText.text = "+" + text + " Gold";
    }
}
