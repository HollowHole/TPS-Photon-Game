using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    Animator animator;
    InputManager inputManager;
    bool fire;
    bool aim;
    [SerializeField]bool reload;//what if player hold R and never release?

    [Header("Shooting Var")]
    public Transform firePoint;
    public float fireRate = 0.23f;
    private float fireTimer = 0;
    public float fireDamage = 15;
    public float fireRange = 100f;

    [Header("Reload")]
    public int maxAmmo = 30;
    [SerializeField]private int currentAmmo;
    public float reloadCD = 2.0f;
    private float reloadTimer = 0;
    private bool isReloading = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
    }

    public void HandleAllGunActions()
    {
        ReadInput();
        HandleShoot();
        HandleReload();
        UpdateAnimation();
    }
    private void ReadInput()
    {
        if (inputManager != null)
        {
            fire = inputManager.Fire;
            aim = inputManager.Aim;
            reload = inputManager.Reload;
        }
    }
    private void UpdateAnimation()
    {
        

        animator.SetBool("Fire", fire);
        animator.SetBool("Aim",aim);

        if (reload) {Debug.Log("trigger reload"); animator.SetTrigger("Reload"); }
    }
    private void HandleShoot()
    {
        if(fireTimer>0 ) { fireTimer -= Time.deltaTime; return; }

        if (!fire) return;
        
        if(currentAmmo > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, fireRange))
            {
                Debug.Log(hit.transform.name);
            }
            fireTimer = fireRate;
            currentAmmo--;
        }
        else
        {
            reload = true;
        }
    }
    private void HandleReload()
    {
        if (!isReloading && currentAmmo < maxAmmo && reload) StartCoroutine(Reload());
        else reload = false;
    }
    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadCD);
        isReloading = false;
        currentAmmo = maxAmmo;
        StopCoroutine(Reload());
    }

}
