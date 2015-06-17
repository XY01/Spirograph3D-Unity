using UnityEngine;
using System.Collections;

public class Constant_Rotation : MonoBehaviour
{
	public Vector3 m_RotationSpeeds;
	public Space m_Space = Space.Self;

	void Update ()
	{
		transform.Rotate (m_RotationSpeeds * Mathf.Deg2Rad, m_Space );
	}
}
