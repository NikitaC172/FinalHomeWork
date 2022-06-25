using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : Weapon
{
    public override void Shoot(Transform shootPoint, Vector3 hitPoint)
    {
        if (_isReloading == false && _isReadyShoot == true)
        {
            _currentAmmo--;
            _isReadyShoot = false;

            if (_currentAmmo == 0)
            {
                _isReadyShoot = false;
            }

            shootPoint.LookAt(hitPoint);
            Instantiate(Bullet, shootPoint.transform.position, shootPoint.transform.rotation);
            Invoke(nameof(ReloadBetweenShoot), _delayBetweenShoot);
        }
        else
        {
            _isReadyShoot = false;
        }

    }

    private void ReloadBetweenShoot()
    {
        if (_currentAmmo > 0)
        {
            _isReadyShoot = true;
        }
        else
        {
            _isReadyShoot = false;
        }
    }
}
