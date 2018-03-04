using UnityEngine;

[System.Serializable]
public class TorsoModule : CharacterModule
{
    [SerializeField] Rigidbody torso;
    [SerializeField] float power;

    public void Rotate(float h, float v)
    {
        Vector3 targetDirection = new Vector3(h, 0, v);
        //torso.AddForceAtPosition(targetDirection * power, torso.transform.forward * power + torso.position);
        torso.transform.LookAt(targetDirection + torso.position);
    }
}
