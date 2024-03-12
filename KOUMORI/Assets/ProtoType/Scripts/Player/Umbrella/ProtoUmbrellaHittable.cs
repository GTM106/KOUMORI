using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IUmbrellaHittable
{
    void OnEnter();
    void OnExit();
    void OnStay();
}

public class ProtoUmbrellaHittable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IUmbrellaHittable hittable))
        {
            hittable.OnEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IUmbrellaHittable hittable))
        {
            hittable.OnExit();
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out IUmbrellaHittable hittable))
        {
            hittable.OnStay();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IUmbrellaHittable hittable))
        {
            hittable.OnEnter();
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IUmbrellaHittable hittable))
        {
            hittable.OnExit();
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IUmbrellaHittable hittable))
        {
            hittable.OnStay();
        }

    }
}
