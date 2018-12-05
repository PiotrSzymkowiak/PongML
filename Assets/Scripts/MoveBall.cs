using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour {

	Vector3 ballStartPosition;
	Rigidbody2D rb;
	float speed = 400;
	public AudioSource blip;
	public AudioSource blop;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		ballStartPosition = transform.position;
		ResetBall();	
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "backwall")
			blop.Play();
		else
			blip.Play();
	}

	public void ResetBall()
	{
		transform.position = ballStartPosition;
		rb.velocity = Vector2.zero;
		Vector2 dir = new Vector3(Random.Range(100,300), Random.Range(-100,100)).normalized;
		rb.AddForce(dir*speed);
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			ResetBall();
		}
	}
}
