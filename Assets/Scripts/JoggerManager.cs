using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoggerManager : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float animationSpeedCoef;
    [SerializeField] private bool debugEnable = false;
    [SerializeField] private AnimationCurve debugCurve;
    [SerializeField] private float debugSpeed;

    private Animator animator;
    private float debugTimer = 0;
    private CovidEmotionMeasurement cem;

    [SerializeField] private float animationTurnSpeedCoef;
    public float turnSpeed;

    // Start is called before the first frame update
    void Start()
    {
        cem = GetComponent<CovidEmotionMeasurement>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vel;
        if (debugEnable)
        {
            if (cem.getEV() > cem.maximumEmotionValue * 0.15)
            {
                Debug.Log("Too close");
                vel = Vector3.zero;
            }
            else
            {
                float curveEval = debugCurve.Evaluate(debugTimer);
                vel = Vector3.forward * curveEval * speed;
                debugTimer += Time.deltaTime * debugSpeed;
                if (debugTimer > 1)
                    debugTimer -= 1;
            }
        }
        else
        {
            float inputAdj = (Input.GetKey(KeyCode.W) ? 1 : (Input.GetKey(KeyCode.S) ? -1 : 0));
            vel = Vector3.forward * inputAdj * speed;
        }
        transform.Translate(vel * Time.deltaTime);
        animator.SetFloat("Speed", vel.z);
        animator.SetFloat("AnimationSpeed", vel.z * animationSpeedCoef);

        Vector3 rot = turnSpeed * Vector3.up;
        transform.Rotate(rot * Time.deltaTime);
        animator.SetFloat("TurnSpeed", turnSpeed);
        animator.SetFloat("AnimationTurnSpeed", turnSpeed * animationSpeedCoef);
    }
}
