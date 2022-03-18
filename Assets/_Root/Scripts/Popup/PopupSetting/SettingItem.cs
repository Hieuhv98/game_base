using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RescueFish.Controller 
{
    public class SettingItem : MonoBehaviour
    {
        public GameObject SelectOff;
        public GameObject SelectOn;

        public void UpdateStatus(bool status)
        {
            if (status)
            {
                SelectOn.SetActive(true);
                SelectOff.SetActive(false);
            }
            else
            {
                SelectOn.SetActive(false);
                SelectOff.SetActive(true);
            }
        }

    }
}

