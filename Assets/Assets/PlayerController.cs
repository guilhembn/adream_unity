using UnityEngine;
using UnityEngine.Networking;

//[RequireComponent(typeof(Animator))]
//[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : NetworkBehaviour{

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float lookSensitivity = 3f;

     [SerializeField]
      private bool lockCursor = false;

     [SerializeField]
      private GameObject heartPrefab;
     [SerializeField]
      private Transform heartSpawn;

	// Component caching
	private PlayerMotor motor;

	void Start ()
	{
		motor = GetComponent<PlayerMotor>();
	}

	void Update ()
	{


		if (Cursor.lockState != CursorLockMode.Locked && lockCursor)
		{
			Cursor.lockState = CursorLockMode.Locked;
		}

		//Calculate movement velocity as a 3D vector
		float _xMov = Input.GetAxis("Horizontal");
		float _zMov = Input.GetAxis("Vertical");

		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;

		// Final movement vector
		Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

		

		//Apply movement
		motor.Move(_velocity);

		//Calculate rotation as a 3D vector (turning around)
		float _yRot = Input.GetAxisRaw("Mouse X");

		Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

		//Apply rotation
		motor.Rotate(_rotation);

		//Calculate camera rotation as a 3D vector (turning around)
		float _xRot = Input.GetAxisRaw("Mouse Y");

		float _cameraRotationX = _xRot * lookSensitivity;

		//Apply camera rotation
		motor.RotateCamera(_cameraRotationX);

           if (Input.GetKeyDown(KeyCode.Space))
           {
              CmdFire();
           }


	}
     [Command]
     void CmdFire()
     {
    // Create the Heart from the Heart Prefab
    GameObject heart = (GameObject)Instantiate (
        heartPrefab,
        heartSpawn.position,
        heartSpawn.rotation);

    // Add velocity to the bullet
    heart.GetComponent<Rigidbody>().velocity = heart.transform.forward * 6;

   // Spawn the bullet on the Clients
        NetworkServer.Spawn(heart);

    // Destroy the bullet after 2 seconds
    Destroy(heart, 2.0f);
    }


}
