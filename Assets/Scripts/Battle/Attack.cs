using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class Attack : MonoBehaviour{
	public BattleSystem bS;
	public Animator Anim;
	[HideInInspector] public Vector3 AttackerStartPosition;
	[HideInInspector] public Vector3 targetPosition;
	[HideInInspector] public Vector3 AttackerPosition;
	[Header("Basic Setting")]
	[SerializeField]public Character charClass;
	[SerializeField]public float MovementSpeed = 0f;
	float timeOfLastAttack = 0f;
	[SerializeField]public bool canAttack;
	[SerializeField]public bool canMove;
	[SerializeField]public bool canRecharge;
	public bool isAttacked;
	public bool isSkillOn;
	[HideInInspector]public bool xPositionDone = false, zPositionDone = false;
	[SerializeField]public GameObject target;
	int randomnum;
	Timer timer;

	public CharacterBattleStatus battleStatus;
	public SkillHandler skillHandler;


	void Start(){
		charClass = gameObject.GetComponent<Character> ();
		Anim = gameObject.GetComponent<Animator> ();
		canAttack = false;
		canMove = false;
		canRecharge = true;
		AttackerPosition = gameObject.transform.position;
		AttackerStartPosition = gameObject.transform.position;
		MovementSpeed = 0f;
		timer = gameObject.AddComponent<Timer> ();

		// Create a battle status container
		GameObject battleStatusObject = new GameObject ();
		battleStatus = battleStatusObject.AddComponent<CharacterBattleStatus> ();
		battleStatusObject.transform.SetParent (this.gameObject.transform);

		// Create a skill handler
		GameObject skillHandlerObject = new GameObject ();
		skillHandler = skillHandlerObject.AddComponent<SkillHandler> ();
		skillHandlerObject.transform.SetParent (this.gameObject.transform);
	}

	void FixedUpdate(){
		if (gameObject.GetComponent<Character> ().isDead || bS.TurnLeft <= 0) {
			return;
		}

		// Handle The Attack Action and Calculate the danage.
		if(timer.CheckTime(1.0f)){
			if (target != null && !isAttacked) {
				int Damage;
				int RecoverHp;
				bool isCrit;
				float TempRandomNum = Random.Range (0, 99);
				if (TempRandomNum < gameObject.GetComponent<Character> ().cR) {
					Damage = ((int)(gameObject.GetComponent<Character> ().atk * (gameObject.GetComponent<Character> ().cD / 100))) - target.GetComponent<Character> ().def;
					isCrit = true;
				} else {
					Damage = gameObject.GetComponent<Character> ().atk - target.GetComponent<Character> ().def;
					isCrit = false;
				}

				// Handle Character Skill Active.
				if (gameObject.GetComponent<Character> ().isMainCharacter == true) {
					Debug.Log ("Is MainCharacter Checked.");
						gameObject.GetComponent<MainCharacter> ().SkillTree = GameObject.Find ("SkillTree").GetComponent<SkillTree> ();
						SkillTree MCST = gameObject.GetComponent<MainCharacter> ().SkillTree;
					if (MCST.CurrentTreeElement [1].isSkillOn) {
						Debug.Log ("Skill On Checked.");
						if (MCST.CurrentTreeElement [10].isLearned) {
							Debug.Log ("Skill Learn Checked.");
							if (MCST.CurrentTreeElement [24].isLearned) {
								// 'Fatal Blow' Skill On and 'Fatal Blow+' and 'Sunder' are Learned.
								Debug.Log ("Skill On With Fatal Blow+ and Sunder.");
								RecoverHp = (int)(target.GetComponent<Character> ().CurrentHp * 0.05f);
								Damage = ((int)(gameObject.GetComponent<Character> ().atk * (gameObject.GetComponent<Character> ().cD / 100)) + gameObject.GetComponent<Character> ().Lv * 3 + (int)RecoverHp) - (target.GetComponent<Character> ().def - (target.GetComponent<Character> ().def / 4));
								gameObject.GetComponent<Character> ().GetHeal (RecoverHp);
								isCrit = true;
								MCST.CurrentTreeElement [1].isSkillOn = false;
								gameObject.GetComponent<Character> ().EnergyLost ((int)MCST.CurrentTreeElement [1].energyNeed);
							} else if (MCST.CurrentTreeElement [25].isLearned) {
								// 'Fatal Blow' Skill On and 'Fatal Blow+' and 'Blast Blow+' are Learned.
								Debug.Log ("Skill On With Fatal Blow+ and Blast Blow+.");
								RecoverHp = (int)(target.GetComponent<Character> ().CurrentHp * 0.05f);
								Damage = ((int)(gameObject.GetComponent<Character> ().atk * (gameObject.GetComponent<Character> ().cD / 100)) + gameObject.GetComponent<Character> ().Lv * 3 + (int)RecoverHp - target.GetComponent<Character> ().def) + Mathf.RoundToInt(MCST.CurrentTreeElement [25].addvalue);
								gameObject.GetComponent<Character> ().GetHeal (RecoverHp);
								isCrit = true;
								MCST.CurrentTreeElement [1].isSkillOn = false;
								gameObject.GetComponent<Character> ().EnergyLost ((int)MCST.CurrentTreeElement [1].energyNeed);
							} else {
								// 'Fatal Blow' Skill On and 'Fatal Blow+' is Learned.
								Debug.Log ("Skill On With Fatal Blow+");
								RecoverHp = (int)(target.GetComponent<Character> ().CurrentHp * 0.05f);
								Damage = ((int)(gameObject.GetComponent<Character> ().atk * (gameObject.GetComponent<Character> ().cD / 100)) + gameObject.GetComponent<Character> ().Lv * 3 + (int)RecoverHp) - target.GetComponent<Character> ().def;
								gameObject.GetComponent<Character> ().GetHeal (RecoverHp);
								isCrit = true;
								MCST.CurrentTreeElement [1].isSkillOn = false;
								gameObject.GetComponent<Character> ().EnergyLost ((int)MCST.CurrentTreeElement [1].energyNeed);
							}
						} else if (MCST.CurrentTreeElement [11].isLearned) {
							if (MCST.CurrentTreeElement [24].isLearned) {
								// 'Fatal Blow' Skill On and 'Blast Blow' and 'Sunder' are Learned.
								Debug.Log ("Skill On With Blast Blow and Sunder.");
								RecoverHp = (int)(target.GetComponent<Character> ().CurrentHp * 0.05f);
								Damage = ((int)(gameObject.GetComponent<Character> ().atk * (gameObject.GetComponent<Character> ().cD / 100)) + gameObject.GetComponent<Character> ().Lv * 3 + (int)RecoverHp) - (target.GetComponent<Character> ().def - (target.GetComponent<Character> ().def / 4));
								gameObject.GetComponent<Character> ().GetHeal (RecoverHp);
								isCrit = true;
								MCST.CurrentTreeElement [1].isSkillOn = false;
								gameObject.GetComponent<Character> ().EnergyLost ((int)MCST.CurrentTreeElement [1].energyNeed);
							} else if (MCST.CurrentTreeElement [25].isLearned) {
								// 'Fatal Blow' Skill On and 'Blast Blow' and 'Blast Blow+' are Learned.
								Debug.Log ("Skill On With Blast Blow and Blast Blow+.");
								RecoverHp = (int)(target.GetComponent<Character> ().CurrentHp * 0.05f);
								Damage = ((int)(gameObject.GetComponent<Character> ().atk * (gameObject.GetComponent<Character> ().cD / 100)) + gameObject.GetComponent<Character> ().Lv * 3 + (int)RecoverHp - target.GetComponent<Character> ().def) + Mathf.RoundToInt(MCST.CurrentTreeElement [25].addvalue);
								gameObject.GetComponent<Character> ().GetHeal (RecoverHp);
								isCrit = true;
								MCST.CurrentTreeElement [1].isSkillOn = false;
								gameObject.GetComponent<Character> ().EnergyLost ((int)MCST.CurrentTreeElement [1].energyNeed);
							} else {
								// 'Fatal Blow' Skill On and 'Blast Blow' is Learned.
								Debug.Log ("Skill On With Blast Blow");
								RecoverHp = (int)(target.GetComponent<Character> ().CurrentHp * 0.05f);
								Damage = ((int)(gameObject.GetComponent<Character> ().atk * (gameObject.GetComponent<Character> ().cD / 100)) + gameObject.GetComponent<Character> ().Lv * 3 + (int)RecoverHp) - target.GetComponent<Character> ().def;
								gameObject.GetComponent<Character> ().GetHeal (RecoverHp);
								isCrit = true;
								//Debug.Log (gameObject);
								//Debug.Log (gameObject.GetComponent<MainCharacter> ());
								//Debug.Log (gameObject.GetComponent<MainCharacter> ().SkillTree);
								//Debug.Log (gameObject.GetComponent<MainCharacter> ().SkillTree.CurrentTreeElement [1]);
								MCST.CurrentTreeElement [1].isSkillOn = false;
								Debug.Log (gameObject);
								gameObject.GetComponent<Character> ().EnergyLost ((int)MCST.CurrentTreeElement [1].energyNeed);
								battleStatus.createNewStatus (Status.StatusType.SPEEDUP, gameObject.GetComponent<Character>(), 50, 4);
							}
						
						} else {
							// 'Fatal Blow' Skill On, But Not any additional Skill are Learned.
							Debug.Log ("Skill On");
							Damage = ((int)(gameObject.GetComponent<Character> ().atk * (gameObject.GetComponent<Character> ().cD / 100)) + gameObject.GetComponent<Character> ().Lv * 3) - target.GetComponent<Character> ().def;
							isCrit = true;
							MCST.CurrentTreeElement [1].isSkillOn = false;
							gameObject.GetComponent<Character> ().EnergyLost ((int)MCST.CurrentTreeElement [1].energyNeed);
						}
						Image tempImage = bS.skillbtn.GetComponent<Image> ();
						var TempColor = tempImage.color;
						TempColor.a = 0.5f;
						GameObject TempObject = new GameObject ();
						TempObject.name = "cooldown";
						TempObject.transform.SetParent (tempImage.transform);
						TempObject.transform.position = tempImage.gameObject.transform.position;
						Text TempCD = TempObject.AddComponent<Text> ();
						//TempCD. = TextAnchor.MiddleCenter;
						//gameObject.GetComponent<MainCharacter> ().SkillTree = GameObject.Find ("SkillTree").GetComponent<SkillTree> ();
						if (MCST.CurrentTreeElement [1].currentCD == 0) {
							MCST.CurrentTreeElement [1].startCD ();
						}
						TempCD.text = MCST.CurrentTreeElement [1].currentCD.ToString();
						TempCD.color = Color.red;
						TempCD.fontSize = 500;
						tempImage.color = TempColor;

					}
				}

				//Debug.Log (gameObject);
				//Debug.Log (target);
				//Debug.Log (gameObject.GetComponent<Character> ().atk);
				//Debug.Log (target.GetComponent<Character> ().def);
				if (Damage < 1)
					Damage = 1;
				target.GetComponent<Character> ().GetHurt (Damage);
				GameObject DamageText = new GameObject ();
				DamageText.name = "DamageText";
				DamageText.transform.SetParent (target.transform);
				DamageText.transform.position = new Vector3 (target.transform.position.x + 25f, target.transform.position.y + 50f, target.transform.position.z - 20f);
				DamageText.transform.eulerAngles = new Vector3 (DamageText.transform.eulerAngles.x + 0, DamageText.transform.eulerAngles.y - 98, DamageText.transform.eulerAngles.z + 0);
				var DamageTextMesh = DamageText.AddComponent<TextMesh> ();
				DamageTextMesh.color = Color.red;
				DamageTextMesh.fontSize = 250;
				if (isCrit) {
					switch (GameManager.GM.language) {
					case GameManager.Language.English:
						DamageTextMesh.text = "Crit! " + Damage.ToString ();
						break;
					case GameManager.Language.TraditionalChinese:
						DamageTextMesh.text = "暴擊! " + Damage.ToString ();
						break;
					}
				}
				else
					DamageTextMesh.text = Damage.ToString ();
				Destroy (DamageText, 0.5f);
				target.GetComponent<Attack> ().Anim.SetTrigger ("Hit");
				canAttack = false;
				isAttacked = true;
				if (target.GetComponent<Character> ().CurrentHp <= 0)
					target.GetComponent<Character> ().isDead = true;
				//bS.EnemyGroupClass.Reload ();
				//bS.PlayerGroupClass.Reload ();
			}
		}

	
		// After Attack, return the character to the position
		if (timer.CheckTime (1.75f)) {
			isAttacked = false;
			try{
			battleStatus.roundCounting ();
			battleStatus.updateStatusList ();
			}catch(System.Exception e){
				Debug.Log ("Can't find Battle Status Class..." + e.ToString ());
			}
			Anim.ResetTrigger ("Walk");
			Anim.ResetTrigger ("Hit");
			timer.CancleTimer ();
			AttackerPosition = AttackerStartPosition;
			gameObject.transform.position = new Vector3 (AttackerPosition.x, AttackerPosition.y, AttackerPosition.z);

			for (int i = 0; i < bS.EnemyGroupObject.Length; i++) {
				if (bS.EnemyGroupObject [i] != null && !gameObject.GetComponent<Character> ().isDead)
					bS.EnemyGroupObject [i].GetComponent<Attack> ().canRecharge = true;
			}
			for (int i = 0; i < bS.PlayerGroupObject.Length; i++) {
				if (bS.PlayerGroupObject [i] != null && !gameObject.GetComponent<Character> ().isDead)
					bS.PlayerGroupObject [i].GetComponent<Attack> ().canRecharge = true;
			}

			if (bS.turn == BattleSystem.Turn.Player)
				bS.EnemyStatus.SetActive (false);
			
			bS.status = BattleSystem.Status.Running;
			bS.TurnLeft--;
			SkillTree ST;
			NormalCharacterSkillList NCSL;
			ST = GameObject.Find ("SkillTree").GetComponent<SkillTree> ();
			NCSL = GameObject.Find ("NormalSkillList").GetComponent<NormalCharacterSkillList> ();
			for (int i = 0; i < ST.CurrentTreeElement.Length; i++) {
				try{
					if(ST.CurrentTreeElement[i].currentCD > 0){
						ST.CurrentTreeElement[i].currentCD--;
						TextMesh CDtext = bS.skillbtn.transform.GetChild(0).GetComponent<TextMesh>();
						CDtext.text = 
							gameObject.GetComponent<MainCharacter> ().SkillTree.CurrentTreeElement [1].currentCD.ToString();
						if(ST.CurrentTreeElement[i].currentCD == 0){
							Destroy(CDtext.gameObject);
						}
					}
				}catch(System.Exception e){
					Debug.Log ("This Not a Active Skill / Some Bugs are happen!");
				}
			}
	}

		bool TempBool = false;


		if (!canAttack && !canMove && target != null && !isAttacked)
			target = null;

		//Debug.Log ("Move" + canMove);
		//Debug.Log (!gameObject.GetComponent<Character> ().isDead || !gameObject.GetComponent<MainCharacter> ().isDead);
		//Debug.Log(GameManager.GM + "  " + GameManager.GM.battleOutcome);
		if (MovementSpeed < charClass.movspeed && canRecharge && GameManager.GM.battleOutcome == GameManager.BattleOutCome.NotReady) {
			MovementSpeed += Time.fixedDeltaTime;
			//Debug.Log (MovementSpeed);
		} 

		if(MovementSpeed >= charClass.movspeed && !canAttack) {
			bS.CurrentCharacter = gameObject;
			canAttack = true;
			if (gameObject.GetComponent<Character> ().controller == Character.Controller.Computer) {
				for (int i = 0; i < GameObject.Find ("EnemyGroup").gameObject.transform.childCount; i++) {
					if (GameObject.Find ("EnemyGroup").gameObject.transform.GetChild (i).GetComponent<Attack> ().canAttack) {
						bS.turn = BattleSystem.Turn.Enemy;
						break;
					}
				}
				canMove = true;
			}
			
			if (gameObject.GetComponent<Character> ().controller == Character.Controller.Player && !gameObject.GetComponent<Character> ().isDead) {
				for (int i = 0; i < GameObject.Find ("PlayerGroup").gameObject.transform.childCount; i++) {
					if (GameObject.Find ("PlayerGroup").gameObject.transform.GetChild (i).GetComponent<Attack> ().canAttack) {
						bS.turn = BattleSystem.Turn.Player;
						break;
					}
				}
				canMove = true;
				if (target == null) {
					randomnum = Random.Range (0, bS.EnemyGroupObject.Length);
					if (bS.EnemyGroupObject [randomnum].GetComponent<Character>().isDead != true) {
						target = bS.EnemyGroupObject [randomnum].gameObject;
						targetPosition = bS.EnemyGroupObject [randomnum].gameObject.transform.position;
					}
				}
			}
			NormalCharacterSkillList.NCSL.currentCharacter = this.gameObject.GetComponent<Character>();

			//Debug.Log (bS.turn);
			//Debug.Log (bS.PlayerGroupObject[0]);
			for (int i = 0; i < bS.EnemyGroupObject.Length; i++) {
				if(bS.EnemyGroupObject [i] != null && !gameObject.GetComponent<Character> ().isDead)
					bS.EnemyGroupObject [i].GetComponent<Attack> ().canRecharge = false;
			}
			for (int i = 0; i < bS.PlayerGroupObject.Length; i++) {
				if(bS.PlayerGroupObject [i] != null && !gameObject.GetComponent<Character> ().isDead)
					bS.PlayerGroupObject [i].GetComponent<Attack> ().canRecharge = false;
			}
		}

		if (canMove && canAttack && bS.turn == BattleSystem.Turn.Player) {
			if (Mathf.Abs (AttackerPosition.z - targetPosition.z) > 60f) {
				AttackerPosition.z -= 3f;
				Anim.SetTrigger ("Walk");
				//Debug.Log ("Called");
			} else {
				zPositionDone = true;
			}

			if (AttackerPosition.x != targetPosition.x) {
				if (AttackerPosition.x > targetPosition.x || AttackerPosition.x < targetPosition.x)
					AttackerPosition.x = targetPosition.x;
				else if (AttackerPosition.x > targetPosition.x && targetPosition.x < AttackerPosition.x)
					AttackerPosition.x -= 2f;
				else if (AttackerPosition.x < targetPosition.x && targetPosition.x < AttackerPosition.x)
					AttackerPosition.x += 2f;
				//Debug.Log ("Called");
			} else
				xPositionDone = true;
			gameObject.transform.position = new Vector3 (AttackerPosition.x, AttackerPosition.y, AttackerPosition.z);

			if (xPositionDone && zPositionDone && !TempBool) {
				Anim.ResetTrigger ("Walk");
				Anim.SetTrigger ("Attack 01");
				TempBool = true;
				canMove = false;
				MovementSpeed = 0f;
				xPositionDone = false;
				zPositionDone = false;
				timer.InitializeStart ();

			}
		}

		if (canMove && canAttack && bS.turn == BattleSystem.Turn.Enemy) {
			if (target == null) {
					randomnum = Random.Range (0, bS.PlayerGroupObject.Length);
				if (bS.PlayerGroupObject [randomnum].GetComponent<Character>().isDead != true) {
						target = bS.PlayerGroupObject [randomnum].gameObject;
						targetPosition = bS.PlayerGroupObject [randomnum].gameObject.transform.position;
					}
			}
			//Debug.Log (AttackerPosition.x);
			//Debug.Log (targetPosition.x);
			if (Mathf.Abs (AttackerPosition.z - targetPosition.z) > 51f) {
				AttackerPosition.z += 3f;
				Anim.SetTrigger ("Walk");
			} else
				zPositionDone = true;

			if (AttackerPosition.x != targetPosition.x) {
				if (AttackerPosition.x > targetPosition.x || AttackerPosition.x < targetPosition.x)
					AttackerPosition.x = targetPosition.x;
				else if (AttackerPosition.x > targetPosition.x && targetPosition.x < AttackerPosition.x)
					AttackerPosition.x += 2f;
				else if (AttackerPosition.x < targetPosition.x && targetPosition.x < AttackerPosition.x)
					AttackerPosition.x -= 2f;
			}else xPositionDone = true;
			gameObject.transform.position = new Vector3 (AttackerPosition.x, AttackerPosition.y, AttackerPosition.z);

			if (xPositionDone && zPositionDone && !TempBool) {
				Anim.ResetTrigger ("Walk");
				Anim.SetTrigger ("Attack 01");
				TempBool = true;
				canMove = false;
				MovementSpeed = 0f;
				xPositionDone = false;
				zPositionDone = false;
				timer.InitializeStart ();
			}
		}			
	}

	public void SetTarget(Character character){
		target = character.gameObject;
	}
}
