using UnityEngine;
using System.Collections;

public class Spirograph : MonoBehaviour 
{
	public int m_NumberOfIterations = 4;
	public float m_IterationRadiusScaler = .33f;

	public Vector3 m_Rotation = new Vector3( 0, 0, 30 );

	public float m_RotationScaler = 2;

	Constant_Rotation[] m_SpiroIterations;

	public GameObject m_SpiroPrefab;
		
	bool m_AltRotation = false;

	void Start ()
	{
		CreatePivots();
	}

	void CreatePivots()
	{
		if( m_SpiroIterations != null )
		{
			if( m_SpiroIterations.Length > 0 )
			{
				for (int i = 1; i < m_SpiroIterations.Length; i++)
				{
					Destroy( m_SpiroIterations[ i ].gameObject );
				}
			}
		}

		m_SpiroIterations = new Constant_Rotation[ m_NumberOfIterations + 1 ];
		m_SpiroIterations[0] = gameObject.GetComponent< Constant_Rotation >();

		for( int i = 0; i < m_NumberOfIterations; i++ )
		{
			float radius = m_IterationRadiusScaler;
			float xPos = -( 1 - m_IterationRadiusScaler )/2;

			GameObject newPivot = Instantiate( m_SpiroPrefab ) as GameObject;

			if( i == 0 )
				newPivot.transform.parent = transform;
			else
				newPivot.transform.parent = m_SpiroIterations[ i ].transform;

			newPivot.transform.localRotation = Quaternion.Euler( Vector3.zero );

			newPivot.transform.localScale = Vector3.one * radius;
			newPivot.transform.localPosition = new Vector3( xPos, 0, 0 );

			Constant_Rotation newRot = newPivot.AddComponent< Constant_Rotation >() as Constant_Rotation;
			newRot.m_RotationSpeeds = m_Rotation * i;

			m_SpiroIterations[ i + 1 ] = newRot;
		}
	}

	int prevIterations;
	void Update () 
	{
		if( m_NumberOfIterations != prevIterations )
			CreatePivots();

		for( int i = 0; i < m_SpiroIterations.Length; i++ )
		{		
			Vector3 rot = m_Rotation;

			if( i != 0 )
			{
				if( m_AltRotation )				
					rot = -( m_SpiroIterations[ i - 1 ].m_RotationSpeeds * m_RotationScaler );
				else
					rot = m_SpiroIterations[ i - 1 ].m_RotationSpeeds * m_RotationScaler;
			}

			m_SpiroIterations[i].m_RotationSpeeds = rot;

			if( i != 0 )
			{
				float radius = m_IterationRadiusScaler;
				float xPos = -( 1 - m_IterationRadiusScaler )/2;			
				m_SpiroIterations[i].transform.localScale = Vector3.one * radius;
				m_SpiroIterations[i].transform.localPosition = new Vector3( xPos, 0, 0 );
			}
		}

		prevIterations = m_NumberOfIterations;
	}


	void OnGUI()
	{
		GUILayout.BeginArea( new Rect( 0, 0, 200, 350 ) );
		GUILayout.BeginVertical( "box" );
		m_Rotation.x = LabeledSliderHorizontal( m_Rotation.x, "Rotation X: " + m_Rotation.x.RoundToOneDecimalPlace(), 0, 90 );
		m_Rotation.y = LabeledSliderHorizontal( m_Rotation.y, "Rotation Y: " + m_Rotation.y.RoundToOneDecimalPlace(), 0, 90 );
		m_Rotation.z = LabeledSliderHorizontal( m_Rotation.z, "Rotation Z: " + m_Rotation.z.RoundToOneDecimalPlace(), 0, 90 );
		GUILayout.Space( 10 );

		m_NumberOfIterations = (int)LabeledSliderHorizontal( m_NumberOfIterations, "Iterations: " + m_NumberOfIterations, 0, 6 );
		m_IterationRadiusScaler = LabeledSliderHorizontal( m_IterationRadiusScaler, "Iteration Scaler: " + m_IterationRadiusScaler.ToDoubleDecimalString(), .01f, .99f );
		m_RotationScaler = LabeledSliderHorizontal( m_RotationScaler, "Rotation Scaler: " + m_RotationScaler.ToDoubleDecimalString(), .5f, 4 );

		m_AltRotation = GUILayout.Toggle( m_AltRotation, "Alt rotation" );

		if( GUILayout.Button( "Reset rotation" ) )
		{
			for( int i = 0; i < m_SpiroIterations.Length; i++ )
			{
				m_SpiroIterations[ i ].transform.localRotation = Quaternion.identity;
			}
		}

		GUILayout.EndVertical(  );

		GUILayout.EndArea();
	}

	float LabeledSliderHorizontal( float val, string label, float min, float max )		
	{
		GUILayout.BeginHorizontal( GUILayout.Height( 15 ) );
		{
			GUILayout.Label( label );			
			val = GUILayout.HorizontalSlider( val, min, max, GUILayout.Width( 70 ) );
		}
		GUILayout.EndHorizontal();
		
		return val;
	}
}
