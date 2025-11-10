using System;
using UnityEngine;

public class MoleArea : MonoBehaviour
{
    [SerializeField] private bool isActive;
    [SerializeField] private Transform mole;
    [SerializeField] private SpriteRenderer duckSprite;
    [SerializeField] private Sprite normalDuckSprite;
    [SerializeField] private Sprite duckHitSprite;
    [SerializeField] private Vector3 upPosition = new Vector3(0f, 0.25f, 0f);
    [SerializeField] private Vector3 downPosition = new Vector3(0f, -0.25f, 0f);

    [SerializeField] private float moveDuration = 1f; 

    private Vector3 _targetPosition;

    public bool IsActive
    {
        get { return isActive; }
        set
        {
            isActive = value;
            _targetPosition = isActive ? transform.position + upPosition : transform.position + downPosition;
        }
    }

    public SpriteRenderer DuckSprite
    {
        get { return duckSprite; }
    }

    public Sprite NormalDuckSprite
    {
        get { return normalDuckSprite; }
    }

    public Sprite DuckHitSprite
    {
        get { return duckHitSprite; }
    }

    private void Start()
    {
        if (mole)
        {
            _targetPosition = transform.position + downPosition;
            mole.position = _targetPosition;
        }
    }

    private void Update()
    {
        if (!mole) 
            return;

        mole.position = Vector3.MoveTowards( mole.position, _targetPosition, (Vector3.Distance(mole.position, _targetPosition) / moveDuration) * Time.deltaTime );
    }

    private void OnDrawGizmos()
    {
        if (isActive)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + upPosition, 0.03f);
        }
        else
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position + downPosition, 0.03f);
        }
    }
}