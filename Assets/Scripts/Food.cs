using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public Transform[] _transforms;
    int _indexTransforms;
    public int maxTransforms;
    public float radius;
    [SerializeField] protected LayerMask boidsMask =  1<<8;

   
    public void ChangePos()
    {
        transform.position = _transforms[_indexTransforms].position;

        _indexTransforms++;

        if (_indexTransforms >= maxTransforms)
        {
            _indexTransforms = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }




}
