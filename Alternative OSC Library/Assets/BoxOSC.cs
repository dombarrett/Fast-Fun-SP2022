using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This namespace code structure is different than what we are used to
//However, we need to wrap our usual public class inside of it so we can use the extOSC library
namespace extOSC.Examples
{

	public class BoxOSC : MonoBehaviour
	{
		//This is a string for our OSC destination
		//Because it is public, we can change it in the Unity editor- useful!
		public string Address = "/publish/func";

		[Header("OSC Settings")]
		public OSCReceiver Receiver;

		//Our's Cube's rigidbody,
		//  and a true or false variable for whether or not it can jump
		Rigidbody rigidbody;
		bool canJump;

		//Variables we can update when we receive values over OSC
		int arduinoInt1, arduinoInt2, arduinoInt3, arduinoInt4, arduinoInt5;
		float receivedFloat = 0.0f;
		string receivedString = "***";

		void Start()
		{
			//Our OSC destination is called the "address" in the extOSC examples
			//We can bind our destinations to specific functions
			//Here, the function ReceivedMessage will run when we get an OSC message with the destination we are listening to
			Receiver.Bind(Address, ReceivedMessage);

			rigidbody = this.GetComponent<Rigidbody>();
		}

		void Update()
		{
			
			//These ints are being updated by incoming OSC messages
			//This should be compatible with the existing ArduinoOSC example on the class Github
			Debug.Log(arduinoInt1 + "," + arduinoInt2 + "," + arduinoInt3 + "," + arduinoInt4 + "," + arduinoInt5);

			//We can use them like our keyboard keys
			if (arduinoInt1==1)
			{
                transform.Translate(Vector3.forward * Time.deltaTime * 10);
            }

			if (arduinoInt2 == 1)
			{
                transform.Translate(Vector3.left * Time.deltaTime * 10);
            }

			if (arduinoInt3 == 1)
			{
                transform.Translate(Vector3.back * Time.deltaTime * 10);
            }

			if (arduinoInt4 == 1)
			{
                transform.Translate(Vector3.right * Time.deltaTime * 10);
            }



			//Jumping
			if (arduinoInt5 == 1)
			{
				//if we are repeatedly sending 1's
				//That could make AddForce a bit wild to work with

				//How can we limit our jumping?
				//We ask if we are already jumping, and if we aren't, we can jump

				//This is good code for regular 'space bar' type jumping, as well
				if (canJump)
				{
					rigidbody.AddForce(Vector3.up * 300);
					canJump = false;
					//But you'll need to reset canJump to 'true' somewhere
					//How about when we touch the ground?
				}
			}


		}


		void OnCollisionEnter(Collision other)
		{
			Debug.Log("Cube has been hit by " + other.gameObject.name);

			//If we touch the ground, reset out ability to jump
			if (other.gameObject.name == "Plane")
			{
				canJump = true;
			}

		}



        //The function that runs when an OSC message is sent to the destination we choose to listen to:
        private void ReceivedMessage(OSCMessage message)
        {
			//Debug.LogFormat("Received: {0}", message);

			//If our message has multiple values, we need to access them via an array
			//This will only work specifically with the existing class example-
			//  because that Arduino is sending 5 integers (zeros or ones)

			//If we only sent one value, we may get an error for trying to access a value that isn't there
			//ie; trying to grab the fifth value when only one was sent
			arduinoInt1 = message.Values[0].IntValue;
			arduinoInt2 = message.Values[1].IntValue;
			arduinoInt3 = message.Values[2].IntValue;
			arduinoInt4 = message.Values[3].IntValue;
			arduinoInt5 = message.Values[4].IntValue;

			//You can also specify different kinds of variables:
			//exampleFloatVar = message.Values[0].FloatValue
			//exampleStringVar = message.Values[0].StringValue
		}


    }
}