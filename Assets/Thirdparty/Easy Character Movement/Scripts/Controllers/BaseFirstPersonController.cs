using UnityEngine;
using UnityEngine.InputSystem;

namespace ECM.Controllers
{
	/// <summary>
	/// Base First Person Controller.
	/// 
	/// Base class for a first person controller.
	/// It inherits from 'BaseCharacterController' and extends it to perform classic FPS movement.
	/// 
	/// As the base character controllers, this default behaviour can easily be modified or completely replaced in a derived class. 
	/// </summary>

	public class BaseFirstPersonController : BaseCharacterController
	{
		private Gnome.Player player;

		#region EDITOR EXPOSED FIELDS

		[Header("Input")]
		[SerializeField]
		private InputActionReference moveButton;

		[SerializeField]
		private InputActionReference jumpButton;

		[SerializeField]
		private InputActionReference crouchButton;

		[SerializeField]
		private InputActionReference mouseButton;

		[Header("First Person")]
		[Tooltip("Speed when moving forward.")]
		[SerializeField]
		private float _forwardSpeed = 5.0f;

		[Tooltip("Speed when moving backwards.")]
		[SerializeField]
		private float _backwardSpeed = 3.0f;

		[Tooltip("Speed when moving sideways.")]
		[SerializeField]
		private float _strafeSpeed = 4.0f;

		[Tooltip("Speed multiplier while running.")]
		[SerializeField]
		private float _runSpeedMultiplier = 2.0f;

		#endregion

		#region PROPERTIES

		private InputAction moveAction;
		private InputAction jumpAction;
		private InputAction crouchAction;

		/// <summary>
		/// Cached camera pivot transform.
		/// </summary>

		public Transform cameraPivotTransform { get; private set; }

		/// <summary>
		/// Cached camera transform.
		/// </summary>

		public Transform cameraTransform { get; private set; }

		public Transform cameraRotTransform { get; private set; }

		/// <summary>
		/// Cached MouseLook component.
		/// </summary>

		public Components.MouseLook mouseLook { get; private set; }

		/// <summary>
		/// Speed when moving forward.
		/// </summary>

		public float forwardSpeed
		{
			get { return _forwardSpeed; }
			set { _forwardSpeed = Mathf.Max(0.0f, value); }
		}

		/// <summary>
		/// Speed when moving backwards.
		/// </summary>

		public float backwardSpeed
		{
			get { return _backwardSpeed; }
			set { _backwardSpeed = Mathf.Max(0.0f, value); }
		}

		/// <summary>
		/// Speed when moving sideways.
		/// </summary>

		public float strafeSpeed
		{
			get { return _strafeSpeed; }
			set { _strafeSpeed = Mathf.Max(0.0f, value); }
		}

		/// <summary>
		/// Speed multiplier while running.
		/// </summary>

		public float runSpeedMultiplier
		{
			get { return _runSpeedMultiplier; }
			set { _runSpeedMultiplier = Mathf.Max(value, 1.0f); }
		}

		/// <summary>
		/// Run input command.
		/// </summary>

		public bool run { get; set; }

		#endregion

		#region METHODS

		/// <summary>
		/// Use this method to animate camera.
		/// The default implementation use this to animate camera's when crouching.
		/// Called on LateUpdate.
		/// </summary>

		protected virtual void AnimateView()
		{
			// Scale camera pivot to simulate crouching

			var yScale = isCrouching ? Mathf.Clamp01(crouchingHeight / standingHeight) : 1.0f;

			cameraPivotTransform.localScale = Vector3.MoveTowards(cameraPivotTransform.localScale,
				new Vector3(1.0f, yScale, 1.0f), 5.0f * Time.deltaTime);
		}

		/// <summary>
		/// Perform 'Look' rotation.
		/// This rotate the character along its y-axis (yaw) and a child camera along its local x-axis (pitch).
		/// </summary>

		protected virtual void RotateView()
		{
			mouseLook.LookRotation(movement, cameraRotTransform);
		}

		/// <summary>
		/// Override the default ECM UpdateRotation to perform typical fps rotation.
		/// </summary>

		protected override void UpdateRotation()
		{
			RotateView();
		}

