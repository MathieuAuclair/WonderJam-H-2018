using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour {
	List<ControlRoads> cities;
	List<Buildings> buildings;
	List<RoadRenderer> renderers;
	public List<GameObject> BuildingModels;
	public GameObject Roads;
	public GameObject Instances;
	public float BuildingSpace = 4.5f;
	public int NbOfCities = 3;
	public float RoadWidth;
	public float Tiling;
	public float MapSize = 100f;

	public Material RoadMaterial;
	public Material IntersectionMaterial;
	// Use this for initialization
	void Start () {
		buildings = new List<Buildings> ();
		cities = new List<ControlRoads> (); 
		renderers = new List<RoadRenderer> ();
		for (int i = 0; i < NbOfCities; i++) {
			var a = Object.Instantiate (Roads);
			var b = Object.Instantiate (Instances);
			a.transform.position = new Vector3 (300, 0, 300) * i;
			a.transform.parent = this.transform;
			b.transform.position = a.transform.position;
			b.transform.parent = this.transform;
			var buildings_controller = new Buildings () {
				Roads = a,
				Instances = b,
				BuildingSpace = BuildingSpace,
				BuildingModels = BuildingModels
			};
			var renderer1 = new RoadRenderer () {
				roads = a,
				RoadMaterial = RoadMaterial,
				IntersectionMaterial = IntersectionMaterial,
				RoadWidth = RoadWidth,
				Tiling = Tiling,
				Intersections = a.transform.GetChild(0).gameObject
			};
			var control = new ControlRoads () {
				roadRenderer = renderer1,
				MapSize = MapSize
			};
			control.MapAngle = 0;
			control.currentType = ControlRoads.GridType.X_Type;
			renderer1.Intersections.transform.position = new Vector3(renderer1.Intersections.transform.position.x, renderer1.Intersections.transform.position.y + 0.03f, renderer1.Intersections.transform.position.z);
			buildings_controller.control = control;
			buildings.Add (buildings_controller);
			cities.Add (control);
			renderers.Add (renderer1);
		}

		for (int i = 0; i < NbOfCities; i++) {
			renderers [i].Start ();
			cities[i].GenerateMap ();
			buildings[i].Clear ();
			buildings[i].GenerateBuildings ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < NbOfCities; i++) {
			buildings [i].Update ();
		}
	}
}
