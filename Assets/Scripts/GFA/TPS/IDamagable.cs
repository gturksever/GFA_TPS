using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GFA.TPS
{
    public interface IDamagable
    {
        void ApplyDamage(float damage,GameObject causer = null);
    }
}

