using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Random = UnityEngine.Random;


public class MowerAgentObj : Agent
{
    // public int c = 0;
    public int currentEpisode = 0;
    public float remainLength = 0.3f;
    public float endAtPercentage = 0.9f;
    public float cumsumReward = 0f;
    public Vector3 grassRandomness;
    public float grassSpawnMul = 0.95f;

    GameObject[] grasses;
    LawnMowerSettings m_LawnMowerSettings;
    Rigidbody m_AgentRb;
    // EnvironmentParameters m_ResetParams;

    void Awake()
    {
        m_LawnMowerSettings = FindObjectOfType<LawnMowerSettings>();
        grasses = GameObject.FindGameObjectsWithTag("Grass");
        Debug.Log(grasses.Length);
    }

    public override void Initialize()
    {
        // Cache the agent rigidbody
        m_AgentRb = GetComponent<Rigidbody>();

        // m_ResetParams = Academy.Instance.EnvironmentParameters;

    }

    /// <summary>
    /// Moves the agent according to the selected action.
    /// </summary>
    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];
        var rotationAction = act[1];

        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            // case 2:
            //     dirToGo = transform.forward * -1f;
            //     break;
            case 2:
                dirToGo = transform.right * -1f;
                break;
            case 3:
                dirToGo = transform.right * 1f;
                break;
        }

        switch (rotationAction)
        {
            case 1:
                rotateDir = transform.up * -1f;
                break;
            case 2:
                rotateDir = transform.up * 1f;
                break;
        }

        transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f * m_LawnMowerSettings.agentRotationSpeed);
        m_AgentRb.AddForce(dirToGo * m_LawnMowerSettings.agentRunSpeed,
            ForceMode.VelocityChange);
    }

    /// <summary>
    /// Called every step of the engine. Here the agent takes an action.
    /// </summary>
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Move the agent using the action.
        MoveAgent(actionBuffers.DiscreteActions);

        // Penalty given each step to encourage agent to finish task quickly.
        AddReward(-1f / MaxStep);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grass") && collision.gameObject.transform.localScale.y > 0.4f)
        {
            Debug.Log("Collide");
            collision.gameObject.transform.localScale = new Vector3(1, remainLength, 1);
            float reward = 1f / grasses.Length / endAtPercentage;
            AddReward(reward);
            cumsumReward += reward;
            if (cumsumReward >= 1)
            {
                EndEpisode();
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        // else if (Input.GetKey(KeyCode.S))
        // {
        //     discreteActionsOut[0] = 2;
        // }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 2;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 3;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            discreteActionsOut[1] = 1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            discreteActionsOut[1] = 2;
        }
    }

    public Vector3 GetRandomSpawnPos()
    {
        float mul = m_LawnMowerSettings.spawnAreaMarginMultiplier;
        float areaWidth = 5f;
        Vector3 randomSpawnPos = new Vector3(
            Random.Range(-areaWidth * mul, areaWidth * mul),
            transform.position.y,
            Random.Range(-areaWidth * mul, areaWidth * mul));
        return randomSpawnPos;

    }

    /// <summary>
    /// In the editor, if "Reset On Done" is checked then AgentReset() will be
    /// called automatically anytime we mark done = true in an agent script.
    /// </summary>
    public override void OnEpisodeBegin()
    {
        m_AgentRb.velocity = Vector3.zero;
        m_AgentRb.angularVelocity = Vector3.zero;
        ResetGrass();
        transform.localPosition = GetRandomSpawnPos();
        // Debug.Log(c);
        cumsumReward = 0f;
        currentEpisode++;
    }

    private void ResetGrass()
    {
        foreach (var grass in grasses)
        {
            float areaWidth = 5f;

            grass.transform.position = new Vector3(
                Random.Range(-areaWidth * grassSpawnMul, areaWidth * grassSpawnMul),
                grass.transform.position.y,
                Random.Range(-areaWidth * grassSpawnMul, areaWidth * grassSpawnMul));

            grass.transform.localScale = new Vector3(
                Random.Range(1f - grassRandomness.x, 1f + grassRandomness.x),
                Random.Range(1f - grassRandomness.y, 1f + grassRandomness.y),
                Random.Range(1f - grassRandomness.z, 1f + grassRandomness.z));
        }
    }

}