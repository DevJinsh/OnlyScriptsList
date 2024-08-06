using UnityEngine;
using UnityEngine.InputSystem;

partial class PlayerController
{
	private class PlayerInputActionContext : PlayerInputActions.IPlayerActions
    {
        private PlayerController _controller;

        public PlayerInputActionContext(PlayerController controller)
        {
            _controller = controller;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 inputV2 = context.ReadValue<Vector2>();
			float x = Mathf.Abs(inputV2.x) > 0.3f ? inputV2.x : 0f;
			float y = Mathf.Abs(inputV2.y) > 0.3f ? inputV2.y : 0f;
            _controller._directionInput = new Vector3(x, 0, y);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
				if (_controller._wallRunningComponent.IsWallRunning)
				{
					_controller._wallRunningComponent.TryJump();
				}
                else if(_controller.playerState != PlayerState.BreakFall)
                {
                    _controller.playerMovementComponent.TryJump();
                }
            }
        }

		public void OnLookAround(InputAction.CallbackContext context)
		{
            if(context.control.device.name.Equals("Mouse"))
            {
                _controller._lookAroundDelta = context.ReadValue<Vector2>();
            }
            else
            {
                _controller._lookAroundDelta = context.ReadValue<Vector2>() * 60f;
            }
		}

        public void OnBreakFall(InputAction.CallbackContext context)
        {
			if(context.phase == InputActionPhase.Started && !_controller._wallRunningComponent.IsWallRunning)
			{
                _controller._breakFallComponent.InitBreakFalling(_controller);
            }
        }

        public void OnSliding(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Started)
            {
                _controller._slidingComponent.StartSliding(_controller);
            }
            if(context.phase == InputActionPhase.Canceled)
            {
                _controller._slidingComponent.EndSliding(_controller);
            }
        }

		public void OnTurn(InputAction.CallbackContext context)
		{
			_controller._wallRunningComponent.TurnToWallForward();
		}

        public void OnPause(InputAction.CallbackContext context)
        {
            GameManager.Instance.Pause();
        }
    }
}