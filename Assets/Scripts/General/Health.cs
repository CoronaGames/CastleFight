using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core
{
    public class Health : MonoBehaviour
        {

        [SerializeField] float healthPoints;
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] bool isUnit = true;
        public Transform hpFill;
        bool isDead = false;
        bool dotActive = false;
        [SerializeField]  Text hpText;
        [SerializeField] float hpTextTimer = 0.5f;
        float hpTextTimerCurrent = 0f;
        [SerializeField] int XPvalue;
        [SerializeField] int moneyValue;

        private void Start()
        {
            healthPoints = maxHealthPoints;
            hpText.gameObject.SetActive(false);
        }

        public int GetXPvalue()
        {
            return XPvalue;
        }

        private void Update()
        {
            Timers();
        }

        public float GetHp()
        {
            return healthPoints;
        }

        private void Timers()
        {
            if (hpText.gameObject.activeInHierarchy)    // Shows health text
            {
               if(hpTextTimerCurrent < hpTextTimer)
                {
                    hpTextTimerCurrent += Time.deltaTime;
                }
                else
                {
                    hpTextTimerCurrent = 0f;
                    hpText.gameObject.SetActive(false);
                }
            }
        }

        public void TakeDamage(float damage)
        {
            if (damage <= 0) return;
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints <= 0f)
            {
                Die();
            }
            else
            {
                GetComponent<Animator>().SetTrigger("Hurt");
                
            }
            SetDamageText(damage, false);
            SetHpFill();
        }

        private void SetDamageText(float amount, bool isHealing)
        {
            if (healthPoints <= 0)
            {
                Die();
            }
            hpText.gameObject.SetActive(true);
            if (isHealing)
            {
                hpText.text = "+" + amount.ToString();
                hpText.color = Color.green;
            }
            else
            {
                hpText.text = "-" + amount.ToString();
                hpText.color = Color.red;
            }
        }


        public bool IsDead()
        {
            return isDead;
        }

        public void Die()
        {
            if (isDead) return;
            if (GetComponent<TeamData>())
            {
                if(GetComponent<TeamData>().GetTeamBelonging() == Team.TeamRed)
                {
                    CastleFightData.instance.RemoveOnePlayerUnitCount();
                }
            }
            if (GetComponent<Mover>()) GetComponent<Mover>().Cancel();
            if (CastleFightData.instance != null) CheckToAddPlayerMoney();
            hpFill.parent.gameObject.SetActive(false);
            SetAllCollidersStatus(false);
            GetComponent<Animator>().SetTrigger("Die");
            isDead = true;
        }

        public void SetAllCollidersStatus(bool active)
        {
            foreach (Collider c in GetComponents<Collider>())
            {
                c.enabled = active;
            }
        }

        private void CheckToAddPlayerMoney()
        {
            if (GetComponent<TeamData>().GetTeamBelonging() == Team.TeamRed) return;    // Player wont get gold for losing their own units
            if(moneyValue > 0)
            {
                CastleFightData.instance.AddMoney(moneyValue);
            }
        }

        private void CheckToAddPlayerXP()
        {
            Debug.Log("CheckToAddPlayerXP");
            /*
            if(leveling.GetComponent<Health>().GetTeamNumber() != GetTeamNumber())
            {
                leveling.AddXP(XPvalue);
            }
            */
        }

        public void DestroyObject()
        {
            Destroy(gameObject);
        }

        private void SetHpFill()
        {
            float fillScale = (healthPoints / maxHealthPoints);
            if (fillScale < 0)
            {
                fillScale = 0f;
            }
            hpFill.localScale = new Vector3(fillScale, 1f, 1f);
        }


        public void Heal(float amount)
        {
            healthPoints += amount;
            if(healthPoints > maxHealthPoints)
            {
                healthPoints = maxHealthPoints;
            }
            SetHpFill();
            SetDamageText(amount, true);
        }

        public bool IsUnit()
        {
            return isUnit;
        }
    }

   

}
