using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS
{
    public class WeaponModelInstantiationSlot : MonoBehaviour
    {
        // WHAT SLOT IS THIS? LEFT OR RIGHT, HIPS, BACK ETC)

        public GameObject currentWeaponModel;
        public WeaponModelSlot weaponModelSlot;

        public void UnloadWeaponModel()
        {
            if(currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }

        public void LoadWeaponModel(GameObject weaponModel)
        {
            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;  
            weaponModel.transform.localScale = Vector3.one;
        }
    }

}