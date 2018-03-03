using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ControlRoads {

	private RoadNetwork network {get;set;}
	public RoadRenderer roadRenderer {get;set;}

	public List<RoadSegment> RoadSegments {get; private set;}
	public List<Intersection> Intersections {get; private set;}
	public float MapSize = 100f;
	public float MapAngle = 120f;

	public Slider GridSlider;
	public Text GridTypeText;

	public enum GridType {X_Type, Y_Type, O_Type};
	private GridType currentType;

	public ControlRoads(){
		this.currentType = RandomEnumValue<GridType> ();
	}

	static T RandomEnumValue<T> ()
	{
		var v = Enum.GetValues (typeof (T));
		return (T) v.GetValue (UnityEngine.Random.Range(0, v.Length));
	}

	public void GridTypeClick()
	{
		this.currentType = (GridType)this.GridSlider.value;
		this.GridTypeText.text = this.currentType.ToString ().Replace('_','-');
	}

	public void GenerateMap()
	{
		this.network = new RoadNetwork (this.MapSize);
		if(this.currentType == GridType.X_Type)
			this.network.AddCityCentreX (Vector2.zero, MapAngle);
		else if(this.currentType == GridType.Y_Type)
			this.network.AddCityCentreY (Vector2.zero, MapAngle);
		else if(this.currentType == GridType.O_Type)
			this.network.AddCityCentreO (Vector2.zero, MapAngle);

		this.network.SplitSegments (0);
		this.network.SplitSegments (0);
		this.network.SplitSegments (1);
		this.network.SplitSegments (1);
		this.network.SplitSegments (2);
		this.network.SplitSegments (3);

		this.roadRenderer.ClearData ();
		List<RoadSegment> farSegments = new List<RoadSegment> ();
		foreach (RoadSegment segment in this.network.RoadSegments) {
			segment.DistancePointA = Vector3.Distance (Vector3.zero, segment.PointA.point);
			segment.DistancePointB = Vector3.Distance (Vector3.zero, segment.PointB.point);
			if (segment.DistancePointA > MapSize || segment.DistancePointB > MapSize) {
				farSegments.Add (segment);
			}
			this.roadRenderer.AddRoadSegments(segment);
		}

		foreach (Intersection inter in this.network.RoadIntersections)
			this.roadRenderer.AddIntersection (inter);

		this.RoadSegments = new List<RoadSegment> (this.network.RoadSegments);
	}
}
