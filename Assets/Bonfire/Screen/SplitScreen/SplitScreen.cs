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
        [SerializeField] Camera basePlayerCamera;

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
		
            Transform cameraHolder = GetCameraHolder(screenedEntity);

            var entityView = InitializeAndMapViewTo(screenedEntity);

            // TODO: CAMERA SHOULD NOT BE A CHILD OF THE GAME OBJECT
            //		It should NOT have a "Cameraman" component which takes the character's transform as a "target".
            //      
            //		It could make itself a child of the character, but preferably not: it should prioritize moving
            //		accordingly to its target transform location and rotation.
            //		Even then, a cameraman should wrap a "Follower" since there is no need for a camera to follow something.
            entityView.camera.transform.AddAndFitTo(cameraHolder);
            // END TODO
        }

        public override void Update()
        {
            dynamicRectGrid.Update();
            foreach (var view in mappedViews.Values)
            {
                view.camera.rect = dynamicRectGrid.GetRectAt(view.quadrant);
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

        Transform GetCameraHolder(Transform screenedEntity)
        {
            Cameraman cameraman = screenedEntity.GetComponentInChildren<Cameraman>();
            if (cameraman == null)
            {
                Debug.LogWarningFormat("No Cameraman component found in {0}'s children. Using {0}'s Transform instead.", screenedEntity.name);
                return screenedEntity;
            }
            return cameraman.transform;
        }

        View InitializeAndMapViewTo(Transform key)
        {
            var quadrant = GetNextPreferredQuadrant();
            var camera = (Camera)GameObject.Instantiate(basePlayerCamera);
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
    }
}