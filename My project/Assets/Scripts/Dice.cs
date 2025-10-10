using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Dice : MonoBehaviour
{
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private Rigidbody rb;
    
    private int score;

    public Dice(Vector3 pos) => Instantiate(dicePrefab, pos, quaternion.identity);
    
    public bool IsStop() => rb.linearVelocity == Vector3.zero && rb.angularVelocity == Vector3.zero;
    
    public int CountScore(LayerMask plane)
    {
        Vector3[] directions =
        {
            -transform.forward,
            -transform.up,
            transform.right,
            transform.right,
            transform.up,
            transform.forward
        };

        for (int i = 0; i < directions.Length; i++)
        {
            if (Physics.Raycast(transform.position, directions[i], 1, plane))
                return i + 1;
        }
        return 0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
