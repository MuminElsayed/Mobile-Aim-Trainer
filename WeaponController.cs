using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class WeaponController : MonoBehaviour
{
    private Weapon weapon;
    private bool isReloading, isShooting, isCharging;
    private Coroutine shootingCoro, reloadCoro, chargeCoro;
    private Animator animator;
    private int reloadingBoolHash, shotToggleHash, headshotCount = 0, bodyshotCount = 0, playerScore = 0;
    public float accuracy, shotsFired, shotsMissed;
    [SerializeField]
    private bool infiniteAmmo;
    [SerializeField]
    private GameObject reloadImage, throwablePrefab;
    [SerializeField]
    private TextMeshProUGUI ammoText, aimText, playerScoreText;
    public Transform muzzlePos;
    [SerializeField]
    private Image chargeIndicator;
    [SerializeField]
    private float chargeTime;
    private List<GameObject> throwables;
    public static Action headshot;
    public static Action<int> addScore, updateScoreAction;
    public static Action<float> updateAimAction;
    public static Action<int, int> updateAmmoAction;
    public static Action<bool> addAccuracy;
    private int difficulty;
    private int enemyLayerMask;

    void Start()
    {
        enemyLayerMask = LayerMask.GetMask("Enemy");
        difficulty = PlayerPrefs.GetInt("Difficulty", 0);
        weapon = GetComponentInChildren<Weapon>();
        animator = GetComponentInChildren<Animator>();
        reloadingBoolHash = Animator.StringToHash("Reloading");
        shotToggleHash = Animator.StringToHash("Shot");
        updateAmmoText();
        updatePlayerScore(0);
        //Pool 10 throwables
        throwables = new List<GameObject>();
        GameObject throwablesHolder = new GameObject("ThrowablesHolder");
        for (int i = 0; i < 10; i++)
        {
            throwables.Add(Instantiate(throwablePrefab, Vector3.zero, Quaternion.identity, throwablesHolder.transform));
            throwables[i].SetActive(false);
        }
        if (weapon.throwable)
        {
            chargeIndicator.fillAmount = 0;
        }
    }
    // void Update()
    // {
    //     Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100f, Color.red);
    // }
    void stopShooting()
    {
        if (isShooting)
        {
            if (shootingCoro != null)
            {
                StopCoroutine(shootingCoro);
            }
            isShooting = false;
        } else if (isCharging) {
            stopCharge();
        }
    }

    void Shoot()
    {
        if (!isShooting && !weapon.throwable)
        {
            shootingCoro = StartCoroutine(Shooting());
        }
    }

    void Charge()
    {
        //move charging mechanic here
        if (!isCharging && weapon.throwable) {
            //Charge throw
            startCharge();
        }
        // print("I didn't move charging mechanic");
    }

    IEnumerator Shooting()
    {
        isShooting = true;
        while (weapon.currentAmmo > 0 && !isReloading)
        {
            //Shooting mechanics
            animator.SetTrigger(shotToggleHash);
            if (!infiniteAmmo)
            {
                weapon.currentAmmo --;
                updateAmmoText();
            }
            AudioManager.audioManager.playClip(weapon.bulletAudio, 1);
            // playClip(weapon.bulletAudio);
            
            //Bullet mechs
            RaycastHit hit;
            Vector3 raycastPos;
            Vector3 raycastDir;
            if (!PlayerController.instance.topDownMode)
            {
                raycastPos = Camera.main.transform.position;
                raycastDir = Camera.main.transform.forward;
            } else { //Top Down
                raycastPos = transform.position;
                raycastDir = transform.forward;
            }
            if (Physics.Raycast(raycastPos, raycastDir, out hit, 100f, enemyLayerMask)) //Shoots ray from gun to center of screen
            {
                if (hit.collider.CompareTag("Head"))
                {
                    headshot();
                    headshotCount++;
                    addScore(500 * (difficulty + 1));
                    hit.transform.root.gameObject.SetActive(false);
                    // print("hit head");
                } else if (hit.collider.CompareTag("Body")) {
                    bodyshotCount++;
                    addScore(250 * (difficulty + 1));
                    hit.transform.root.gameObject.SetActive(false);
                    // print("hit bodu");
                }
                //Add bullet impact
            } else { //Didn't hit enemy
                    //Lower aim % and score
                shotsMissed++;
                addScore(-50 * (difficulty + 1));
            }
            shotsFired++;
            updateAimText();
            updatePlayerScore(playerScore);
            yield return new WaitForSeconds(weapon.fireRate);
        }
        if (weapon.currentAmmo == 0 && !isReloading)
            {
                reloadCoro = StartCoroutine(Reload(weapon.reloadTime));
                stopShooting();
            }
    }

    void startCharge()
    {
        isCharging = true;
        chargeCoro = StartCoroutine(percentOverTime(chargeTime));
    }

    IEnumerator percentOverTime(float totalDuration)
    {
        float startTime = Time.time;
        float percentDone = 0;
        while (Time.time < startTime + totalDuration)
        {
            percentDone = (Time.time - startTime)/totalDuration;
            chargeIndicator.fillAmount = percentDone;
            yield return new WaitForEndOfFrame();
        }
    }

    void shootCharge()
    {
        if (!isShooting && weapon.throwable)
        {
            stopCharge();
            isShooting = true;
            GameObject obj = getFreeThrowable();
            obj.transform.position = transform.position + transform.forward;
            obj.transform.forward = transform.forward;
            obj.SetActive(true);
            StartCoroutine(reloadCharge());
        }
    }

    IEnumerator reloadCharge()
    {
        yield return new WaitForSeconds(0.25f);
        isShooting = false;
    }

    void stopCharge()
    {
        chargeIndicator.fillAmount = 0;
        if (chargeCoro != null)
        {
            StopCoroutine(chargeCoro);
        }
        isCharging = false;
    }

    GameObject getFreeThrowable() //Return an inactive throwable
    {
        foreach (GameObject throwableObj in throwables)
        {
            if (!throwableObj.activeSelf)
            {
                return throwableObj;
            }
        }
        return null; //If all are active (should be impossible)
    }

    IEnumerator Reload(float reloadTime)
    {
        isReloading = true;
        reloadImage.SetActive(true);
        animator.SetBool(reloadingBoolHash, true);
        AudioManager.audioManager.playClip(weapon.reloadAudio, 1);
        yield return new WaitForSeconds(reloadTime);
        reloadImage.SetActive(false);
        animator.SetBool(reloadingBoolHash, false);
        weapon.currentAmmo = weapon.maxAmmo;
        updateAmmoText();
        isReloading = false;
    }
    void addPlayerScore(int score)
    {
        if (playerScore + score < 0)
        {
            playerScore = 0;
        } else {
            playerScore += score;
        }
        updatePlayerScore(playerScore);
    }
    void addPlayerAccuracy(bool shotHit)
    {
        shotsFired += 1;
        if (shotHit)
        {
            bodyshotCount ++;
        } else {
            shotsMissed ++;
        }
        updateAimText();
    }
    void updateAmmoText()
    {
        if (updateAmmoAction != null)
            updateAmmoAction(weapon.currentAmmo, weapon.maxAmmo);
    }

    void updateAimText()
    {
        accuracy = ((shotsFired - shotsMissed)/shotsFired) * 100f;
        if (updateAimAction != null)
        {
            updateAimAction(accuracy);
        }
    }
    

    void updatePlayerScore(int score)
    {
        if (updateScoreAction != null)
            updateScoreAction(score);
    }


    void sendPlayerStats()
    {
        PostGameCanvas.instance.playerShotsFired = (int)shotsFired;
        PostGameCanvas.instance.playerHeadshotCount = headshotCount;
        PostGameCanvas.instance.playerBodyshotCount = bodyshotCount;
        PostGameCanvas.instance.playerShotsMissed = (int)shotsMissed;
        PostGameCanvas.instance.playerAccuracy = accuracy;
        PostGameCanvas.instance.playerScore = playerScore;
        PostGameCanvas.instance.setStats();
    }

    void OnEnable()
    {
        PlayerController.startShooting += Shoot;
        PlayerController.startCharging += Charge;
        PlayerController.stopShooting += stopShooting;
        PlayerController.stopAim += shootCharge;
        GameManager.gameEnd += sendPlayerStats;
        addScore += addPlayerScore;
        addAccuracy += addPlayerAccuracy;
    }

    void OnDisable()
    {
        PlayerController.startShooting -= Shoot;
        PlayerController.startCharging -= Charge;
        PlayerController.stopShooting -= stopShooting;
        PlayerController.stopAim -= shootCharge;
        GameManager.gameEnd -= sendPlayerStats;
        addScore -= addPlayerScore;
        addAccuracy -= addPlayerAccuracy;
    }
}
