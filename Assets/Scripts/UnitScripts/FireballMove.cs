using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireballMove : MonoBehaviour
{
    private float speed = 10;
    private float duration = 10;
    private Vector3 startPosition, finishPosition;
    private Renderer renderer;
    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }
    public void StartMove()
    {
        startPosition = new Vector3(GameInfo.save.up.x, 5.95f, GameInfo.save.up.y);
        finishPosition = new Vector3(GameInfo.save.down.x, 2.95f, GameInfo.save.down.y);
        renderer.enabled = true;
        //запуск корунтини для плавного переміщення
        StartCoroutine(MoveToFinish());
    }
    IEnumerator MoveToFinish()
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            //розрахунок прогресу (від 0 до 1)
            float t = timeElapsed / duration;
            //інтерполяція між початковою та кінцевою точкою
            transform.position = Vector3.Lerp(startPosition, finishPosition, t);
            //збільшення часу
            timeElapsed += speed * Time.deltaTime;
            //очікування наступного кадру
            yield return null;
            //забезпечення точної фінальної позиції
            transform.position = finishPosition;
            renderer.enabled = false;
        }
    }
}
