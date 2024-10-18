using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Damaged,
        Die
    };

    //������Ʈ ����
    public Slider HPBar;
    public Slider StunHPBar;
    Animator ani;
    NavMeshAgent agent;

    //���� state
    EnemyState _state;

    //�ӵ�
    public float moveSpeed = 3.0f;
    //���� �Ÿ�
    public float MoveDis = 10.0f;
    //���� �Ÿ�
    public float AttackDis = 2.0f;
    //����
    Vector3 StartPos;
    Quaternion StartRot;

    //��ǥ
    GameObject Player;

    //���� ����
    float currentTime;
    public float AttackDelay = 2.0f;

    //�������ͽ�
    public int AttackPower = 10;
    public int MaxHP = 500;
    int HP;
    public int StunMaxHP = 30;
    int StunHP;

    // Start is called before the first frame update
    void Start()
    {
        //������Ʈ ����
        Player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();

        //���� ����
        HP = MaxHP;
        StunHP = StunMaxHP;
        agent.speed = moveSpeed;
        _state = EnemyState.Idle;
        StartPos = transform.position;
        StartRot = transform.rotation;
        HPBar.value = (float)HP / (float)MaxHP;
        StunHPBar.value = (float)StunHP / (float)StunMaxHP;

        //if(GameManager.GM.Monster.Contains(this.gameObject))
        GameManager.GM.Monster.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.GM.gameState == GameManager.GameState.Stop)
        {
            return;
        }

        switch (_state)
        {
            case EnemyState.Idle:
                Idle();
                break;

            case EnemyState.Move:
                Move();
                break;

            case EnemyState.Attack:
                Attack();
                break;

            case EnemyState.Damaged:
                break;

            case EnemyState.Die:
                break;
        }
    }

    void Idle()
    {
        float distance = Vector3.Distance(Player.transform.position, transform.position);
        if (distance < MoveDis || HP != MaxHP)
        {
            ani.SetBool("Run", true);
            _state = EnemyState.Move;
        }
    }

    void Move()
    {
        float distance = Vector3.Distance(Player.transform.position, transform.position);

        if (distance > AttackDis)
        {
            agent.stoppingDistance = AttackDis;
            agent.destination = Player.transform.position;
            if(agent.areaMask == 2)
            {

            }
        }

        else
        {
            ani.SetBool("Run", false);
            _state = EnemyState.Attack;
            currentTime = AttackDelay;
        }
    }

    void Attack()
    {

        currentTime += Time.deltaTime;
        if (currentTime > AttackDelay)
        {
            StartCoroutine(AttackAnimation());
        }
        float distance = Vector3.Distance(Player.transform.position, transform.position);
        if (distance >= AttackDis)
        {
            ani.SetBool("Run", true);
            _state = EnemyState.Move;
        }
    }

    IEnumerator AttackAnimation()
    {
        agent.speed = 0;
        ani.SetTrigger("Attack");
        currentTime = 0.0f;
        yield return new WaitForSeconds(AttackDelay);
        agent.speed = moveSpeed;
    }

    void Damaged(EnemyState mode)
    {
        _state = EnemyState.Damaged;
        StartCoroutine(DamageProcess(mode));
    }

    IEnumerator DamageProcess(EnemyState mode)
    {
        agent.speed = 0;
        ani.SetTrigger("Hit");
        yield return new WaitForSeconds(2.0f);
        agent.speed = moveSpeed;
        _state = mode;
    }

    void Die()
    {
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        agent.speed = 0;
        ani.SetTrigger("Dead");
        yield return new WaitForSeconds(2.0f);
        GameManager.GM.Monster.Remove(this.gameObject);
        gameObject.SetActive(false);
    }

    public void EnamyDamaged(int damage, string tag)
    {
        //������ ���� ü�� ����ġ�� ����
        if (tag == "Shell")
        {
            HP -= damage * 1/2;
            StunHP -= 1;
        }
        if (tag == "Enemy")
        {
            HP -= damage;
            StunHP -= 2;
        }
        if (tag == "Head")
        {
            HP -= damage * 2;
            StunHP -= 3;
        }

        //���ҵ� ���� UI�� ǥ��
        HPBar.value = (float)HP / (float)MaxHP;
        StunHPBar.value = (float)StunHP / (float)StunMaxHP;

        //����ġ�� 0�� �Ǹ� ���� ����
        if (StunHP <= 0 )
        {
            StunMaxHP = (int)(StunMaxHP * 1.2);
            StunHP = StunMaxHP;
            StunHPBar.value = (float)StunHP / (float)StunMaxHP;

            if (_state != EnemyState.Die)
            {
                Damaged(EnemyState.Move);
            }
        }

        //HP�� 0�� �Ǹ� ��� ó��
        if(HP <= 0 && _state != EnemyState.Die)
        {
            StopAllCoroutines();
            _state = EnemyState.Die;
            Die();
        }
    }

    void PlayerAttack()
    {
        Player.GetComponent<PlayerController>().PlayerDamaged(AttackPower);
    }
    void TurtleAttack()
    {
        GetComponent<TurtleEffect>().EffectIns(this.transform);
        Player.GetComponent<PlayerController>().PlayerDamaged(AttackPower);
    }
}