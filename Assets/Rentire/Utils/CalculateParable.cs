using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateParable
{
    public static float CalculateVelocity(Vector3 objectPosition, Vector3 targetPosition, float angle, float gravity)
    {
        float target_Distance = Vector3.Distance(targetPosition, objectPosition);
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / gravity);
        return projectile_Velocity;
    }
    
    public static float CalculateFlightTime(Vector3 objectPosition, Vector3 targetPosition, float angle, float gravity)
    {
        float target_Distance = Vector3.Distance(targetPosition, objectPosition);
        var projectile_Velocity = CalculateVelocity(objectPosition, targetPosition, angle, gravity);
        // Extract the X & Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(angle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(angle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        return flightDuration;
    }

    public static Vector3 GetLocalVelocityToReachDestination(Vector3 objectPosition, Vector3 targetPosition, float angle)
    {
        // source and target positions
        Vector3 pos = objectPosition;
        Vector3 target = targetPosition;

        // distance between target and source
        float dist = Vector3.Distance(pos, target);


        // calculate initival velocity required to land the cube on target using the formula (9)
        float Vi = Mathf.Sqrt(dist * -Physics.gravity.y / (Mathf.Sin(Mathf.Deg2Rad * angle * 2)));
        float Vy, Vz;   // y,z components of the initial velocity

        Vy = Vi * Mathf.Sin(Mathf.Deg2Rad * angle);
        Vz = Vi * Mathf.Cos(Mathf.Deg2Rad * angle);

        // create the velocity vector in local space
        Vector3 localVelocity = new Vector3(0f, Vy, Vz);

        return localVelocity;
    }

    public static Vector3 GetGlobalVelocityToReachDestination(Transform _object, Vector3 target, float angle)
    {
        _object.LookAt(target);
        var localVelocity = GetLocalVelocityToReachDestination(_object.position, target, angle);
        return _object.TransformVector(localVelocity);
    }
    
    public static IEnumerator SimulateProjectileTransform(Transform Projectile, Vector3 objectPosition, Vector3 targetPosition, float angle, float gravity)
    {
        // Calculate distance to target
        float target_Distance = Vector3.Distance(Projectile.position, targetPosition);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / gravity);

        // Extract the X & Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(angle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(angle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        // Rotate projectile to face the target.
        Projectile.rotation = Quaternion.LookRotation(targetPosition - Projectile.position);
        float elapse_time = 0;
        while (elapse_time < flightDuration)
        {
            Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
            elapse_time += Time.deltaTime;
            yield return null;
        }
    }
    
    public static IEnumerator SimulateProjectileRigid(Rigidbody Projectile, Vector3 objectPosition, Vector3 targetPosition, float angle, float gravity)
    {
        // Calculate distance to target
        float target_Distance = Vector3.Distance(Projectile.position, targetPosition);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / gravity);

        // Extract the X & Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(angle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(angle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        // Rotate projectile to face the target.
        Projectile.rotation = Quaternion.LookRotation(targetPosition - Projectile.position);
        float elapse_time = 0;
        while (elapse_time < flightDuration)
        {
            Projectile.velocity = new Vector3(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
            elapse_time += Time.deltaTime;
            yield return null;
        }
    }
    
    IEnumerator SimulateProjectileCR(Transform projectile, Vector3 startPosition, Vector3 endPosition)
    {
        projectile.position = startPosition;
        float arcAmount = 8f;
        float heightOfShot = 12f;
        Vector3 newVel = new Vector3();
        // Find the direction vector without the y-component
        Vector3 direction = new Vector3(endPosition.x, 0f, endPosition.z) - new Vector3(startPosition.x, 0f, startPosition.z);
        // Find the distance between the two points (without the y-component)
        float range = direction.magnitude;
 
        // Find unit direction of motion without the y component
        Vector3 unitDirection = direction.normalized;
        // Find the max height
   
        float maxYPos = startPosition.y + heightOfShot;
   
        // if it has, switch the height to match a 45 degree launch angle
        if (range / 2f > maxYPos)
            maxYPos = range / arcAmount;
        //fix bug when shooting on tower
        if (maxYPos - startPosition.y <= 0)
        {
            maxYPos = startPosition.y + 2f;
        }
        //fix bug caused if we can't shoot higher than target
        if (maxYPos - endPosition.y <= 0)
        {
            maxYPos = endPosition.y + 2f;
        }
        // find the initial velocity in y direction
        newVel.y = Mathf.Sqrt(-2.0f * -Physics.gravity.y * (maxYPos - startPosition.y));
        // find the total time by adding up the parts of the trajectory
        // time to reach the max
        float timeToMax = Mathf.Sqrt(-2.0f * (maxYPos - startPosition.y) / -Physics.gravity.y);
        // time to return to y-target
        float timeToTargetY = Mathf.Sqrt(-2.0f * (maxYPos - endPosition.y) / -Physics.gravity.y);
        // add them up to find the total flight time
        float totalFlightTime = timeToMax + timeToTargetY;
        // find the magnitude of the initial velocity in the xz direction
        float horizontalVelocityMagnitude = range / totalFlightTime;
        // use the unit direction to find the x and z components of initial velocity
        newVel.x = horizontalVelocityMagnitude * unitDirection.x;
        newVel.z = horizontalVelocityMagnitude * unitDirection.z;
   
        float elapse_time = 0;
        while (elapse_time < totalFlightTime)
        {
            projectile.Translate(newVel.x * Time.deltaTime, (newVel.y - (Physics.gravity.y * elapse_time)) * Time.deltaTime, newVel.z * Time.deltaTime);
            elapse_time += Time.deltaTime;
            yield return null;
        }
   
 
    }
    
    public static Vector3 CalculateVelocityToReachDestination(Vector3 source, Vector3 target, float angle){
        Vector3 direction = target - source;                            
        float h = direction.y;                                           
        direction.y = 0;                                               
        float distance = direction.magnitude;                           
        float a = angle * Mathf.Deg2Rad;                                
        direction.y = distance * Mathf.Tan(a);
        distance += h / Mathf.Tan(a);                                    
 
        // calculate velocity
        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2*a));
        return velocity * direction.normalized;    
    }
    
    
    public static void Launch(Transform _transform, Rigidbody _rigidbody, Vector3 target, float LaunchAngle)
    {
        // think of it as top-down view of vectors: 
        //   we don't care about the y-component(height) of the initial and target position.
        Vector3 projectileXZPos = new Vector3(_transform.position.x, _transform.position.y, _transform.position.z);
        Vector3 targetXZPos = new Vector3(target.x, _transform.position.y, target.z);
    
        // rotate the object to face the target
        _transform.LookAt(targetXZPos);

        // shorthands for the formula
        float R = Vector3.Distance(projectileXZPos, targetXZPos);
        float G = Physics.gravity.y;
        float tanAlpha = Mathf.Tan(LaunchAngle * Mathf.Deg2Rad);
        float H = target.y - _transform.position.y;

        // calculate the local space components of the velocity 
        // required to land the projectile on the target object 
        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)) );
        float Vy = tanAlpha * Vz;

        // create the velocity vector in local space and get it in global space
        Vector3 localVelocity = new Vector3(0f, Vy, Vz);
        Vector3 globalVelocity = _transform.TransformDirection(localVelocity);

        // launch the object by setting its initial velocity and flipping its state
        _rigidbody.velocity = globalVelocity;
    }
}
