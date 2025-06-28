
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class ArmImpulser : MonoBehaviour
{
    //Variables to adjust parameters of the IMPULSE force
    [SerializeField] private float _recoilDuration;
    [SerializeField] private float _moveSmoothTime;


    //Variables tied to the player's arm:
    [SerializeField] private GameObject _armShot;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private AudioClip _dashSFX;
    [SerializeField] private AudioClip _throwSFX;
    private ArmImpulser _impulser;
    private GameObject _currentArmBullet;
    private Collider2D _playerCol;


    //Variables to obtain components external to the arm.
    private MousePosition _mousePosition;
    private OldPlayerBehaviour _movementBehaviour;


    //Public methods so that the methods of this main mechanic (private)
    //can be accessed from the OldPlayerBehaviour script (which manages inputs)
    //and the Armbullet script (which manages the logic of the thrown arm).

    public void MovePlayerToAnchor(Vector2 anchorPosition, ImpulseType type)
    {
        StartCoroutine(ApplyRecoil(anchorPosition, type));
    }
    public void GetThrowArm(ImpulseType type)
    {
        ThrowArm(type);
    }
 
    void Start()
    {
        //I'll ​​leave the link to the other script established. From here I can use other methods or properties.
        //Mainly used to get the mouse position, which caused problems when calculating it in both scripts
        _playerCol = GetComponent<Collider2D>();
        _mousePosition = GetComponent<MousePosition>(); 
        _movementBehaviour = GetComponent<OldPlayerBehaviour>();
        _impulser = this;
    }

    private IEnumerator ApplyRecoil(Vector2 anchorPosition, ImpulseType type) 
    {
        if (type != ImpulseType.Pull)
            yield break; //if not pull, break the coroutine
        {
            SFXManager.Instance.PlaySFX(_dashSFX); 
            _movementBehaviour.SetMovementEnabled(false);
            _movementBehaviour.isRecoiling = true;

            Vector2 velocity = Vector2.zero;
            float smoothTime = _moveSmoothTime;
            float stopThreshold = 0.5f;

            //Move the player as long as there is distance between him and the anchor point
            while (Vector2.Distance(transform.position, anchorPosition) > stopThreshold)
            {
                transform.position = Vector2.SmoothDamp(transform.position, anchorPosition, ref velocity, smoothTime);
                yield return null;
            }
            //make sure it ends exactly at the point
            //I moved the character away from the anchor point a bit because it was buggy
            transform.position = anchorPosition; 
            Vector2 directionAway = (transform.position - (Vector3)anchorPosition).normalized;
            float separationDistance = 5; 
            transform.position += (Vector3)(directionAway * separationDistance);


            //when finish, let the player move again
            _movementBehaviour.SetMovementEnabled(true);
            _movementBehaviour.isRecoiling = false;
        }

    }
    private void ThrowArm(ImpulseType type) 
    {
  
        if (_currentArmBullet != null) return; //only let be one active arm.
        SFXManager.Instance.PlaySFX(_throwSFX); 
        Vector2 direction = _mousePosition.MouseWorlPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //Rotate the projectile so that it looks where the mouse points
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        GameObject armBullet = GameObject.Instantiate(_armShot, _spawnPoint.position, rotation);

        if (armBullet!= null)
        {
             _currentArmBullet = armBullet; 
            //Save the reference of the current arml
            //Ignore collisions so the arm doesn't collide with the player
            Collider2D bulletCol = armBullet.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(bulletCol, _playerCol);


            //I pass the parameters to the methods that manage the arm logic
            var armScript = armBullet.GetComponent<ArmBullet>();
            armScript.SetDirection(direction);
            armScript.SetImpulseForce(_impulser); 
            armScript.SetImpulseType(type);
    
        }
    }
}


