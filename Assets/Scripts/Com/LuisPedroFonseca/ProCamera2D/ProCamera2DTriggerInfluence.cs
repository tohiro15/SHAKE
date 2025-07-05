using System.Collections;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D
{
	[HelpURL("http://www.procamera2d.com/user-guide/trigger-influence/")]
	public class ProCamera2DTriggerInfluence : BaseTrigger
	{
		public static string TriggerName = "Influence Trigger";

		public Transform FocusPoint;

		public float InfluenceSmoothness = 0.3f;

		[Range(0f, 1f)]
		public float ExclusiveInfluencePercentage = 0.25f;

		private Vector2 _influence;

		private Vector2 _velocity;

		private Vector3 _exclusivePointVelocity;

		private Vector3 _tempExclusivePoint;

		private void Start()
		{
			if (FocusPoint == null)
			{
				FocusPoint = base.transform.Find("FocusPoint");
			}
			if (FocusPoint == null)
			{
				FocusPoint = base.transform;
			}
		}

		protected override void EnteredTrigger()
		{
			base.EnteredTrigger();
			StartCoroutine(InsideTriggerRoutine());
		}

		protected override void ExitedTrigger()
		{
			base.ExitedTrigger();
			StartCoroutine(OutsideTriggerRoutine());
		}

		private IEnumerator InsideTriggerRoutine()
		{
			yield return base.ProCamera2D.GetYield();
			float previousDistancePercentage = 1f;
			_tempExclusivePoint = VectorHV(Vector3H(base.ProCamera2D.transform.position), Vector3V(base.ProCamera2D.transform.position));
			while (_insideTrigger)
			{
				_exclusiveInfluencePercentage = ExclusiveInfluencePercentage;
				float distanceToCenterPercentage = GetDistanceToCenterPercentage(new Vector2(Vector3H(base.ProCamera2D.TargetsMidPoint), Vector3V(base.ProCamera2D.TargetsMidPoint)));
				Vector2 a = new Vector2(Vector3H(base.ProCamera2D.TargetsMidPoint) + Vector3H(base.ProCamera2D.TargetsMidPoint) - Vector3H(base.ProCamera2D.PreviousTargetsMidPoint), Vector3V(base.ProCamera2D.TargetsMidPoint) + Vector3V(base.ProCamera2D.TargetsMidPoint) - Vector3V(base.ProCamera2D.PreviousTargetsMidPoint)) - new Vector2(Vector3H(FocusPoint.position), Vector3V(FocusPoint.position));
				if (distanceToCenterPercentage == 0f)
				{
					base.ProCamera2D.ExclusiveTargetPosition = Vector3.SmoothDamp(_tempExclusivePoint, VectorHV(Vector3H(FocusPoint.position), Vector3V(FocusPoint.position)), ref _exclusivePointVelocity, InfluenceSmoothness);
					_tempExclusivePoint = base.ProCamera2D.ExclusiveTargetPosition.Value;
					_influence = -a * (1f - distanceToCenterPercentage);
					base.ProCamera2D.ApplyInfluence(_influence);
				}
				else
				{
					if (previousDistancePercentage == 0f)
					{
						_influence = new Vector2(Vector3H(base.ProCamera2D.CameraTargetPositionSmoothed), Vector3V(base.ProCamera2D.CameraTargetPositionSmoothed)) - new Vector2(Vector3H(base.ProCamera2D.TargetsMidPoint) + Vector3H(base.ProCamera2D.TargetsMidPoint) - Vector3H(base.ProCamera2D.PreviousTargetsMidPoint), Vector3V(base.ProCamera2D.TargetsMidPoint) + Vector3V(base.ProCamera2D.TargetsMidPoint) - Vector3V(base.ProCamera2D.PreviousTargetsMidPoint)) + new Vector2(Vector3H(base.ProCamera2D.ParentPosition), Vector3V(base.ProCamera2D.ParentPosition));
					}
					_influence = Vector2.SmoothDamp(_influence, -a * (1f - distanceToCenterPercentage), ref _velocity, InfluenceSmoothness, float.PositiveInfinity, Time.deltaTime);
					base.ProCamera2D.ApplyInfluence(_influence);
					_tempExclusivePoint = VectorHV(Vector3H(base.ProCamera2D.CameraTargetPosition), Vector3V(base.ProCamera2D.CameraTargetPosition)) + VectorHV(Vector3H(base.ProCamera2D.ParentPosition), Vector3V(base.ProCamera2D.ParentPosition));
				}
				previousDistancePercentage = distanceToCenterPercentage;
				yield return base.ProCamera2D.GetYield();
			}
		}

		private IEnumerator OutsideTriggerRoutine()
		{
			yield return base.ProCamera2D.GetYield();
			while (!_insideTrigger && _influence != Vector2.zero)
			{
				_influence = Vector2.SmoothDamp(_influence, Vector2.zero, ref _velocity, InfluenceSmoothness, float.PositiveInfinity, Time.deltaTime);
				base.ProCamera2D.ApplyInfluence(_influence);
				yield return base.ProCamera2D.GetYield();
			}
		}
	}
}
