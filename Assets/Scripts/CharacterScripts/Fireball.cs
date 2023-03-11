using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speedUp = 0, velocity, acceleration, direction;
    public bool set = false;
    public Vector3 targetPos;

    public void SetFireball(int playerNum, int oppPlayerNum) {
        if (playerNum == 1) {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        targetPos = GameObject.Find("Player" + oppPlayerNum).transform.position;
        set = true;
    }

    public void Destroy() {
        Destroy(gameObject, 0);
    }

    void Update()
    {
        if (set) {
            direction = (targetPos.x - gameObject.transform.position.x)/Mathf.Abs(targetPos.x - gameObject.transform.position.x);
            gameObject.transform.position += new Vector3 ((direction * velocity + speedUp)*Time.deltaTime, 0, .05f*Time.deltaTime);
            speedUp += acceleration * direction * Time.deltaTime;
        }
    }
}
