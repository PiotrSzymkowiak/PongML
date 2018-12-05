using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

	public GameObject paddle;
	public GameObject ball;
	public string backwallTag;
	[Range(0,1)]
	public float trainingRate;
	Rigidbody2D brb;
	float yVelocity;
	float paddleMinY = 8.8f;
	float paddleMaxY = 17.4f;
	float paddleMaxSpeed = 15;
	

	ANN ann;

	void Start () {
		ann = new ANN(6,1,1,4,trainingRate);
		brb = ball.GetComponent<Rigidbody2D>();		
	}
	
	List<double> Run(double bx, double by, double bvx, double bvy, double px, double py, double pv, bool train)
	{
		List<double> inputs = new List<double>() {bx,by,bvx,bvy,px,py};
		List<double> outputs = new List<double>() {pv};
		if(train)
			return (ann.Train(inputs, outputs));
		else
			return (ann.CalcOutput(inputs, outputs));
	}
	
	void Update () {
		float posy = Mathf.Clamp(paddle.transform.position.y + (yVelocity * Time.deltaTime * paddleMaxSpeed),
								 paddleMinY, paddleMaxY);
		paddle.transform.position = new Vector3(paddle.transform.position.x, posy, paddle.transform.position.z);
		List<double> output = new List<double>();
		int layerMask = 1 << 9;
		RaycastHit2D hit = Physics2D.Raycast(ball.transform.position, brb.velocity, 1000, layerMask);
		if(hit.collider != null)
		{
			if(hit.collider.gameObject.tag == "tops")
			{
				Vector3 reflection = Vector3.Reflect(brb.velocity, hit.normal);
				hit = Physics2D.Raycast(hit.point, reflection, 1000, layerMask);
			}
		
			if(hit.collider != null && hit.collider.gameObject.tag == backwallTag)
			{
				float dy = (hit.point.y - paddle.transform.position.y);
				output = Run(ball.transform.position.x,
							 ball.transform.position.y,
							 brb.velocity.x, brb.velocity.y,
							 paddle.transform.position.x,
							 paddle.transform.position.y,
							 dy, true);

				yVelocity = (float) output[0];
			}
		}
		else
		{
			yVelocity = 0;
		}
	}
}
