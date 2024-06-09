using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CameraControl;
using static GameController;
using static ForCueBall;

public class PhysicsSimulation : MonoBehaviour
{
    // A simple data container for transform values 
    private class TransformData
    {
        public Vector3 Position;
        public Quaternion Rotation;

        private readonly Rigidbody _rigidbody;

        public TransformData(Rigidbody rigidbody)
        {
            _rigidbody = rigidbody;
            Update();
        }

        public void Update()
        {
            Position = _rigidbody.position;
            Rotation = _rigidbody.rotation;
        }
    }

    Vector3 direction;
    public static float force;

    // How far to predict into the future
    public float maxPreviewTime = 10;

    public GameObject testBall;
    public LayerMask layerMask;
    SphereCollider ball_sc;
    MeshRenderer test_mr;
    GameObject currentHitObject;
    float currentHitDistance;

    private readonly Dictionary<Ball, TransformData> initialPositions = new Dictionary<Ball, TransformData>();
    private readonly Dictionary<Ball, List<Vector3>> simulatedPositions = new Dictionary<Ball, List<Vector3>>();

    private void Start()
    {
        ball_sc = CueBall.GetComponent<SphereCollider>();
        test_mr = testBall.GetComponent<MeshRenderer>();

        // Initialize empty data sets for the existing balls
        foreach (var ball in balls)
        {
            initialPositions.Add(ball, new TransformData(ball.Rigidbody));
            simulatedPositions.Add(ball, new List<Vector3>());
        }

    }

    private void Update()
    {
        RayCast();
        direction = -offset2.normalized;
        if (Input.GetMouseButtonUp(1) && isNewTurn && Hit_n_times != 0)
        {
            UpdateLines();
        }

    }

    [ContextMenu("Update Preview Lines")]
    public void UpdateLines()
    {



        // update all balls initial transform values to the current ones
        foreach (var transformData in initialPositions.Values)
        {
            transformData.Update();
        }

        // clear all prediction values
        foreach (var list in this.simulatedPositions.Values)
        {
            list.Clear();
        }
        Physics.autoSimulation = false;
        var somethingChanged = true;
        var simulatedTime = 0f;

        // ShootWhiteBall
        balls[0].Rigidbody.velocity = direction.normalized * force;

        while (somethingChanged && simulatedTime < maxPreviewTime)
        {
            // Simulate a physics step   
            Physics.Simulate(Time.fixedDeltaTime);

            // always assume there was no change (-> would break out of prediction loop)
            somethingChanged = false;

            foreach (var kvp in simulatedPositions)
            {
                var ball = kvp.Key;
                //
                ball.Line.positionCount = 0;
                //
                var positions = kvp.Value;
                var currentPosition = ball.Rigidbody.position;

                // either this is the first frame or the current position is different from the previous one
                var hasChanged = positions.Count == 0 || currentPosition != positions[positions.Count - 1];

                if (hasChanged)
                {
                    positions.Add(currentPosition);
                }

                // it is enough for only one ball to be moving to keep running the prediction loop
                somethingChanged = somethingChanged || hasChanged;
            }

            // increase the counter by one physics step
            simulatedTime += Time.fixedDeltaTime;
        }

        // Reset all balls to the initial state
        foreach (var kvp in initialPositions)
        {
            kvp.Key.Rigidbody.velocity = Vector3.zero;
            kvp.Key.Rigidbody.angularVelocity = Vector3.zero;
            kvp.Key.Rigidbody.position = kvp.Value.Position;
            kvp.Key.Rigidbody.rotation = kvp.Value.Rotation;
        }
        // apply the line renderers
        foreach (var kvp in simulatedPositions)
        {
            var ball = kvp.Key;
            var positions = kvp.Value;

            //if(ball.name == currentHitObject.name || ball.name == "CueBall")
            //{
            ball.Line.positionCount = positions.Count;
            ball.Line.SetPositions(positions.ToArray());
            //}
            //print(ball);
        }
        // re-enable the physics
        Physics.autoSimulation = true;
        hasTouched = false;
        isFoul = false;
    }
    void RayCast()
    {
        RaycastHit hit;
        if (Input.GetMouseButton(1))
        {
            if (Physics.SphereCast(CueBall.transform.position, ball_sc.radius, -offset2.normalized, out hit, 200f, layerMask, QueryTriggerInteraction.UseGlobal))
            {
                currentHitObject = hit.transform.gameObject;
                currentHitDistance = hit.distance;
                testBall.transform.position = CueBall.transform.position - offset2.normalized * currentHitDistance;

            }
            else
            {
                currentHitObject = null;
                currentHitDistance = 200f;
            }
        }
        if (currentHitObject == null)
            return;
    }
}
