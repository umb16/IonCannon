using System;
using System.Collections.Generic;
using UnityEngine;

public class MobOld : MonoBehaviour
{
	public GameObject ParticlePrefab;

	public GameObject ParticlePrefab2;

	public GameObject ParticlePrefab3;

	private Renderer _mobRenderer;

	public float Speed = 1f;

	public float DmgAgrSpeed;

	public float Hp = 3f;

	public float Radiation;

	private float _maxHp;

	private float _maxSpeed;

	public int ScoreCost = 10;

	private Animator _anim;

	public bool Bloody;

	public bool Stalker;

	public float HunterSpeed;

	public static int ComboCount;

	public static float ComboTimer;

	public GameObject Child;

	private float _childTimer;

	private Timer SpeedUpEnd;

	private List<GameObject> _listOfChilds = new List<GameObject>();

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "SpeedUp" && Speed < 1.5f)
		{
			Speed = 1.5f;
			Animator anim = _anim;
			float speed = Speed;
			Vector3 localScale = transform.localScale;
			anim.speed = speed * localScale.x;
			if (SpeedUpEnd != null)
			{
				SpeedUpEnd = new Timer(()=>
				{
					Speed = _maxSpeed;
				}, 1f);
			}
		}
		if (collision.gameObject.tag == "Ray")
		{
			Hp -= Player.Self.RayDmg * Mathf.Max(0.25f, Player.Self.RaySplash * 1.25f - Vector3.Distance(transform.position, RayScript.RayTransform.position));
			DmgCheck(0);
		}
		if (collision.gameObject.tag == "Barrel")
		{
			Hp -= 9f;
			DmgCheck(2);
			UnityEngine.Object.Destroy(collision.gameObject.transform.parent.gameObject);
		}
	}

	private void CreateChild()
	{
		if (_listOfChilds.Count < 5)
		{
			Vector2 vector = new Vector2(UnityEngine.Random.value * 2f - 1f, UnityEngine.Random.value * 2f - 1f);
			vector.Normalize();
			vector *= 2f;
			float x = vector.x;
			Vector3 position = transform.position;
			vector.x = x + position.x;
			float y = vector.y;
			Vector3 position2 = transform.position;
			vector.y = y + position2.y;
			GameObject gameObject = UnityEngine.Object.Instantiate(Child, new Vector3(vector.x, vector.y, -0.5f), Quaternion.identity) as GameObject;
			_listOfChilds.Add(gameObject);
			MobOld component = gameObject.GetComponent<MobOld>();
			component.Hp = Hp / 20f;
			if (UnityEngine.Random.value < 0.05f)
			{
				gameObject.transform.localScale *= 1.5f;
				component.Hp *= 1.5f;
				component.ScoreCost = (int)((float)component.ScoreCost * 1.5f);
			}
		}
		else
		{
			for (int i = 0; i < _listOfChilds.Count; i++)
			{
				if (_listOfChilds[i] == null)
				{
					_listOfChilds.RemoveAt(i);
					i--;
				}
			}
		}
		Invoke("CreateChild", 1f);
	}

	private void DmgCheck(int type)
	{
		if (Hp <= 0f)
		{
			UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate(ParticlePrefab, transform.position + Vector3.back * 0.5f, ParticlePrefab.transform.rotation), 10f);
			UnityEngine.Object.Destroy(base.gameObject);
			Score.CurrentScore += (int)((float)ScoreCost * (1f + 0.1f * (float)ComboCount));
			ComboCount++;
			MobGenerator.Self.WaveMobCounter++;
			ComboTimer = 0f;
			return;
		}
		if (type == 0)
		{
			Radiation += Player.Self.Radiation * Mathf.Max(0.25f, Player.Self.RaySplash * 1.25f - Vector3.Distance(transform.position, RayScript.RayTransform.position));
			if (Radiation > 0f)
			{
				Timer timer = null;
				timer = new Timer(()=>
				{
					if (transform == null)
					{
						timer.Stop();
					}
					else
					{
						Destroy(Instantiate(ParticlePrefab2, transform.position + Vector3.back * 0.5f, ParticlePrefab2.transform.rotation), 10f);
					}
				}, 1f);
			}
		}
		if (type == 0 || type == 2)
		{
			UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate(ParticlePrefab2, transform.position + Vector3.back * 0.5f, ParticlePrefab2.transform.rotation), 10f);
		}
		if (DmgAgrSpeed > 0f)
		{
			Speed = DmgAgrSpeed;
			Animator anim = _anim;
			float num = Speed * 1f;
			Vector3 localScale = transform.localScale;
			anim.speed = num / localScale.x;
		}
	}

	private void Start()
	{
		_mobRenderer = GetComponentInChildren<Renderer>();
		_maxSpeed = Speed;
		_maxHp = 3f;
		_anim = GetComponentInChildren<Animator>();
		Animator anim = _anim;
		float num = Speed * 1f;
		Vector3 localScale = transform.localScale;
		anim.speed = num / localScale.x;
		if (Child != null)
		{
			CreateChild();
		}
		if (ParticlePrefab3 != null)
		{
			UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate(ParticlePrefab3, transform.position + Vector3.back * 0.5f, ParticlePrefab3.transform.rotation), 10f);
		}
	}

	private void Update()
	{
		if (!MainMenu.gameIsStart)
		{
			_anim.speed = 0f;
			return;
		}
		transform.position += (Player.Self.transform.position - transform.position).normalized * Time.deltaTime * Speed;
		float num = Vector2.Angle(Vector2.up, Player.Self.transform.position - transform.position);
		Vector3 position = Player.Self.transform.position;
		float x = position.x;
		Vector3 position2 = transform.position;
		if (x > position2.x)
		{
			num *= -1f;
		}
		transform.eulerAngles = new Vector3(0f, 0f, num);
		if (Bloody)
		{
			Hp += Time.deltaTime;
			if (_maxHp < Hp)
			{
				Hp = _maxHp;
			}
		}
		if (Radiation > 0f)
		{
			Hp -= Radiation * Time.deltaTime;
			DmgCheck(1);
		}
		if (HunterSpeed > 0f)
		{
			if (Vector3.Distance(transform.position, Player.Self.transform.position) < 6f)
			{
				Speed = HunterSpeed;
				Animator anim = _anim;
				float num2 = Speed * 1f;
				Vector3 localScale = transform.localScale;
				anim.speed = num2 / localScale.x;
			}
			else
			{
				Speed = _maxSpeed;
				Animator anim2 = _anim;
				float num3 = Speed * 1f;
				Vector3 localScale2 = transform.localScale;
				anim2.speed = num3 / localScale2.x;
			}
		}
		if (Stalker)
		{
			if (Vector3.Distance(transform.position, Player.Self.transform.position) < 6f)
			{
				_mobRenderer.enabled = !_mobRenderer.enabled;
			}
			else
			{
				_mobRenderer.enabled = false;
			}
		}
	}
}
