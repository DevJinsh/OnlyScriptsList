using UnityEngine;

public class BlueGravityZone : GravityZone
{
    protected override void EnterGravityZone(Collider other, Rigidbody item)
    {
        
    }
    protected override void ApplyGravity()
    {
        if(_rigidbodies != null)
        {
            foreach (var item in _rigidbodies)
            {
                if (item != null)
                {
                    item.GetComponent<Rigidbody>().useGravity = false;
                    Vector3 forceAtPoint = ((transform.position - item.transform.position) * gravityPower) - (damping * item.velocity);
                    if (item.CompareTag("AdjustMassObj"))
                        item.AddForce(forceAtPoint, ForceMode.Force);
                    else
                        item.AddForce(forceAtPoint, ForceMode.Acceleration);
                    item.velocity = Vector3.ClampMagnitude(item.velocity, 4f); // 최대 속도 제한
                }
            }
        }
    }
}
