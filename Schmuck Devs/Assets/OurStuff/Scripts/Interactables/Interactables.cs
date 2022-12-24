using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Interactables : MonoBehaviour
{
	/// <summary>
	///		Note: The Collider on interactables Should be a bit higher as it grabs from the camera position for the
	///		ray and not the mouse pointer. So it's relative to the camera position, to fix this issue as a little 
	///		hack I can make the collider about .25 times higher on the y - axis and -.25 on the x = axis (just keep this in mind).
	/// </summary>
	[Header("Can Be Interupted If Clicked In Between Animation:")]
	[SerializeField] private bool canBeInterupted = false;
	private bool doingSomething = false;
	private Coroutine doingSomethingCo = null;
	private Coroutine playParticleEffectCo = null;
	[Header("Scale:")]
	[SerializeField] private Vector3 maxValueScale = new Vector3(1f, 1f, 1f);
	[SerializeField] private Vector3 minValueScale = new Vector3(.666f, .666f, .666f);
	[Header("ScaleSpeed:")]
	[SerializeField] private float baseScaleSpeed = 0f;
	[Header("ParticleEffect:")]
	[SerializeField] private bool hasAParticleEffect = false;
	[Tooltip("Leave Blank (null) if it (the interactable Object) does not have one.")]
	public Transform particleEffectPrefab = null;
	[SerializeField] private float particleEffectDuration = 1f;
	[SerializeField] private GameObject instantiateHolder = null;
	public bool doSomething()
	{
		if (!doingSomething)
		{
			doingSomethingCo = StartCoroutine(simpleLerpScaleDown());
			if (hasAParticleEffect)
				playParticleEffectCo = StartCoroutine(playParticleEffect());
			return true;
		}
		//Then the coroutine is currently running:
		if(canBeInterupted)
		{
			if(playParticleEffectCo != null)
				StopCoroutine(playParticleEffectCo);
			if(doingSomethingCo != null)
				StopCoroutine(doingSomethingCo);
			doingSomethingCo = StartCoroutine(simpleLerpScaleDown());
			if (hasAParticleEffect)
				playParticleEffectCo = StartCoroutine(playParticleEffect());
			return true;
		}
		return false;
	}
	private IEnumerator playParticleEffect()
	{
		var obj = Instantiate(particleEffectPrefab, transform);
		obj.parent = instantiateHolder.transform;
		obj.GetComponent<ParticleSystemRenderer>().material = this.gameObject.GetComponent<MeshRenderer>().material;
		yield return new WaitForSeconds(particleEffectDuration - 0.02f);
		Destroy(obj.gameObject);
	}
	private IEnumerator simpleLerpScaleDown()
	{
		doingSomething = true;
		float tTime = 0f;
		while(tTime < 1f)
		{
			tTime += Time.deltaTime / baseScaleSpeed;
			this.transform.localScale = Vector3.Lerp(this.transform.localScale, minValueScale, tTime);
			yield return null;
		}
		tTime = 0f;
		while (tTime < 1f)
		{
			tTime += Time.deltaTime / baseScaleSpeed;
			this.transform.localScale = Vector3.Lerp(this.transform.localScale, maxValueScale, tTime);
			yield return null;
		}
		doingSomething = false;
	}
}
