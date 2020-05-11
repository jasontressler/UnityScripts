using UnityEngine;

[ExecuteInEditMode]
public class StatController : MonoBehaviour
{
    
    public enum Stat { HP, MP, SP }

    private Animator anim;
    private Transform hp, mp, sp, xp;
    [Header("Vital Stats")]
    public float maxHealth = 100;
    public float health = 100;
    public float maxMana = 100;
    public float mana = 100;
    public float maxStamina = 100;
    public float stamina = 100;

    [Header("Combat Stats")]
    public int attack = 1;
    public int def = 1;

    [Header("Experience")]
    public float exp = 0;
    public float expNext = 100;
    public float expPrev = 0;
    float expDiff = 100;
    public float level = 1;

    
    void Start() {
        hp = GameObject.Find("HP").transform;
        mp = GameObject.Find("MP").transform;
        sp = GameObject.Find("SP").transform;
        xp = GameObject.Find("Experience").transform;
        hp.transform.localScale = new Vector3(health / maxHealth, hp.localScale.y, hp.localScale.z);
        mp.transform.localScale = new Vector3(mana / maxMana, mp.localScale.y, mp.localScale.z);
        sp.transform.localScale = new Vector3(stamina / maxStamina, sp.localScale.y, sp.localScale.z);
        xp.transform.localScale = new Vector3(exp / expNext, xp.localScale.y, xp.localScale.z);
        anim = GetComponent<Animator>();
    }

    void LateUpdate() {
        hp.GetComponent<RectTransform>().localScale.Set(health / maxHealth, hp.transform.localScale.y, hp.transform.localScale.z);
    }

    public void Damage(float dam, Stat stat = Stat.HP) {
        float temp;
        switch (stat) {
            case Stat.HP:
                anim.SetTrigger("Hurt");
                temp = (dam - def);
                dam = temp >= 0 ? temp : 0;
                temp = health - dam;
                health = temp > 0 ? temp : 0;
                break;
            case Stat.MP:
                mana -= dam;
                temp = mana - (dam);
                health = temp > 0 ? temp : 0;
                break;
            case Stat.SP:
                temp = stamina - (dam);
                health = temp > 0 ? temp : 0;
                break;
            default:
                Debug.Log("Bad stat.");
                break;
        }
        StatBarScale(stat);
    }
    public void Recover(float rec, Stat stat = Stat.HP) {
        switch (stat) {
            case Stat.HP:
                health += rec;
                break;
            case Stat.MP:
                mana += rec;
                break;
            case Stat.SP:
                stamina += rec;
                break;
        }
        StatBarScale(stat);
    }
    public void AddExp(float exp) {
        this.exp += exp;       
        if (this.exp >= expNext) {
            LevelUp();
        }
        float progress = (this.exp - expPrev) / expDiff;
        Debug.Log(this.exp + " - " + expPrev + " = " + (this.exp-expPrev));
        Debug.Log((this.exp - expPrev) + " / " + expNext + " = " + progress);
        xp.transform.localScale = new Vector3(progress, xp.localScale.y, xp.localScale.z);
    }
    public void StatBarScale(Stat stat) {
        switch (stat) {
            case (Stat.HP):
                hp.transform.localScale = new Vector3(health / maxHealth, hp.localScale.y, hp.localScale.z);
                break;
            case (Stat.MP):
                mp.transform.localScale = new Vector3(mana / maxMana, mp.localScale.y, mp.localScale.z);
                break;
            case (Stat.SP):
                sp.transform.localScale = new Vector3(stamina / maxStamina, sp.localScale.y, sp.localScale.z);
                break;
        }
    }

    public void LevelUp() {
        level++;
        expPrev = expNext;
        expNext = calcNext();
        expDiff = expNext - expPrev;
        maxHealth *= 1.1f;
        health = maxHealth;
    }
    public float calcNext() {
        return Mathf.Round(expNext * 1.2f + 100);
    }
}