		/// <summary>
		/// Get target speed, relative to input moveDirection,
		/// eg: forward, backward or strafe.
		/// </summary>

		protected virtual float GetTargetSpeed()
		{
			// Defaults to forward speed

			var targetSpeed = forwardSpeed;

			// Strafe

			if (moveDirection.x > 0.0f || moveDirection.x < 0.0f)
				targetSpeed = strafeSpeed;

			// Backwards

			if (moveDirection.z < 0.0f)
				targetSpeed = backwardSpeed;

			// Forward handled last as if strafing and moving forward at the same time,
			// forward speed should take precedence

			if (moveDirection.z > 0.0f)
				targetSpeed = forwardSpeed;

			// Handle run speed modifier

			return run ? targetSpeed * runSpeedMultiplier : targetSpeed;
		}

		/// <summary>
		/// Overrides CalcDesiredVelocity to generate a velocity vector relative to view direction
		/// eg: forward, backward or strafe.
		/// </summary>

		protected override Vector3 CalcDesiredVelocity()
		{
			// Set character's target speed (eg: moving forward, backward or strafe)

			speed = GetTargetSpeed();

			// Return desired velocity relative to view direction and target speed

			return transform.TransformDirection(base.CalcDesiredVelocity());
		}

		/// <summary>
		/// Overrides 'BaseCharacterController' HandleInput method,
		/// to perform custom input code. 
		/// </summary>

		protected override void HandleInput()
		{
			var move = moveAction.ReadValue<Vector2>();

			moveDirection = new Vector3
			{
				x = move.x,
				y = 0.0f,
				z = move.y
			};

			jump = jumpAction.ReadValue<float>() == 1;

			crouch = crouchAction.ReadValue<float>() == 1;


			/*
			// Toggle pause / resume.
			// By default, will restore character's velocity on resume (eg: restoreVelocityOnResume = true)

			// FIXME: Should this be enabled?
			//if (Input.GetKeyDown(KeyCode.P))
			//	pause = !pause;
			*/
		}

		#endregion

		#region MONOBEHAVIOUR

		/// <summary>
		/// Validate this editor exposed fields.
		/// </summary>

		public override void OnValidate()
		{
			// Call the parent class' version of method

			base.OnValidate();

			// Validate this editor exposed fields

			forwardSpeed = _forwardSpeed;
			backwardSpeed = _backwardSpeed;
			strafeSpeed = _strafeSpeed;

			runSpeedMultiplier = _runSpeedMultiplier;
		}

		/// <summary>
		/// Initialize this.
		/// </summary>

		public override void Awake()
		{
			// Call the parent class' version of method

			base.Awake();

			player = GetComponent<Gnome.Player>();

			moveAction = player.playerInput.actions[moveButton.action.name];
			jumpAction = player.playerInput.actions[jumpButton.action.name];
			crouchAction = player.playerInput.actions[crouchButton.action.name];

			// Cache and initialize this components

			mouseLook = GetComponent<Components.MouseLook>();
			if (mouseLook == null)
			{
				Debug.LogError(
					string.Format(
						"BaseFPSController: No 'MouseLook' found. Please add a 'MouseLook' component to '{0}' game object",
						name));
			}

			mouseLook.SetMouseAction(player.playerInput.actions[mouseButton.action.name]);

			cameraPivotTransform = transform.Find("Camera_Pivot");
			if (cameraPivotTransform == null)
			{
				Debug.LogError(string.Format(
					"BaseFPSController: No 'Camera_Pivot' found. Please parent a transform gameobject to '{0}' game object.",
					name));
			}

			var camRot = cameraPivotTransform.transform.Find("Camera_Rotate");
			if (camRot == null)
			{
				Debug.LogError("Nein cameran rotaten");
			}

			var cam = GetComponentInChildren<Camera>();
			if (cam == null)
			{
				Debug.LogError(
					string.Format(
						"BaseFPSController: No 'Camera' found. Please parent a camera to '{0}' game object.", name));
			}
			else
			{
				cameraTransform = cam.transform;
				cameraRotTransform = camRot;
				mouseLook.Init(transform, cameraTransform);
			}
		}

		public virtual void LateUpdate()
		{
			// Perform camera's (view) animation

			AnimateView();
		}

		#endregion
	}
}
