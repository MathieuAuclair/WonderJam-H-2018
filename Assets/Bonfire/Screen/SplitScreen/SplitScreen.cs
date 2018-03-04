using System.Collections.Generic;
using UnityEngine;

namespace Bonfire
{
    public enum Orientation
    {
        HORIZONTAL,
        VERTICAL
    }

    public class SplitScreen : Screen
    {
        const TwoByTwoDynamicRectGrid.CellQuadrant TOP_LEFT = TwoByTwoDynamicRectGrid.CellQuadrant.TOP_LEFT;
        const TwoByTwoDynamicRectGrid.CellQuadrant TOP_RIGHT = TwoByTwoDynamicRectGrid.CellQuadrant.TOP_RIGHT;
        const TwoByTwoDynamicRectGrid.CellQuadrant BOTTOM_LEFT = TwoByTwoDynamicRectGrid.CellQuadrant.BOTTOM_LEFT;
        const TwoByTwoDynamicRectGrid.CellQuadrant BOTTOM_RIGHT = TwoByTwoDynamicRectGrid.CellQuadrant.BOTTOM_RIGHT;

        public class View
        {
            public Camera camera;
            public TwoByTwoDynamicRectGrid.CellQuadrant quadrant;

            public View(Camera cam, TwoByTwoDynamicRectGrid.CellQuadrant pos)
            {
                camera = cam;
                quadrant = pos;
            }

            public bool isAt(TwoByTwoDynamicRectGrid.CellQuadrant position)
            {
                return this.quadrant == position;
            }
        }

        [SerializeField] TwoByTwoDynamicRectGrid dynamicRectGrid;
        [SerializeField] PlayerView basePlayerCamera;

        IDictionary<Transform, View> mappedViews = new Dictionary<Transform, View>();
        TwoByTwoDynamicRectGrid.CellQuadrant[] orderedPreferredPositions = new TwoByTwoDynamicRectGrid.CellQuadrant[]
        {
            TOP_LEFT,
            BOTTOM_LEFT,
            BOTTOM_RIGHT,
            TOP_RIGHT
        };

        public override void Initialize()
        {
        }

        public override void Register(Transform screenedEntity)
        {
            InitializeAndMapViewTo(screenedEntity);
        }

        public override void Update()
        {
            dynamicRectGrid.Update();
            foreach (var view in mappedViews.Values)
            {
				if (view.camera != null) {
					view.camera.rect = dynamicRectGrid.GetRectAt(view.quadrant);
				}
            }
        }

        TwoByTwoDynamicRectGrid.CellQuadrant GetNextPreferredQuadrant()
        {
            var nextPreferredPosition = TwoByTwoDynamicRectGrid.CellQuadrant.NONE;
            foreach (var position in orderedPreferredPositions)
                if (IsScreenQuadrantVacant(position) && nextPreferredPosition == TwoByTwoDynamicRectGrid.CellQuadrant.NONE)
                    nextPreferredPosition = position;
            return nextPreferredPosition;
        }

        bool IsScreenQuadrantVacant(TwoByTwoDynamicRectGrid.CellQuadrant quadrant)
        {
            bool isVacant = true;
            foreach (var view in mappedViews.Values)
                isVacant &= view.quadrant != quadrant;
            return isVacant;
        }

        public override void Unregister(Transform screenedEntity)
        {
		
        }

        View InitializeAndMapViewTo(Transform key)
        {
            var quadrant = GetNextPreferredQuadrant();
            var playerView = Object.Instantiate(basePlayerCamera);
            var camera = playerView.View;
            playerView.SetTarget(key);
            var view = new View(camera, quadrant);
            mappedViews.Add(key, view);
            dynamicRectGrid.SetRectActive(quadrant, true);
            return view;
        }

        bool IsAViewAt(TwoByTwoDynamicRectGrid.CellQuadrant position)
        {
            bool isAtPosition = false;
            foreach (var view in mappedViews.Values)
                isAtPosition |= view.isAt(position);
            return isAtPosition;
        }

        public override void CleanUp()
        {
            foreach (var view in mappedViews.Values)
            {
				Destroy(view.camera.transform.parent.gameObject);
            }
        }
    }
}