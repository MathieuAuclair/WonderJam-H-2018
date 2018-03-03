using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ControlRoads : MonoBehaviour {

	private RoadNetwork network {get;set;}
	private RoadRenderer roadRenderer {get;set;}

	public List<RoadSegment> RoadSegments {get; private set;}
	public List<Intersection> Intersections {get; private set;}
	public float MapSize = 100f;
	public float MapAngle = 120f; 

	public Slider GridSlider;
	public Text GridTypeText;

	public Buildings buildings;

	public enum GridType {X_Type, Y_Type, O_Type};
	private GridType currentType;

	public static bool generatorIdle = false;

	// Use this for initialization
	void Start () 
	{
		this.buildings = this.buildings.GetComponent<Buildings> ();
		this.currentType = RandomEnumValue<GridType> ();
		this.GenerateMap ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	static T RandomEnumValue<T> ()
	{
		var v = Enum.GetValues (typeof (T));
		return (T) v.GetValue (new System.Random ().Next(v.Length));
	}

	public void GridTypeClick()
	{
		this.currentType = (GridType)this.GridSlider.value;
		this.GridTypeText.text = this.currentType.ToString ().Replace('_','-');
	}

	public void GenerateMap()
	{
		//if (Buildings.Splitting)
		//	return;
		generatorIdle = false;
		this.buildings.Clear ();

		this.network = new RoadNetwork (this.MapSize);
		if(this.currentType == GridType.X_Type)
			this.network.AddCityCentreX (new Vector2(0,0), MapAngle);
		else if(this.currentType == GridType.Y_Type)
			this.network.AddCityCentreY (new Vector2(0,0), MapAngle);
		else if(this.currentType == GridType.O_Type)
			this.network.AddCityCentreO (new Vector2(0,0), MapAngle);

		this.network.SplitSegments (0);
		this.network.SplitSegments (0);
		this.network.SplitSegments (1);
		this.network.SplitSegments (1);
		this.network.SplitSegments (2);
		this.network.SplitSegments (3);
		
		this.roadRenderer = this.GetComponent<RoadRenderer> ();
		this.roadRenderer.ClearData ();

		foreach (RoadSegment segment in this.network.RoadSegments)
            this.roadRenderer.AddRoadSegments(segment);

		foreach (Intersection inter in this.network.RoadIntersections)
			this.roadRenderer.AddIntersection (inter);

		this.RoadSegments = new List<RoadSegment> (this.network.RoadSegments);
		generatorIdle = true;
	}
}
