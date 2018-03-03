using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ControlRoads : MonoBehaviour {

	private RoadNetwork network {get;set;}
	private RoadRenderer roadRenderer {get;set;}

	public List<RoadSegment> RoadSegments {get; private set;}
	public List<Intersection> Intersections {get; private set;}
	public float MapSize = 100f;

	public Slider GridSlider;
	public Text GridTypeText;

	public Buildings buildings;

	public enum GridType {X_Type, Y_Type, O_Type};
	public GridType currentType = GridType.X_Type;

	public static bool a = false;

	// Use this for initialization
	void Start () 
	{
		this.buildings = this.buildings.GetComponent<Buildings> ();
		this.GenerateMap ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
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
		a = false;
		this.buildings.Clear ();

		this.network = new RoadNetwork (this.MapSize);
		if(this.currentType == GridType.X_Type)
			this.network.AddCityCentreX (new Vector2(0,0), 120f);
		else if(this.currentType == GridType.Y_Type)
			this.network.AddCityCentreY (new Vector2(0,0), 120f);
		else if(this.currentType == GridType.O_Type)
			this.network.AddCityCentreO (new Vector2(0,0), 120f);

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
		a = true;
	}
}
