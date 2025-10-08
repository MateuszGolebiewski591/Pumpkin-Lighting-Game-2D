using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class Card : MonoBehaviour
{
    [SerializeField] private string cardName;
    private Func<double, double> application;
    private bool clickable = false;
    private Player player;


    public double Apply(double input)
    {
        return application(input);
    }

    public void SetPlayer(Player givenPlayer)
    {
        this.player = givenPlayer;
    }

    public string GetCardName()
    {
        return this.cardName;
    }

    public void EnableClicking()
    {
        this.clickable = true;
    }
    public void DisableClicking()
    {
        this.clickable = false;
    }

    public void AssignRole(Func<double, double> givenApplication)
    {
        this.application = givenApplication;
    }

    public void ResetCard()
    {
        transform.position = new Vector3(15f, -3.4f, -2f);
        gameObject.SetActive(false);
    }

    public void AiPosition()
    {
        transform.position = new Vector3(6f, 10f, -2f);
        gameObject.SetActive(true);
    }

    public void AssumePosition(int position)
    {
        transform.position = new Vector3(15f, -3.4f, -2f);
        gameObject.SetActive(true);
        Shift(position);
    }

    public void Shift(int position)
    {
        switch (position)
        {
            case 1: MoveTo(new Vector3(-5f, -3.4f, -2f)); break;
            case 2: MoveTo(new Vector3(-2.5f, -3.4f, -2f)); break;
            case 3: MoveTo(new Vector3(0f, -3.4f, -2f)); break;
            case 4: MoveTo(new Vector3(2.5f, -3.4f, -2f)); break;
            case 5: MoveTo(new Vector3(5f, -3.4f, -2f)); break;
        }
    }

    public void MoveTo(Vector3 targetPosition)
    {
        StartCoroutine(MoveToPosition(targetPosition, 1f, false)); // 1 second
    }

    public void MoveToForever(Vector3 targetPosition)
    {
        StartCoroutine(MoveToPosition(targetPosition, 1f, true)); // 1 second
    }

    private IEnumerator MoveToPosition(Vector3 target, float duration, bool forever)
    {
        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Smooth easing curve (fast in middle, slow at start & end)
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target; // Snap exactly at end
        if (forever) Used();
    }

    public void Dispose()
    {
        MoveToForever(new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z));
    }

    public void ClickResponse()
    {
        if (this.clickable)
        {
            MoveTo(new Vector3(-6f, 2.1f, -2f));
            this.player.HandleClick(this);
        }
    }

    public void Used()
    {
        gameObject.SetActive(false);
    }
}
