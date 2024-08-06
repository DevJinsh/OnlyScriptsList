using UnityEngine;

public class WallJumpComponent : BaseComponent
{
	private WallJumpData _data;

	private Rigidbody _rigidbody;

	public WallJumpComponent(BaseController controller, WallJumpData wallJumpData) : base(controller)
	{
		_data = wallJumpData;
	}

	public void TryWallJump()
	{

	}
}
