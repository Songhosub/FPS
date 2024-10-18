using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    enum WeaponMode
    {
        Rifle,
        Bomb,
        Sniper
    };

    //공용
    WeaponMode Mode;
    Transform FirePos;
    int AttackPower;
    float CurrentTime = 0;
    float AttackDelay;
    int MaxHP = 150;
    int HP;
    int Mag;
    public bool Reload;
    bool Change;
    GameObject Crosshair;
    GameObject SnipeCrosshair;
    Animator ani;
    GameObject CameraPos;

    //이펙트
    Transform MuzzlePos;
    public GameObject[] MuzzleFlash;
    public GameObject bulletEffect;

    //라이플
    public GameObject RifleObj;
    float RifleDelay = 0.1f;
    int RiflekPower = 3;
    int RifleMag = 30;

    //수류탄
    public GameObject BombObj;
    float BombDelay = 0.7f;
    int BombPower = 250;
    float ExplosionRadius = 3f;
    int BombCount = 7;

    //스나이프
    public GameObject SniperObj;
    float ScopeFOV;
    float ScopeNear;
    float SnipeDelay = 2.0f;
    int SnipePower = 75;
    bool Scope;
    int SnipeMag = 5;

    //이동
    public float MoveSpeed = 5;
    Vector3 dir;

    //점프
    public float JumpSpeed = 3;
    bool JumpCheck;

    //회전
    public float RotateSpeed = 60;

    // Start is called before the first frame update
    void Start()
    {
        //컴포넌트 접근
        FirePos = Camera.main.transform;
        Crosshair = GameObject.Find("Crosshair");
        SnipeCrosshair = GameObject.Find("SnipeCrosshair");
        MuzzlePos = GameObject.Find("Assault/MuzzleFlash").transform;
        ani = GetComponent<Animator>();
        CameraPos = GameObject.Find("CameraPosition");

        //변수 설정
        ScopeFOV = Camera.main.fieldOfView;
        ScopeNear = Camera.main.nearClipPlane;
        Mode = WeaponMode.Rifle;
        AttackPower = RiflekPower;
        AttackDelay = RifleDelay;
        CurrentTime = AttackDelay;
        HP = MaxHP;
        Scope = false;
        JumpCheck = false;
        Mag = RifleMag;
        Reload = false;
        Change = false;

        //초기화
        SnipeCrosshair.SetActive(false);
        SniperObj.SetActive(false);
        GameManager.GM.HPUI(HP, MaxHP);
        GameManager.GM.MagText(Mag);
        GameManager.GM.CountText(BombCount);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GM.gameState == GameManager.GameState.Stop)
        {
            return;
        }

        //회전
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetMouseButtonDown(0))
        {
            float mx = Camera.main.transform.eulerAngles.y;
            transform.eulerAngles = new Vector3(0, mx, 0);
        }

        //점프
        if (Input.GetButtonDown("Jump") && Mathf.Abs(GetComponent<Rigidbody>().velocity.y) <= 0.5f && !Reload && !JumpCheck)
        {
            GetComponent<Rigidbody>().velocity = Vector3.up * JumpSpeed;
            JumpCheck = true;
            ani.SetTrigger("Jump");
        }

        //이동
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        h = h * MoveSpeed * Time.deltaTime;
        v = v * MoveSpeed * Time.deltaTime;
        this.transform.Translate(Vector3.right * h);
        this.transform.Translate(Vector3.forward * v);
        ani.SetFloat("Blend", Input.GetAxis("Vertical"));

        //공격
        CurrentTime += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && CurrentTime >= AttackDelay && !Reload && !Change)
        {
            switch (Mode)
            {
                case WeaponMode.Rifle:
                    Rifle();
                    break;

                case WeaponMode.Bomb:
                    Bomb();
                    break;

                case WeaponMode.Sniper:
                    Snipe();
                    break;
            }
        }

        //줌 인
        if (Mode == WeaponMode.Sniper && !Change)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Scope = true;
                Camera.main.fieldOfView = ScopeFOV - 50.0f;
                Camera.main.nearClipPlane = ScopeNear + 0.8f;
                Crosshair.SetActive(false);
                SnipeCrosshair.SetActive(true);
                SniperObj.SetActive(false);
                MoveSpeed = 1;
                Camera.main.GetComponent<CameraController>().RotateSpeedChange(30.0f);
                ani.SetBool("SniperMode", true);
            }
            else
            {
                Scope = false;
                Camera.main.fieldOfView = ScopeFOV;
                Camera.main.nearClipPlane = ScopeNear;
                SnipeCrosshair.SetActive(false);
                Crosshair.SetActive(true);
                SniperObj.SetActive(true);
                MoveSpeed = 5;
                Camera.main.GetComponent<CameraController>().RotateSpeedChange(120.0f);
                ani.SetBool("SniperMode", false);
            }
        }

        if(!Scope && !Reload && !Change)
        {
            ModeChange();
        }
    }

    void Rifle()
    {
        RaycastHit hit;
        CurrentTime = 0;
        Mag -= 1;
        GameManager.GM.MagText(Mag);
        StartCoroutine(MuzzleEffect());
        if (Physics.Raycast(FirePos.transform.position, FirePos.transform.forward, out hit, 15))
        {
            bulletEffect.transform.position = hit.point;
            bulletEffect.transform.forward = hit.normal;
            bulletEffect.GetComponent<ParticleSystem>().Play();
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                hit.collider.gameObject.transform.root.gameObject.GetComponent<EnemyController>().EnamyDamaged(AttackPower, hit.collider.tag);
            }
        }
        if(Mag == 0)
        {
            StartCoroutine(ReloadCR());
        }
    }

    void Bomb()
    {
        if(BombCount > 0)
        {
            CurrentTime = 0;
            BombCount -= 1;
            GameManager.GM.CountText(BombCount);
            Vector3 pos = FirePos.position;
            pos += FirePos.right * 0.5f;
            pos += FirePos.up * 0.5f;
            GameObject bomb = Instantiate(BombObj, pos, Quaternion.identity);
            bomb.GetComponent<Rigidbody>().useGravity = true;
            bomb.GetComponent<Rigidbody>().AddForce((FirePos.up / 2 + FirePos.forward) * 600);
        }
    }

    public void Explosion(Vector3 ExPos)
    {
        Collider[] Col = Physics.OverlapSphere(ExPos, ExplosionRadius, LayerMask.GetMask("Enemy"));
        foreach (Collider c in Col)
        {
            if (c.gameObject.tag == "Head" || c.gameObject.tag == "Enemy" || c.gameObject.tag == "Shell")
            {
                c.gameObject.transform.root.gameObject.GetComponent<EnemyController>().EnamyDamaged(AttackPower, null);
                break;
            }
        }
    }

    void Snipe()
    {
        RaycastHit hit;
        CurrentTime = 0;
        Mag -= 1;
        GameManager.GM.MagText(Mag);
        if (Scope == false)
        {
            StartCoroutine(MuzzleEffect());
        }
        if (Physics.Raycast(FirePos.transform.position, FirePos.transform.forward, out hit, 45))
        {
            bulletEffect.transform.position = hit.point;
            bulletEffect.transform.forward = hit.normal;
            bulletEffect.GetComponent<ParticleSystem>().Play();
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                hit.collider.gameObject.transform.root.gameObject.GetComponent<EnemyController>().EnamyDamaged(AttackPower, hit.collider.tag);
            }
        }
        if (Mag == 0)
        {
            StartCoroutine(ReloadCR());
        }
    }

    IEnumerator ReloadCR()
    {
        Reload = true;
        ani.SetTrigger("Reload");
        if(Mode == WeaponMode.Rifle)
        {
            RifleObj.GetComponent<Assault>().Reload();
        }
        else if(Mode == WeaponMode.Sniper && Scope == false)
        {
            SniperObj.GetComponent<Sniper>().Reload();
        }
        yield return new WaitForSeconds(2.65f);
        if(Mode == WeaponMode.Rifle)
        {
            Mag = RifleMag;
        }
        if(Mode == WeaponMode.Sniper)
        {
            Mag = SnipeMag;
        }
        yield return null;
        GameManager.GM.MagText(Mag);
        Reload = false;
    }

    IEnumerator MuzzleEffect()
    {
        yield return null;
        Instantiate(MuzzleFlash[Random.Range(0, MuzzleFlash.Length)], MuzzlePos);
    }

    void ModeChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && Mode != WeaponMode.Rifle)
        {
            Change = true;
            StartCoroutine(ChangeRifle());
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && Mode != WeaponMode.Bomb)
        {
            Change = true;
            StartCoroutine(ChangeBomb());
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && Mode != WeaponMode.Sniper)
        {
            Change = true;
            StartCoroutine(ChangeSnipe());
        }
    }

    IEnumerator ChangeRifle()
    {
        SniperObj.SetActive(false);
        GameManager.GM.ModeImage("Rifle", 0.5f);
        yield return new WaitForSeconds(0.5f);
        AttackPower = RiflekPower;
        AttackDelay = RifleDelay;
        CurrentTime = AttackDelay;
        Mag = RifleMag;
        RifleObj.SetActive(true);
        MuzzlePos = GameObject.Find("Assault/MuzzleFlash").transform;
        ani.SetBool("BombMode", false);
        if (Mode == WeaponMode.Bomb)
        {
            CameraPos.transform.position = CameraPos.transform.position - transform.forward * 0.18f;
        }
        Change = false;
        GameManager.GM.MagText(Mag);
        Mode = WeaponMode.Rifle;
    }

    IEnumerator ChangeBomb()
    {
        SniperObj.SetActive(false);
        RifleObj.SetActive(false);
        GameManager.GM.ModeImage("Bomb", 0.5f);
        yield return new WaitForSeconds(0.5f);
        AttackPower = BombPower;
        AttackDelay = BombDelay;
        CurrentTime = AttackDelay;
        ani.SetBool("BombMode", true);
        CameraPos.transform.position = CameraPos.transform.position + transform.forward * 0.18f;
        Change = false;
        GameManager.GM.MagText(-10);
        Mode = WeaponMode.Bomb;
    }

    IEnumerator ChangeSnipe()
    {
        RifleObj.SetActive(false);
        GameManager.GM.ModeImage("Sniper", 0.5f);
        yield return new WaitForSeconds(0.5f);
        AttackPower = SnipePower;
        AttackDelay = SnipeDelay;
        CurrentTime = AttackDelay;
        Mag = SnipeMag;
        SniperObj.SetActive(true);
        MuzzlePos = GameObject.Find("Sniper/MuzzleFlash").transform;
        ani.SetBool("BombMode", false);
        if (Mode == WeaponMode.Bomb)
        {
            CameraPos.transform.position = CameraPos.transform.position - transform.forward * 0.18f;
        }
        Change = false;
        GameManager.GM.MagText(Mag);
        Mode = WeaponMode.Sniper;
    }

    public void PlayerDamaged(int damage)
    {
        HP -= damage;
        GameManager.GM.HPUI(HP, MaxHP);
        if(HP < (MaxHP / 3))
        {
            GameManager.GM.HitEffect.gameObject.SetActive(true);
        }
        else
        {
            GameManager.GM.Hit();
        }
        if (HP <= 0)
        {
            GameManager.GM.GameOver();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        JumpCheck = false;
    }
}
