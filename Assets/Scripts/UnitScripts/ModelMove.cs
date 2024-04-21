using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ModelMove : MonoBehaviour
{
    private float speed = 10;
    private float duration = 10;
    private Vector3 startPosition, finishPosition;
    public void StartMove(int x,int y,int z)
    {
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        finishPosition = new Vector3(x, y, z);
        //������ ��������� ��� �������� ����������
        StartCoroutine(MoveToFinish());
    }
    public void ModelDestroy()
    {
        Destroy(this.GameObject());
    }
    IEnumerator MoveToFinish()
    {
        float timeElapsed = 0;
        while(timeElapsed<duration)
        {
            //���������� �������� (�� 0 �� 1)
            float t = timeElapsed / duration;
            //������������ �� ���������� �� ������� ������
            transform.position = Vector3.Lerp(startPosition, finishPosition, t);
            //��������� ����
            timeElapsed += speed * Time.deltaTime;
            //���������� ���������� �����
            yield return null;
            //������������ ����� �������� �������
            transform.position = finishPosition;
        }
    }
}
