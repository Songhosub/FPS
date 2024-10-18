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

    //컴포넌트 접근
    public Slider HPBar;
    public Slider StunHPBar;
    Animator ani;
    NavMeshAgent agent;

    //현재 state
    EnemyState _state;

    //속도
    public float moveSpeed = 3.0f;
    //추적 거리
    public float MoveDis = 10.0f;
    //공격 거리
    public float AttackDis = 2.0f;
    //시작
    Vector3 StartPos;
    Quaternion StartRot;

    //목표
    GameObject Player;

    //공격 동작
    float currentTime;
    public float AttackDelay = 2.0f;

    //스테이터스
    public int AttackPower = 10;
    public int MaxHP = 500;
    int HP;
    public int StunMaxHP = 30;
    int StunHP;

    // Start is called before the first frame update
    void Start()
    {
        //컴포넌트 접근
        Player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();

        //변수 설정
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
        //부위에 따라 체력 기절치를 감소
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

        //감소된 것을 UI에 표시
        HPBar.value = (float)HP / (float)MaxHP;
        StunHPBar.value = (float)StunHP / (float)StunMaxHP;

        //기절치가 0이 되면 기절 진행
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

        //HP가 0이 되면 사망 처리
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