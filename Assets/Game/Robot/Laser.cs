using UnityEngine;

public class Laser : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        var building = col.GetComponent<Destruction>();
        if (building != null)
        {
            building.ExplodeWithForce(20);
            ScoreBoard.IncreaseScore(GetComponentInParent<PlayerController>().PlayerId, 1);
        }
    }
}
